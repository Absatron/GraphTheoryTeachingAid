using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using TeachingAidMac.Models;

namespace TeachingAidMac.Controls
{
    public class GraphCanvas : UserControl
    {
        private Graph? _graph;
        private Canvas _canvas = new Canvas();
        private List<Line> _connectionLines = new List<Line>();
        private List<TextBlock> _weightLabels = new List<TextBlock>();

        public Graph? Graph
        {
            get => _graph;
            set
            {
                _graph = value;
                if (_graph != null)
                {
                    _graph.Canvas = _canvas;
                    _graph.OnConnectionsChanged = () => RedrawAllConnections();

                    // Add all existing nodes to canvas
                    foreach (var node in _graph.Nodes)
                    {
                        if (!_canvas.Children.Contains(node))
                        {
                            _canvas.Children.Add(node);
                        }
                    }

                    // Draw initial connections
                    RedrawAllConnections();
                }
            }
        }

        public Canvas Canvas => _canvas;

        public GraphCanvas()
        {
            _canvas.Background = Brushes.White;
            _canvas.ClipToBounds = false;
            Content = _canvas;

            _canvas.IsHitTestVisible = true;
            this.IsHitTestVisible = true;
        }

        private void RedrawAllConnections()
        {
            // Clear existing connection visuals first
            ClearAllConnections();

            if (_graph == null || _graph.Nodes.Count == 0) return;

            Console.WriteLine($"RedrawAllConnections: Drawing {_graph.Nodes.Sum(n => n.Connections.Count)} connections");

            foreach (var node in _graph.Nodes)
            {
                var nodePos = node.GetPosition();
                var startX = nodePos.X + node.Width / 2;
                var startY = nodePos.Y + node.Height / 2;

                if (nodePos.X == 0 && nodePos.Y == 0) continue;

                foreach (var connection in node.Connections)
                {
                    var endNode = connection.Target;
                    var endNodePos = endNode.GetPosition();

                    if (endNodePos.X == 0 && endNodePos.Y == 0) continue;

                    var endX = endNodePos.X + endNode.Width / 2;
                    var endY = endNodePos.Y + endNode.Height / 2;

                    // Skip self-loops (connections from a node to itself) to prevent horizontal lines
                    if (node == endNode)
                    {
                        Console.WriteLine($"  SKIPPING self-connection: {node.NodeName} -> {endNode.NodeName}");
                        continue;
                    }

                    // Skip if coordinates are identical or very close (prevents artifacts)
                    if (Math.Abs(startX - endX) < 1 && Math.Abs(startY - endY) < 1)
                    {
                        Console.WriteLine($"  SKIPPING identical coordinates: ({startX},{startY}) -> ({endX},{endY})");
                        continue;
                    }

                    // Additional check: Skip if coordinates are too close (prevents tiny lines/labels)
                    var distance = Math.Sqrt(Math.Pow(endX - startX, 2) + Math.Pow(endY - startY, 2));
                    if (distance < 10) // Minimum distance of 10 pixels
                    {
                        Console.WriteLine($"  SKIPPING too close: distance={distance:F2} between ({startX},{startY}) -> ({endX},{endY})");
                        continue;
                    }

                    // Additional safety check for invalid coordinates
                    if (double.IsNaN(startX) || double.IsNaN(startY) || double.IsNaN(endX) || double.IsNaN(endY))
                    {
                        Console.WriteLine($"  SKIPPING invalid coordinates: ({startX},{startY}) -> ({endX},{endY})");
                        continue;
                    }

                    // Create actual Line control
                    var line = new Line
                    {
                        StartPoint = new Point(startX, startY),
                        EndPoint = new Point(endX, endY),
                        Stroke = GetPenBrush(node, endNode, connection),
                        StrokeThickness = GetPenThickness(node, endNode, connection),
                        ZIndex = -1 // Behind nodes
                    };

                    // Create weight label with white background for better visibility
                    var weightLabel = new TextBlock
                    {
                        Text = connection.Weight.ToString(),
                        Foreground = Brushes.Red,
                        FontSize = 12,
                        FontWeight = FontWeight.Bold,
                        Background = Brushes.White, // White background for better visibility
                        ZIndex = 10 // Above everything
                    };

                    // Position weight label at midpoint
                    var midX = (startX + endX) / 2;
                    var midY = (startY + endY) / 2;
                    Canvas.SetLeft(weightLabel, midX - 10);
                    Canvas.SetTop(weightLabel, midY - 8);

                    Console.WriteLine($"  Creating weight label '{connection.Weight}' at ({midX - 10:F1}, {midY - 8:F1})");

                    // Add to canvas
                    _canvas.Children.Add(line);
                    _canvas.Children.Add(weightLabel);

                    // Track for cleanup
                    _connectionLines.Add(line);
                    _weightLabels.Add(weightLabel);

                    Console.WriteLine($"  Created line from ({startX},{startY}) to ({endX},{endY}) weight {connection.Weight}");
                }
            }

            // Multiple-stage invalidation to ensure proper cleanup
            ForceCanvasRefresh();
            Console.WriteLine($"DEBUG: RedrawAllConnections completed - canvas now has {_canvas.Children.Count} children");
        }

