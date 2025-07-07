using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using TeachingAidMac.Controls;

namespace TeachingAidMac.Models
{
    public class Node : Border
    {
        private bool _isDragging = false;
        private Point _startPoint;
        private string _name = "";
        private List<Connection> _connections = new List<Connection>();
        private Node? _previousNode;
        private int _shortestFromSource = 1000000;
        private bool _processed = false;
        private bool _discovered = false;
        private bool _isGraphNode;
        private Graph? _currentGraph;
        private NodeVisualState _visualState = NodeVisualState.Normal;
        private Point _position = new Point(0, 0); // Backup position storage

        public enum NodeVisualState
        {
            Normal,
            Source,
            Target,
            Highlighted,
            Processing,
            BestPath,
            Orange
        }

        public string NodeName => _name;
        public List<Connection> Connections => _connections;
        public Node? PreviousNode
        {
            get => _previousNode;
            set => _previousNode = value;
        }
        public int ShortestFromSource
        {
            get => _shortestFromSource;
            set => _shortestFromSource = value;
        }
        public bool Processed
        {
            get => _processed;
            set => _processed = value;
        }
        public bool Discovered
        {
            get => _discovered;
            set => _discovered = value;
        }
        public Graph? CurrentGraph
        {
            get => _currentGraph;
            set => _currentGraph = value;
        }
        public NodeVisualState VisualState
        {
            get => _visualState;
            set
            {
                _visualState = value;
                UpdateVisualAppearance();
            }
        }

        public Node(bool isGraphNode)
        {
            _isGraphNode = isGraphNode;
            InitializeNode();
        }

        private void InitializeNode()
        {
            Width = 60;
            Height = 60;
            Background = Brushes.AliceBlue;
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(30); // Make it perfectly circular

            // Ensure the node is visible and interactive
            IsVisible = true;
            IsHitTestVisible = true;
            Focusable = true;

            // Set up event handlers for dragging
            PointerPressed += OnPointerPressed;
            PointerMoved += OnPointerMoved;
            PointerReleased += OnPointerReleased;

            // Set up context menu
            SetupContextMenu();
        }

        private void SetupContextMenu()
        {
            var contextMenu = new ContextMenu();
            // Context menu items will be added dynamically based on available nodes
            ContextMenu = contextMenu;
        }

        public void UpdateContextMenu(List<Node> allNodes)
        {
            if (ContextMenu == null) return;

            ContextMenu.Items.Clear();

            foreach (var node in allNodes)
            {
                if (node == this) continue;

                var menuItem = new MenuItem();
                menuItem.Header = $"Node: {node.NodeName}";

                var connection = GetConnectionTo(node);
                menuItem.IsChecked = connection != null;

                menuItem.Click += (sender, e) =>
                {
                    if (menuItem.IsChecked)
                    {
                        RemoveConnection(node);
                    }
                    else
                    {
                        AddConnection(node.NodeName);
                    }
                };

                ContextMenu.Items.Add(menuItem);
            }
        }

        private void UpdateVisualAppearance()
        {
            var brush = _visualState switch
            {
                NodeVisualState.Normal => Brushes.AliceBlue,
                NodeVisualState.Source => Brushes.LightGreen,
                NodeVisualState.Target => Brushes.LightCoral,
                NodeVisualState.Highlighted => Brushes.Yellow,
                NodeVisualState.Processing => Brushes.Red,
                NodeVisualState.BestPath => Brushes.LightGreen,
                NodeVisualState.Orange => Brushes.Orange,
                _ => Brushes.AliceBlue
            };

            Background = brush;
        }

        public void SetName(string name)
        {
            _name = name;
            Child = new TextBlock
            {
                Text = name,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                FontWeight = FontWeight.Bold,
                FontSize = 24,
                Foreground = Brushes.Black
            };
        }

        public string GetName()
        {
            return _name;
        }

        public void SetConnection(Node targetNode, int weight)
        {
            // Prevent self-connections (node connecting to itself)
            if (targetNode == this) return;

            // Check if connection already exists
            var existingConnection = GetConnectionTo(targetNode);
            if (existingConnection != null)
            {
                existingConnection.Weight = weight;
                return;
            }

            var connection = new Connection(this, targetNode, weight);
            _connections.Add(connection);
        }

        public Connection? GetConnectionTo(Node targetNode)
        {
            return _connections.Find(c => c.Target == targetNode);
        }

        public List<Connection> GetConnections()
        {
            return _connections;
        }

        public void RemoveConnection(Node targetNode)
        {
            var connection = GetConnectionTo(targetNode);
            if (connection != null)
            {
                _connections.Remove(connection);
                connection.RemoveLine();
            }
        }

        public void RemoveConnection(string targetNodeName)
        {
            var connection = _connections.Find(c => c.Target.NodeName == targetNodeName);
            if (connection != null)
            {
                _connections.Remove(connection);
                connection.RemoveLine();
            }
        }

