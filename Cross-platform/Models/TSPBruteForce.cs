using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachingAidMac.Models;

namespace TeachingAidMac.Models
{
    public class TSPBruteForce
    {
        private Graph? _currentGraph;
        private List<Node> _bestPath = new List<Node>();
        private int _bestDistance;
        private Node? _startNode;

        public TSPBruteForce()
        {
        }

        public async Task<(List<Node> path, int distance)> SolveTSP(List<Node> nodes, int animationDelay = 500)
        {
            return await Task.Run(async () =>
            {
                _bestDistance = int.MaxValue;
                _bestPath.Clear();

                if (nodes.Count == 0)
                    return (new List<Node>(), 0);

                // Reset graph visualization
                foreach (var node in nodes)
                {
                    node.DrawNormalNode();
                    foreach (var connection in node.GetConnections())
                    {
                        connection.DrawLine();
                    }
                }

                // Start from the first node
                _startNode = nodes[0];
                var unvisitedNodes = new List<Node>(nodes);
                unvisitedNodes.Remove(_startNode);

                var currentPath = new List<Node> { _startNode };
                await SearchWithVisualization(currentPath, unvisitedNodes, 0, animationDelay);

                // Add return to start for complete cycle
                if (_bestPath.Count > 0 && !_bestPath.Contains(_startNode))
                {
                    _bestPath.Add(_startNode);
                }

                return (_bestPath, _bestDistance == int.MaxValue ? 0 : _bestDistance);
            });
        }

        public string FindBestPath(Graph graph)
        {
            _bestDistance = int.MaxValue;
            _currentGraph = graph;
            _bestPath.Clear();

            // Reset graph visualization
            foreach (var node in graph.Nodes)
            {
                node.ResetPathfinding();
            }

            // Start search from the first node in the graph
            _startNode = graph.Nodes.FirstOrDefault();
            if (_startNode == null) return "No nodes in graph";

            var unvisitedNodes = new List<Node>(graph.Nodes);
            unvisitedNodes.Remove(_startNode);

            var currentPath = new List<Node> { _startNode };

            // Start the recursive search
            Search(currentPath, unvisitedNodes, 0);

            // Build result string
            var result = new StringBuilder();
            result.AppendLine($"TSP Brute Force Algorithm");
            result.AppendLine($"Best distance: {_bestDistance}");
            result.AppendLine($"Best path:");

            foreach (var node in _bestPath)
            {
                result.Append($"{node.NodeName} -> ");
            }

            if (_bestPath.Count > 0)
            {
                result.Append(_bestPath[0].NodeName); // Return to start
            }

            // Highlight the best path
            HighlightBestPath();

            return result.ToString();
        }

        private async Task SearchWithVisualization(List<Node> currentPath, List<Node> unvisitedNodes, int currentDistance, int animationDelay)
        {
            var currentNode = currentPath.LastOrDefault();
            if (currentNode == null) return;

            // Highlight current node being processed
            currentNode.DrawRedHighlightedNode();
            await Task.Delay(animationDelay / 10); // Animation delay similar to VB.NET version

            if (unvisitedNodes.Count == 0)
            {
                // All nodes visited, check if we can return to start
                var connectionToStart = currentNode.GetConnectionTo(_startNode!);

                if (connectionToStart != null)
                {
                    var totalDistance = currentDistance + connectionToStart.Weight;
                    if (totalDistance < _bestDistance)
                    {
                        _bestDistance = totalDistance;
                        _bestPath = new List<Node>(currentPath);
                        await HighlightBestPath(_bestPath, animationDelay);
                    }
                    else
                    {
                        await HighlightPathFound(currentPath, animationDelay);
                    }
                }
                return;
            }

            // Try visiting each unvisited node
            foreach (var nextNode in unvisitedNodes.ToList())
            {
                var connection = currentNode.GetConnectionTo(nextNode);

                if (connection != null)
                {
                    var newDistance = currentDistance + connection.Weight;

                    // Pruning: if current distance already exceeds best, skip this path
                    if (newDistance < _bestDistance)
                    {
                        // Highlight the connection being explored
                        connection.DrawRedHighlightedLine();
                        await Task.Delay(animationDelay / 5); // Animation delay

                        var newPath = new List<Node>(currentPath) { nextNode };
                        var newUnvisited = new List<Node>(unvisitedNodes);
                        newUnvisited.Remove(nextNode);

                        await SearchWithVisualization(newPath, newUnvisited, newDistance, animationDelay);

                        // Highlight current path state
                        await HighlightCurrentPath(currentPath, animationDelay);
                        currentNode.DrawRedHighlightedNode();
                    }
                }
            }
        }

