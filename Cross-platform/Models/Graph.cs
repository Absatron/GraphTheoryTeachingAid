using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using TeachingAidMac.Controls;

namespace TeachingAidMac.Models
{
    public class Graph
    {
        private List<Node> _nodes = new List<Node>();
        private Canvas? _canvas;
        private bool _pixelMode = false;
        private Action? _onConnectionsChanged;

        public List<Node> Nodes => _nodes;
        public Canvas? Canvas
        {
            get => _canvas;
            set => _canvas = value;
        }
        public bool PixelMode
        {
            get => _pixelMode;
            set
            {
                _pixelMode = value;
                // Update all connections when mode changes
                foreach (var node in _nodes)
                {
                    foreach (var connection in node.Connections)
                    {
                        connection.SetPixelMode(value);
                    }
                }
            }
        }

        public Action? OnConnectionsChanged
        {
            get => _onConnectionsChanged;
            set => _onConnectionsChanged = value;
        }

        public Graph()
        {
            // Empty constructor - we'll call SetInitialGraph later
        }

        public void SetInitialGraph()
        {
            _nodes.Clear();

            // Create nodes exactly like the VB.NET version
            var nodeA = new Node(true);
            var nodeB = new Node(true);
            var nodeC = new Node(true);
            var nodeD = new Node(true);
            var nodeE = new Node(true);
            var nodeF = new Node(true);
            var nodeG = new Node(true);

            // Set names and connections exactly like VB.NET version
            nodeA.SetName("A");
            nodeA.SetConnection(nodeB, 3);
            nodeA.SetConnection(nodeD, 6);
            nodeA.SetConnection(nodeC, 5);

            nodeB.SetName("B");
            nodeB.SetConnection(nodeA, 3);
            nodeB.SetConnection(nodeD, 2);

            nodeC.SetName("C");
            nodeC.SetConnection(nodeA, 5);
            nodeC.SetConnection(nodeD, 2);
            nodeC.SetConnection(nodeF, 3);
            nodeC.SetConnection(nodeG, 7);
            nodeC.SetConnection(nodeE, 6);

            nodeD.SetName("D");
            nodeD.SetConnection(nodeB, 2);
            nodeD.SetConnection(nodeA, 6);
            nodeD.SetConnection(nodeC, 2);
            nodeD.SetConnection(nodeF, 9);

            nodeE.SetName("E");
            nodeE.SetConnection(nodeC, 6);
            nodeE.SetConnection(nodeF, 5);
            nodeE.SetConnection(nodeG, 2);

            nodeF.SetName("F");
            nodeF.SetConnection(nodeD, 9);
            nodeF.SetConnection(nodeC, 3);
            nodeF.SetConnection(nodeE, 5);
            nodeF.SetConnection(nodeG, 1);

            nodeG.SetName("G");
            nodeG.SetConnection(nodeF, 1);
            nodeG.SetConnection(nodeE, 2);
            nodeG.SetConnection(nodeC, 7);

            // Add nodes to graph
            AddNode(nodeA);
            AddNode(nodeB);
            AddNode(nodeC);
            AddNode(nodeD);
            AddNode(nodeE);
            AddNode(nodeF);
            AddNode(nodeG);

            // Set positions exactly like the VB.NET version
            SetNodePosition(nodeA, 100, 100);
            SetNodePosition(nodeB, 100, 300);
            SetNodePosition(nodeC, 300, 100);
            SetNodePosition(nodeD, 200, 300);
            SetNodePosition(nodeE, 600, 100);
            SetNodePosition(nodeF, 450, 300);
            SetNodePosition(nodeG, 600, 300);

            // Clean up any invalid connections (like self-connections)
            CleanupInvalidConnections();

            // Update context menus for all nodes
            UpdateAllContextMenus();
        }

        public void AddNode(Node node)
        {
            _nodes.Add(node);
            node.CurrentGraph = this;

            // Add to canvas if available
            if (_canvas != null)
            {
                _canvas.Children.Add(node);
            }

            // Update context menus for all nodes
            UpdateAllContextMenus();
        }

