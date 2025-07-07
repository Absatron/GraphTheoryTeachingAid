using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeachingAidMac.Models
{
    public class PathfindingAlgorithms
    {
        private Graph _graph;

        public PathfindingAlgorithms(Graph graph)
        {
            _graph = graph;
        }

        // Helper method to highlight connection in both directions for bidirectional graphs
        private void HighlightConnectionBidirectional(Node nodeA, Node nodeB)
        {
            var connectionAB = nodeA.GetConnectionTo(nodeB);
            if (connectionAB != null)
            {
                Console.WriteLine($"DEBUG: Highlighting connection {nodeA.NodeName}->{nodeB.NodeName}");
                connectionAB.DrawHighlightedLine();
            }

            var connectionBA = nodeB.GetConnectionTo(nodeA);
            if (connectionBA != null)
            {
                Console.WriteLine($"DEBUG: Highlighting reverse connection {nodeB.NodeName}->{nodeA.NodeName}");
                connectionBA.DrawHighlightedLine();
            }
        }

        // Breadth-First Search implementation
        public async Task<(List<Node> path, string output)> BFS(Node source, Node target, int animationDelay = 500)
        {
            Console.WriteLine($"DEBUG: BFS algorithm started - Source: {source?.NodeName}, Target: {target?.NodeName}");
            var output = "Starting BFS (Breadth-First Search)...\n";
            var path = new List<Node>();
            var queue = new Queue<Node>();
            var visited = new HashSet<Node>();
            var parent = new Dictionary<Node, Node?>();

            // Reset all nodes and connections
            _graph.ResetAllNodes();

            // Initialize
            queue.Push(source);
            visited.Add(source);
            parent[source] = null;
            source.VisualState = Node.NodeVisualState.Processing;

            output += $"Starting from node {source.NodeName}\n";
            Console.WriteLine($"DEBUG: Queue size after initialization: {queue.Count}");
            await Task.Delay(animationDelay);

            while (!queue.IsEmpty())
            {
                var current = queue.Pop();
                if (current == null)
                {
                    Console.WriteLine("DEBUG: Current node is null, breaking");
                    break;
                }

                Console.WriteLine($"DEBUG: Processing node {current.NodeName}, connections count: {current.GetConnections().Count}");

                output += $"Processing node {current.NodeName}\n";
                current.VisualState = Node.NodeVisualState.Processing;
                await Task.Delay(animationDelay);

                if (current == target)
                {
                    output += $"Target node {target.NodeName} found!\n";
                    break;
                }

                // Explore neighbors
                foreach (var connection in current.GetConnections())
                {
                    Console.WriteLine($"DEBUG: Exploring connection {current.NodeName}->{connection.Target.NodeName}");

                    // Highlight connection during exploration in RED
                    connection.DrawRedHighlightedLine();
                    _graph.TriggerRedraw(); // Show the red highlighting immediately
                    await Task.Delay(animationDelay / 2);

                    var neighbor = connection.Target;
                    if (!visited.Contains(neighbor))
                    {
                        Console.WriteLine($"DEBUG: Adding {neighbor.NodeName} to queue");
                        visited.Add(neighbor);
                        parent[neighbor] = current;
                        queue.Push(neighbor);
                        neighbor.VisualState = Node.NodeVisualState.Orange;
                        output += $"  Added {neighbor.NodeName} to queue\n";
                        await Task.Delay(animationDelay / 2);
                    }
                    else
                    {
                        Console.WriteLine($"DEBUG: {neighbor.NodeName} already visited, skipping");
                    }
                }

                if (current != source && current != target)
                {
                    current.VisualState = Node.NodeVisualState.BestPath;
                }
            }

            Console.WriteLine($"DEBUG: BFS main loop completed, reconstructing path");

            // FIRST: Reset ALL connection highlighting AND node visual states from exploration phase
            Console.WriteLine($"DEBUG: Resetting all connection highlighting and node visual states from exploration");
            foreach (var node in _graph.GetGraphNodes())
            {
                // Reset node visual state to normal
                node.VisualState = Node.NodeVisualState.Normal;

                // Reset all connection highlighting
                foreach (var connection in node.GetConnections())
                {
                    connection.ResetHighlighting();
                }
            }
            _graph.TriggerRedraw(); // Clear all red highlighting immediately
            await Task.Delay(animationDelay / 2);

            // Reconstruct path
            if (parent.ContainsKey(target))
            {
                var current = target;
                while (current != null)
                {
                    path.Add(current);
                    // Don't set node visual state - we'll use connection visual states for edge colors
                    current = parent.GetValueOrDefault(current);
                }
                path.Reverse();

                // Highlight ONLY the path connections in YELLOW (without individual redraws)
                Console.WriteLine($"DEBUG: Highlighting final path with {path.Count - 1} connections");
                for (int i = 0; i < path.Count - 1; i++)
                {
                    var connection = path[i].GetConnectionTo(path[i + 1]);
                    if (connection != null)
                    {
                        Console.WriteLine($"DEBUG: Highlighting final path edge {path[i].NodeName}->{path[i + 1].NodeName}");
                        // Set visual state directly without triggering individual redraws
                        connection.VisualState = Connection.ConnectionVisualState.Highlighted;

                        // Also highlight reverse connection for bidirectional graphs
                        var reverseConnection = path[i + 1].GetConnectionTo(path[i]);
                        if (reverseConnection != null && reverseConnection != connection)
                        {
                            reverseConnection.VisualState = Connection.ConnectionVisualState.Highlighted;
                        }
                    }
                }

                // Trigger single redraw to show all highlighted connections at once
                _graph.TriggerRedraw();

                output += $"Path found: {string.Join(" -> ", path.Select(n => n.NodeName))}\n";
                output += $"Path length: {path.Count - 1} edges\n";
            }
            else
            {
                output += "No path found!\n";
            }

            return (path, output);
        }

        // Depth-First Search implementation
        public async Task<(List<Node> path, string output)> DFS(Node source, Node target, int animationDelay = 500)
        {
            var output = "Starting DFS (Depth-First Search)...\n";
            var path = new List<Node>();
            var stack = new Stack<Node>();
            var visited = new HashSet<Node>();
            var parent = new Dictionary<Node, Node?>();

            // Reset all nodes
            _graph.ResetAllNodes();

            // Initialize
            stack.Push(source);
            parent[source] = null;

            output += $"Starting from node {source.NodeName}\n";
            await Task.Delay(animationDelay);

            while (!stack.IsEmpty())
            {
                var current = stack.Pop();
                if (current == null) break;

                if (visited.Contains(current))
                    continue;

                visited.Add(current);
                output += $"Processing node {current.NodeName}\n";
                current.VisualState = Node.NodeVisualState.Processing;
                await Task.Delay(animationDelay);

                if (current == target)
                {
                    output += $"Target node {target.NodeName} found!\n";
                    break;
                }

                // Explore neighbors (in reverse order for DFS)
                var connections = current.GetConnections().ToList();
                connections.Reverse();

                foreach (var connection in connections)
                {
                    // Highlight connection during exploration
                    connection.DrawRedHighlightedLine();
                    _graph.TriggerRedraw(); // Show the red highlighting immediately
                    await Task.Delay(animationDelay / 2);

                    var neighbor = connection.Target;
                    if (!visited.Contains(neighbor))
                    {
                        if (!parent.ContainsKey(neighbor))
                        {
                            parent[neighbor] = current;
                        }
                        stack.Push(neighbor);
                        neighbor.VisualState = Node.NodeVisualState.Orange;
                        output += $"  Added {neighbor.NodeName} to stack\n";
                        await Task.Delay(animationDelay / 2);
                    }
                }

                if (current != source && current != target)
                {
                    current.VisualState = Node.NodeVisualState.BestPath;
                }
            }

            // FIRST: Reset ALL connection highlighting AND node visual states from exploration phase
            Console.WriteLine($"DEBUG: DFS - Resetting all connection highlighting and node visual states from exploration");
            foreach (var node in _graph.GetGraphNodes())
            {
                // Reset node visual state to normal
                node.VisualState = Node.NodeVisualState.Normal;

                // Reset all connection highlighting
                foreach (var connection in node.GetConnections())
                {
                    connection.ResetHighlighting();
                }
            }
            _graph.TriggerRedraw(); // Clear all red highlighting immediately
            await Task.Delay(animationDelay / 2);

            // Reconstruct path
            if (parent.ContainsKey(target))
            {
                var current = target;
                while (current != null)
                {
                    path.Add(current);
                    // Don't set node visual state - we'll use connection visual states for edge colors
                    current = parent.GetValueOrDefault(current);
                }
                path.Reverse();

                // Highlight ONLY the path connections in YELLOW (without individual redraws)
                Console.WriteLine($"DEBUG: DFS - Highlighting final path with {path.Count - 1} connections");
                for (int i = 0; i < path.Count - 1; i++)
                {
                    var connection = path[i].GetConnectionTo(path[i + 1]);
                    if (connection != null)
                    {
                        Console.WriteLine($"DEBUG: DFS - Highlighting final path edge {path[i].NodeName}->{path[i + 1].NodeName}");
                        // Set visual state directly without triggering individual redraws
                        connection.VisualState = Connection.ConnectionVisualState.Highlighted;

                        // Also highlight reverse connection for bidirectional graphs
                        var reverseConnection = path[i + 1].GetConnectionTo(path[i]);
                        if (reverseConnection != null && reverseConnection != connection)
                        {
                            reverseConnection.VisualState = Connection.ConnectionVisualState.Highlighted;
                        }
                    }
                }

                // Trigger single redraw to show all highlighted connections at once
                _graph.TriggerRedraw();

                output += $"Path found: {string.Join(" -> ", path.Select(n => n.NodeName))}\n";
                output += $"Path length: {path.Count - 1} edges\n";
            }
            else
            {
                output += "No path found!\n";
            }

            return (path, output);
        }

        // Dijkstra's Algorithm implementation
        public async Task<(List<Node> path, string output)> Dijkstra(Node source, Node target, int animationDelay = 500)
        {
            var output = "Starting Dijkstra's Algorithm...\n";
            var path = new List<Node>();
            var distances = new Dictionary<Node, int>();
            var parent = new Dictionary<Node, Node?>();
            var unvisited = new HashSet<Node>(_graph.GetGraphNodes());

            // Reset all nodes
            _graph.ResetAllNodes();

            // Initialize distances
            foreach (var node in _graph.GetGraphNodes())
            {
                distances[node] = int.MaxValue;
            }
            distances[source] = 0;
            parent[source] = null;

            output += $"Starting from node {source.NodeName}\n";
            output += "Distance from source: 0\n";
            await Task.Delay(animationDelay);

            while (unvisited.Count > 0)
            {
                // Find unvisited node with minimum distance
                var current = unvisited
                    .Where(n => distances[n] != int.MaxValue)
                    .OrderBy(n => distances[n])
                    .FirstOrDefault();

                if (current == null) break;

                unvisited.Remove(current);
                current.VisualState = Node.NodeVisualState.Processing;
                output += $"Processing node {current.NodeName} (distance: {distances[current]})\n";
                await Task.Delay(animationDelay);

                if (current == target)
                {
                    output += $"Target node {target.NodeName} reached!\n";
                    break;
                }

                // Update distances to neighbors
                foreach (var connection in current.GetConnections())
                {
                    // Highlight connection during exploration
                    connection.DrawRedHighlightedLine();
                    _graph.TriggerRedraw(); // Show the red highlighting immediately
                    await Task.Delay(animationDelay / 2);

                    var neighbor = connection.Target;
                    if (unvisited.Contains(neighbor))
                    {
                        var newDistance = distances[current] + connection.Weight;
                        if (newDistance < distances[neighbor])
                        {
                            distances[neighbor] = newDistance;
                            parent[neighbor] = current;
                            neighbor.VisualState = Node.NodeVisualState.Orange;
                            output += $"  Updated distance to {neighbor.NodeName}: {newDistance}\n";
                            await Task.Delay(animationDelay / 2);
                        }
                    }
                }

                if (current != source && current != target)
                {
                    current.VisualState = Node.NodeVisualState.BestPath;
                }
            }

            // FIRST: Reset ALL connection highlighting AND node visual states from exploration phase
            Console.WriteLine($"DEBUG: Dijkstra - Resetting all connection highlighting and node visual states from exploration");
            foreach (var node in _graph.GetGraphNodes())
            {
                // Reset node visual state to normal
                node.VisualState = Node.NodeVisualState.Normal;

                // Reset all connection highlighting
                foreach (var connection in node.GetConnections())
                {
                    connection.ResetHighlighting();
                }
            }
            _graph.TriggerRedraw(); // Clear all red highlighting immediately
            await Task.Delay(animationDelay / 2);

            // Reconstruct shortest path
            if (parent.ContainsKey(target) && distances[target] != int.MaxValue)
            {
                var current = target;
                var totalDistance = 0;
                while (current != null)
                {
                    path.Add(current);
                    // Don't set node visual state - we'll use connection visual states for edge colors
                    current = parent.GetValueOrDefault(current);
                }
                path.Reverse();

                // Calculate total distance and highlight ONLY the path connections in YELLOW (without individual redraws)
                Console.WriteLine($"DEBUG: Dijkstra - Highlighting final path with {path.Count - 1} connections");
                for (int i = 0; i < path.Count - 1; i++)
                {
                    var connection = path[i].GetConnectionTo(path[i + 1]);
                    if (connection != null)
                    {
                        totalDistance += connection.Weight;
                        Console.WriteLine($"DEBUG: Dijkstra - Highlighting final path edge {path[i].NodeName}->{path[i + 1].NodeName}");
                        // Set visual state directly without triggering individual redraws
                        connection.VisualState = Connection.ConnectionVisualState.Highlighted;

                        // Also highlight reverse connection for bidirectional graphs
                        var reverseConnection = path[i + 1].GetConnectionTo(path[i]);
                        if (reverseConnection != null && reverseConnection != connection)
                        {
                            reverseConnection.VisualState = Connection.ConnectionVisualState.Highlighted;
                        }
                    }
                }

                // Trigger single redraw to show all highlighted connections at once
                _graph.TriggerRedraw();

                output += $"Shortest path found: {string.Join(" -> ", path.Select(n => n.NodeName))}\n";
                output += $"Total distance: {totalDistance}\n";
                output += $"Number of edges: {path.Count - 1}\n";
            }
            else
            {
                output += "No path found!\n";
            }

            return (path, output);
        }
    }
}