        public void AddConnection(string nodeName)
        {
            if (_currentGraph == null) return;

            var targetNode = _currentGraph.FindNodeByName(nodeName);
            if (targetNode == null) return;

            // Prevent self-connections (node connecting to itself)
            if (targetNode == this) return;

            // For now, use a default weight of 1. In a full implementation,
            // this would show an input dialog for manual mode
            int weight = 1;

            SetConnection(targetNode, weight);
            var newConnection = GetConnectionTo(targetNode);
            if (newConnection != null)
            {
                // GraphCanvas will handle rendering
            }

            // Add reciprocal connection for undirected graph
            targetNode.SetConnection(this, weight);

            // Trigger redraw to show new connections
            _currentGraph?.TriggerRedraw();
        }

        // Visual state methods
        public void DrawNormalNode()
        {
            VisualState = NodeVisualState.Normal;
        }

        public void DrawRedHighlightedNode()
        {
            VisualState = NodeVisualState.Processing;
        }

        public void DrawGreenHighlightedNode()
        {
            VisualState = NodeVisualState.BestPath;
        }

        public void DrawOrangeHighlightedNode()
        {
            VisualState = NodeVisualState.Orange;
        }

        public void DrawYellowHighlightedNode()
        {
            VisualState = NodeVisualState.Highlighted;
        }

        // Algorithm state methods
        public void ResetAlgorithmValues()
        {
            _shortestFromSource = 1000000;
            _processed = false;
            _discovered = false;
            _previousNode = null;
            VisualState = NodeVisualState.Normal;
        }

        public void ResetPathfinding()
        {
            _processed = false;
            _discovered = false;
            _shortestFromSource = 1000000;
            _previousNode = null;
            _visualState = NodeVisualState.Normal;
            Background = Brushes.AliceBlue;
        }

        // Pointer event handlers for dragging
        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (_isGraphNode && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                _isDragging = true;
                _startPoint = e.GetPosition(this);
                e.Pointer.Capture(this);
                e.Handled = true;
            }
            else if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                // Handle right-click for context menu
                ShowContextMenu(e.GetCurrentPoint(this));
                e.Handled = true;
            }
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isDragging && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                var currentPoint = e.GetPosition(Parent as Visual);
                var currentPos = GetPosition();

                // Calculate new position centered on cursor
                var newX = currentPoint.X - (Width / 2);
                var newY = currentPoint.Y - (Height / 2);

                // Clamp to canvas bounds
                if (Parent is Canvas canvas)
                {
                    newX = Math.Max(0, Math.Min(newX, canvas.Bounds.Width - Width));
                    newY = Math.Max(0, Math.Min(newY, canvas.Bounds.Height - Height));
                }

                SetPosition(new Point(newX, newY));

                // Update connections if pixel mode is enabled
                if (CurrentGraph?.PixelMode == true)
                {
                    foreach (var connection in _connections)
                    {
                        connection.UpdateEdgeNumber();
                    }
                }

                // Force canvas redraw using Graph's TriggerRedraw method
                CurrentGraph?.TriggerRedraw();