        private void ClearAllConnections()
        {
            Console.WriteLine($"DEBUG: ClearAllConnections clearing {_connectionLines.Count} lines and {_weightLabels.Count} labels");

            foreach (var line in _connectionLines)
            {
                _canvas.Children.Remove(line);
            }
            foreach (var label in _weightLabels)
            {
                Console.WriteLine($"  Removing weight label: '{label.Text}' at ({Canvas.GetLeft(label)}, {Canvas.GetTop(label)})");
                _canvas.Children.Remove(label);
            }
            _connectionLines.Clear();
            _weightLabels.Clear();

            // Force immediate and comprehensive canvas refresh after clearing
            ForceCanvasRefresh();

            Console.WriteLine($"DEBUG: ClearAllConnections completed - canvas now has {_canvas.Children.Count} children");

            // DON'T reset connection visual states here - algorithms set these intentionally
            // Only reset when explicitly requested (e.g., when starting new algorithms)
        }

        private void ForceCanvasRefresh()
        {
            // Multi-stage canvas refresh to ensure complete invalidation in Avalonia
            _canvas.InvalidateVisual();

            // Schedule additional refresh passes to handle timing issues
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                _canvas.InvalidateVisual();

                // Final refresh pass to ensure cleanup
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _canvas.InvalidateVisual();
                    InvalidateVisual(); // Also invalidate the parent control
                }, Avalonia.Threading.DispatcherPriority.Background);

            }, Avalonia.Threading.DispatcherPriority.Render);
        }

        private IBrush GetPenBrush(Node startNode, Node endNode, Connection connection)
        {
            Console.WriteLine($"DEBUG: GetPenBrush called for {startNode.NodeName}->{endNode.NodeName}, VisualState: {connection.VisualState}, Object ID: {connection.GetHashCode()}");

            // First check connection-specific highlighting
            switch (connection.VisualState)
            {
                case Connection.ConnectionVisualState.Highlighted:
                    Console.WriteLine($"  Connection {startNode.NodeName}->{endNode.NodeName}: Using YELLOW brush");
                    return Brushes.Yellow;
                case Connection.ConnectionVisualState.RedHighlighted:
                    Console.WriteLine($"  Connection {startNode.NodeName}->{endNode.NodeName}: Using RED brush");
                    return Brushes.Red;
                case Connection.ConnectionVisualState.GreenHighlighted:
                    Console.WriteLine($"  Connection {startNode.NodeName}->{endNode.NodeName}: Using GREEN brush");
                    return Brushes.LightGreen;
                case Connection.ConnectionVisualState.OrangeHighlighted:
                    Console.WriteLine($"  Connection {startNode.NodeName}->{endNode.NodeName}: Using ORANGE brush");
                    return Brushes.Orange;
            }

            // Then check node-based highlighting (for backwards compatibility)
            if (startNode.VisualState == Node.NodeVisualState.BestPath &&
                endNode.VisualState == Node.NodeVisualState.BestPath)
            {
                return Brushes.Green;
            }
            else if (startNode.VisualState == Node.NodeVisualState.Highlighted ||
                     endNode.VisualState == Node.NodeVisualState.Highlighted)
            {
                return Brushes.Red;
            }
            else if (startNode.VisualState == Node.NodeVisualState.Processing ||
                     endNode.VisualState == Node.NodeVisualState.Processing)
            {
                return Brushes.Orange;
            }
            return Brushes.Black;
        }

        private double GetPenThickness(Node startNode, Node endNode, Connection connection)
        {
            // Check connection-specific highlighting first
            switch (connection.VisualState)
            {
                case Connection.ConnectionVisualState.Highlighted:
                case Connection.ConnectionVisualState.GreenHighlighted:
                    return 6;
                case Connection.ConnectionVisualState.RedHighlighted:
                case Connection.ConnectionVisualState.OrangeHighlighted:
                    return 5;
            }

            // Then check node-based highlighting
            if (startNode.VisualState == Node.NodeVisualState.BestPath &&
                endNode.VisualState == Node.NodeVisualState.BestPath)
            {
                return 6;
            }
            else if (startNode.VisualState == Node.NodeVisualState.Highlighted ||
                     endNode.VisualState == Node.NodeVisualState.Highlighted)
            {
                return 5;
            }
            else if (startNode.VisualState == Node.NodeVisualState.Processing ||
                     endNode.VisualState == Node.NodeVisualState.Processing)
            {
                return 5;
            }
            return 4;
        }

        public void ForceCompleteRefresh()
        {
            // Complete canvas cleanup and rebuild to eliminate artifacts
            Console.WriteLine("DEBUG: ForceCompleteRefresh - performing complete canvas rebuild");

            // Clear all existing visuals immediately
            ClearAllConnections();

            // Force complete visual tree refresh
            _canvas.InvalidateVisual();
            InvalidateVisual();

            // Rebuild connections from scratch
            if (_graph != null)
            {
                // Schedule rebuild after current rendering pass completes
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    RedrawAllConnections();
                }, Avalonia.Threading.DispatcherPriority.Background);
            }
        }
    }
}