        private async Task HighlightBestPath(List<Node> path, int animationDelay)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                path[i].DrawGreenHighlightedNode();
                var connection = path[i].GetConnectionTo(path[i + 1]);
                connection?.DrawGreenHighlightedLine();
            }

            if (path.Count > 0)
            {
                path[path.Count - 1].DrawGreenHighlightedNode();
                // Highlight return to start
                var returnConnection = path[path.Count - 1].GetConnectionTo(_startNode!);
                returnConnection?.DrawGreenHighlightedLine();
            }

            await Task.Delay(animationDelay / 2); // Similar to VB.NET threading delay
        }

        private async Task HighlightPathFound(List<Node> path, int animationDelay)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                path[i].DrawOrangeHighlightedNode();
                var connection = path[i].GetConnectionTo(path[i + 1]);
                connection?.DrawOrangeHighlightedLine();
            }

            if (path.Count > 0)
            {
                path[path.Count - 1].DrawOrangeHighlightedNode();
                // Highlight return to start
                var returnConnection = path[path.Count - 1].GetConnectionTo(_startNode!);
                returnConnection?.DrawOrangeHighlightedLine();
            }

            await Task.Delay(animationDelay / 2); // Similar to VB.NET threading delay
        }

        private async Task HighlightCurrentPath(List<Node> path, int animationDelay)
        {
            // Reset all nodes and connections first
            if (_currentGraph != null)
            {
                foreach (var node in _currentGraph.Nodes)
                {
                    node.DrawNormalNode();
                    foreach (var connection in node.GetConnections())
                    {
                        connection.DrawLine();
                    }
                }
            }

            // Highlight current path
            for (int i = 0; i < path.Count - 1; i++)
            {
                path[i].DrawRedHighlightedNode();
                var connection = path[i].GetConnectionTo(path[i + 1]);
                connection?.DrawRedHighlightedLine();
            }

            await Task.Delay(animationDelay / 10);
        }

        private void Search(List<Node> currentPath, List<Node> unvisitedNodes, int currentDistance)
        {
            if (unvisitedNodes.Count == 0)
            {
                // All nodes visited, check if we can return to start
                var lastNode = currentPath.LastOrDefault();
                var connectionToStart = lastNode?.GetConnectionTo(_startNode!);

                if (connectionToStart != null)
                {
                    var totalDistance = currentDistance + connectionToStart.Weight;
                    if (totalDistance < _bestDistance)
                    {
                        _bestDistance = totalDistance;
                        _bestPath = new List<Node>(currentPath);
                    }
                }
                return;
            }

            var currentNode = currentPath.LastOrDefault();
            if (currentNode == null) return;

            // Try visiting each unvisited node
            for (int i = 0; i < unvisitedNodes.Count; i++)
            {
                var nextNode = unvisitedNodes[i];
                var connection = currentNode.GetConnectionTo(nextNode);

                if (connection != null)
                {
                    var newDistance = currentDistance + connection.Weight;

                    // Pruning: if current distance already exceeds best, skip this path
                    if (newDistance < _bestDistance)
                    {
                        var newPath = new List<Node>(currentPath) { nextNode };
                        var newUnvisited = new List<Node>(unvisitedNodes);
                        newUnvisited.RemoveAt(i);

                        Search(newPath, newUnvisited, newDistance);
                    }
                }
            }
        }

        private void HighlightBestPath()
        {
            if (_bestPath.Count == 0) return;

            // Reset all nodes first
            if (_currentGraph != null)
            {
                foreach (var node in _currentGraph.Nodes)
                {
                    node.ResetPathfinding();
                }
            }

            // Highlight nodes in the best path with green (best path)
            foreach (var node in _bestPath)
            {
                node.DrawGreenHighlightedNode();
            }

            // Highlight the path connections with green
            for (int i = 0; i < _bestPath.Count - 1; i++)
            {
                var connection = _bestPath[i].GetConnectionTo(_bestPath[i + 1]);
                if (connection != null)
                {
                    connection.DrawGreenHighlightedLine(); // Green for best TSP path
                }
            }

            // Highlight return connection to start
            if (_bestPath.Count > 0 && _startNode != null)
            {
                var returnConnection = _bestPath[^1].GetConnectionTo(_startNode);
                if (returnConnection != null)
                {
                    returnConnection.DrawGreenHighlightedLine();
                }
            }

            // Trigger redraw to show highlighted connections
            _currentGraph?.TriggerRedraw();
        }
    }
}