                e.Handled = true;
            }
        }

        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                e.Pointer.Capture(null);
            }
        }

        private void ShowContextMenu(PointerPoint point)
        {
            // Create context menu for adding/removing connections
            var contextMenu = new ContextMenu();

            if (CurrentGraph != null)
            {
                var otherNodes = CurrentGraph.GetGraphNodes().Where(n => n != this).ToList();

                foreach (var node in otherNodes)
                {
                    var existingConnection = GetConnectionTo(node);

                    if (existingConnection == null)
                    {
                        // Add "Connect to X" menu item
                        var connectItem = new MenuItem
                        {
                            Header = $"Connect to {node.NodeName}"
                        };
                        connectItem.Click += (s, e) => AddConnectionToNode(node);
                        contextMenu.Items.Add(connectItem);
                    }
                    else
                    {
                        // Add "Remove connection to X" menu item
                        var disconnectItem = new MenuItem
                        {
                            Header = $"Remove connection to {node.NodeName}"
                        };
                        disconnectItem.Click += (s, e) => RemoveConnectionToNode(node);
                        contextMenu.Items.Add(disconnectItem);
                    }
                }
            }

            if (contextMenu.Items.Count > 0)
            {
                contextMenu.Open(this);
            }
        }

        private void AddConnectionToNode(Node targetNode)
        {
            Console.WriteLine($"DEBUG: AddConnectionToNode called: {NodeName} -> {targetNode.NodeName}");

            if (CurrentGraph != null)
            {
                // Prevent self-connections
                if (targetNode == this)
                {
                    Console.WriteLine($"  PREVENTED self-connection in AddConnectionToNode: {NodeName} -> {targetNode.NodeName}");
                    return;
                }

                // Use default weight of 1 in manual mode, or calculate pixel distance in pixel mode
                var weight = CurrentGraph.PixelMode ? CalculatePixelDistance(targetNode) : 1;

                Console.WriteLine($"  Setting connection with weight: {weight}");
                SetConnection(targetNode, weight);
                CurrentGraph.UpdateAddedConnection(this, targetNode);

                // Immediate canvas refresh for context menu operations
                ForceImmediateCanvasRefresh();

                Console.WriteLine($"  AddConnectionToNode completed with immediate refresh");
            }
        }

        private void RemoveConnectionToNode(Node targetNode)
        {
            Console.WriteLine($"DEBUG: RemoveConnectionToNode called: {NodeName} -> {targetNode.NodeName}");

            if (CurrentGraph != null)
            {
                var connection = GetConnectionTo(targetNode);
                if (connection != null)
                {
                    connection.RemoveLine();
                    RemoveConnection(targetNode);
                    CurrentGraph.UpdateRemovedConnection(this, targetNode);

                    // Immediate canvas refresh for context menu operations
                    ForceImmediateCanvasRefresh();

                    Console.WriteLine($"  RemoveConnectionToNode completed with immediate refresh");
                }
            }
        }

        private void ForceImmediateCanvasRefresh()
        {
            // Immediate multi-stage refresh to prevent context menu artifacts
            CurrentGraph?.Canvas?.InvalidateVisual();
            CurrentGraph?.TriggerRedraw();

            // Use the complete refresh method for context menu operations
            // Find the GraphCanvas parent control
            var parent = Parent;
            while (parent != null && !(parent is TeachingAidMac.Controls.GraphCanvas))
            {
                parent = parent.Parent;
            }

            if (parent is TeachingAidMac.Controls.GraphCanvas graphCanvas)
            {
                graphCanvas.ForceCompleteRefresh();
            }

            // Additional immediate refresh passes as backup
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                CurrentGraph?.Canvas?.InvalidateVisual();

                // Final cleanup pass
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    CurrentGraph?.Canvas?.InvalidateVisual();
                    if (Parent is Canvas parentCanvas)
                    {
                        parentCanvas.InvalidateVisual();
                    }
                }, Avalonia.Threading.DispatcherPriority.Background);

            }, Avalonia.Threading.DispatcherPriority.Render);
        }

        private int CalculatePixelDistance(Node targetNode)
        {
            var dx = this.Bounds.Center.X - targetNode.Bounds.Center.X;
            var dy = this.Bounds.Center.Y - targetNode.Bounds.Center.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);
            return (int)Math.Round(distance / 20.0); // Scale factor similar to VB version
        }

        // Additional methods for VB compatibility
        public void SetAsSource()
        {
            _visualState = NodeVisualState.Source;
            Background = Brushes.LightBlue;
        }

        public void SetAsTarget()
        {
            _visualState = NodeVisualState.Target;
            Background = Brushes.LightPink;
        }

        public void SetAsInPath()
        {
            _visualState = NodeVisualState.BestPath;
            Background = Brushes.LightGreen;
        }

        public void SetAsVisited()
        {
            _visualState = NodeVisualState.Processing;
            Background = Brushes.Yellow;
        }

        public void SetAsDiscovered()
        {
            _discovered = true;
            _visualState = NodeVisualState.Orange;
            Background = Brushes.Orange;
        }

        // Set position on canvas
        public void SetPosition(Point position)
        {
            _position = position; // Always store backup position

            // Set Canvas position if available
            if (Parent is Canvas || CurrentGraph?.Canvas != null)
            {
                Canvas.SetLeft(this, position.X);
                Canvas.SetTop(this, position.Y);
            }
        }

        // Get current position
        public Point GetPosition()
        {
            // Try Canvas positioning first (most accurate for rendering)
            if (Parent is Canvas && CurrentGraph?.Canvas != null)
            {
                var x = Canvas.GetLeft(this);
                var y = Canvas.GetTop(this);
                if (!double.IsNaN(x) && !double.IsNaN(y))
                {
                    return new Point(x, y);
                }
            }

            // Fallback to backup position
            return _position;
        }

        // Additional methods for VB compatibility
        public void RemoveRemovedNodeConnections(Node removedNode)
        {
            var connection = GetConnectionTo(removedNode);
            if (connection != null)
            {
                connection.RemoveLine();
                _connections.Remove(connection);
            }
        }

        // Update visual state based on current state
        public void UpdateVisualState()
        {
            switch (_visualState)
            {
                case NodeVisualState.Normal:
                    Background = Brushes.AliceBlue;
                    break;
                case NodeVisualState.Source:
                    Background = Brushes.LightBlue;
                    break;
                case NodeVisualState.Target:
                    Background = Brushes.LightPink;
                    break;
                case NodeVisualState.Highlighted:
                    Background = Brushes.LightCoral;
                    break;
                case NodeVisualState.Processing:
                    Background = Brushes.Yellow;
                    break;
                case NodeVisualState.BestPath:
                    Background = Brushes.LightGreen;
                    break;
                case NodeVisualState.Orange:
                    Background = Brushes.Orange;
                    break;
            }
        }
    }
}