        public void AddNode(string nodeName)
        {
            if (_nodes.Count >= 26)
            {
                // Graph is full (A-Z)
                return;
            }

            if (string.IsNullOrEmpty(nodeName))
            {
                return;
            }

            // Check if it's a valid capital letter
            if (nodeName.Length != 1 || nodeName[0] < 'A' || nodeName[0] > 'Z')
            {
                return;
            }

            // Check if node name is already in use
            if (_nodes.Any(n => n.NodeName == nodeName))
            {
                return;
            }

            var newNode = new Node(true);
            newNode.SetName(nodeName);

            // Set default position
            if (_canvas != null)
            {
                Canvas.SetLeft(newNode, 20);
                Canvas.SetTop(newNode, 20);
            }

            AddNode(newNode);
        }

        public void RemoveNode(string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName))
            {
                return;
            }

            var nodeToRemove = _nodes.FirstOrDefault(n => n.NodeName == nodeName);
            if (nodeToRemove == null)
            {
                return;
            }

            // Remove all connections to this node from other nodes
            foreach (var node in _nodes)
            {
                node.RemoveRemovedNodeConnections(nodeToRemove);
            }

            // Remove connections from this node
            foreach (var connection in nodeToRemove.GetConnections().ToList())
            {
                connection.RemoveLine();
            }

            // Remove from nodes list
            _nodes.Remove(nodeToRemove);

            // Remove from canvas
            if (_canvas != null && _canvas.Children.Contains(nodeToRemove))
            {
                _canvas.Children.Remove(nodeToRemove);
            }

            // Redraw all remaining connections
            TriggerRedraw();

