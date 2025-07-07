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

        public async Task<(List<Node> path, int distance)> SolveTSP(List<Node> nodes)
        {
            return await Task.Run(() =>
            {
                _bestDistance = int.MaxValue;
                _bestPath.Clear();

                if (nodes.Count == 0)
                    return (new List<Node>(), 0);

                // Start from the first node
                _startNode = nodes[0];
                var unvisitedNodes = new List<Node>(nodes);
                unvisitedNodes.Remove(_startNode);

                var currentPath = new List<Node> { _startNode };
                Search(currentPath, unvisitedNodes, 0);

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

            // Highlight nodes in the best path
            foreach (var node in _bestPath)
            {
                node.SetAsInPath();
            }

            // Highlight the path connections
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

            // Mark start node as source
            if (_startNode != null)
            {
                _startNode.SetAsSource();
            }

            // Trigger redraw to show highlighted connections
            _currentGraph?.TriggerRedraw();
        }
    }
}