            // Update context menus
            UpdateAllContextMenus();
        }

        // Algorithm support methods
        public void ResetAllNodes()
        {
            foreach (var node in _nodes)
            {
                node.ResetPathfinding();
            }

            // Reset all connection highlighting
            foreach (var node in _nodes)
            {
                foreach (var connection in node.Connections)
                {
                    connection.ResetHighlighting();
                }
            }
        }

        // Reset only node visual states, preserve connection highlighting
        public void ResetNodeVisualStates()
        {
            foreach (var node in _nodes)
            {
                node.ResetPathfinding();
            }
            // Don't reset connection highlighting - preserve edge highlighting after algorithms
        }

        public void UpdateConnections()
        {
            // Update pixel mode for all connections
            foreach (var node in _nodes)
            {
                foreach (var connection in node.GetConnections())
                {
                    connection.SetPixelMode(_pixelMode);
                    if (_pixelMode)
                    {
                        connection.UpdateEdgeNumber();
                    }
                    // GraphCanvas will handle rendering
                }
            }
        }

        // Missing methods for VB compatibility
        public void ClearGraph()
        {
            // Remove all connections first
            foreach (var node in _nodes)
            {
                foreach (var connection in node.GetConnections().ToList())
                {
                    connection.RemoveLine();
                }
                // Clear the connections list for each node
                node.GetConnections().Clear();
            }

            // Remove all nodes from canvas
            if (_canvas != null)
            {
                foreach (var node in _nodes)
                {
                    _canvas.Children.Remove(node);
                }
            }

            _nodes.Clear();

            // Trigger redraw to ensure clean canvas
            TriggerRedraw();
        }

        public void UpdateAddedConnection(Node sourceNode, Node targetNode)
        {
            Console.WriteLine($"DEBUG: UpdateAddedConnection called: {sourceNode.NodeName} -> {targetNode.NodeName}");

            // Prevent self-connections
            if (sourceNode == targetNode)
            {
                Console.WriteLine($"  PREVENTED self-connection: {sourceNode.NodeName} -> {targetNode.NodeName}");
                return;
            }

            // For bidirectional connections
            var sourceConnection = sourceNode.GetConnectionTo(targetNode);
            var targetConnection = targetNode.GetConnectionTo(sourceNode);

            if (sourceConnection != null && targetConnection == null)
            {
                Console.WriteLine($"  Adding reciprocal connection: {targetNode.NodeName} -> {sourceNode.NodeName}");
                targetNode.SetConnection(sourceNode, sourceConnection.Weight);
            }

            TriggerRedraw();
        }

        public void UpdateRemovedConnection(Node sourceNode, Node targetNode)
        {
            Console.WriteLine($"DEBUG: UpdateRemovedConnection called: {sourceNode.NodeName} -> {targetNode.NodeName}");

            // Remove bidirectional connections
            sourceNode.RemoveConnection(targetNode);
            targetNode.RemoveConnection(sourceNode);

            TriggerRedraw();
        }

        public void UpdateAllContextMenus()
        {
            // This would be implemented to update context menus for node connections
            // For now, it's a placeholder for VB compatibility
        }

        public void TriggerRedraw()
        {
            Console.WriteLine($"DEBUG: TriggerRedraw called - _onConnectionsChanged is {(_onConnectionsChanged != null ? "not null" : "null")}");

            // Clean up any invalid connections before redrawing
            CleanupInvalidConnections();

            // Force comprehensive canvas refresh before and after redraw
            if (_canvas != null)
            {
                _canvas.InvalidateVisual();
            }

            _onConnectionsChanged?.Invoke();

            // Additional refresh pass after redraw to ensure cleanup
            if (_canvas != null)
            {
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _canvas.InvalidateVisual();
                }, Avalonia.Threading.DispatcherPriority.Render);
            }
        }

        public void SetNodePosition(Node node, double x, double y)
        {
            node.SetPosition(new Point(x, y));
            TriggerRedraw(); // Redraw connections when nodes move
        }

        public Node? GetNodeFromName(string name)
        {
            return FindNodeByName(name);
        }

        public Node? FindNodeByName(string name)
        {
            return _nodes.FirstOrDefault(n => n.NodeName == name);
        }

        public List<Node> GetGraphNodes()
        {
            return _nodes;
        }

        public List<Connection> GetAllConnections()
        {
            var connections = new List<Connection>();
            foreach (var node in _nodes)
            {
                connections.AddRange(node.GetConnections());
            }
            return connections.Distinct().ToList();
        }

        public void FullyConnectNodes()
        {
            foreach (var node in _nodes)
            {
                foreach (var otherNode in _nodes)
                {
                    if (node.NodeName == otherNode.NodeName)
                        continue;

                    // Check if connection doesn't already exist
                    if (node.GetConnectionTo(otherNode) == null)
                    {
                        // In pixel mode, weight will be calculated automatically
                        // In manual mode, use a default weight of 1
                        var weight = 1;
                        node.SetConnection(otherNode, weight);

                        var connection = node.GetConnectionTo(otherNode);
                        if (connection != null)
                        {
                            connection.SetPixelMode(_pixelMode);
                            // GraphCanvas will handle rendering
                        }
                    }
                }
            }
        }

        // Adjacency matrix and list generation
        public string[,] GetAdjacencyMatrix()
        {
            var size = _nodes.Count;
            var matrix = new string[size + 1, size + 1];

            // Set header row and column
            matrix[0, 0] = "";
            for (int i = 0; i < size; i++)
            {
                matrix[0, i + 1] = _nodes[i].NodeName;
                matrix[i + 1, 0] = _nodes[i].NodeName;
            }

            // Fill matrix
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var connection = _nodes[i].GetConnectionTo(_nodes[j]);
                    matrix[i + 1, j + 1] = connection?.Weight.ToString() ?? "0";
                }
            }

            return matrix;
        }

        public List<string> GetAdjacencyList()
        {
            var list = new List<string>();

            foreach (var node in _nodes)
            {
                var connections = string.Join(", ",
                    node.GetConnections().Select(c => $"{c.Target.NodeName}({c.Weight})"));
                list.Add($"{node.NodeName}: {connections}");
            }

            return list;
        }

        public void CleanupInvalidConnections()
        {
            foreach (var node in _nodes)
            {
                // Remove any self-connections
                var invalidConnections = node.GetConnections()
                    .Where(c => c.Target == node || c.Source == node && c.Target == node)
                    .ToList();

                foreach (var invalidConnection in invalidConnections)
                {
                    node.RemoveConnection(invalidConnection.Target);
                }
            }
        }
    }
}
