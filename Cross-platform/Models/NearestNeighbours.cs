using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachingAidMac.Models;

namespace TeachingAidMac.Models
{
    public class NearestNeighbours
    {
        private Graph? _currentGraph;
        private List<Node> _bestPath = new List<Node>();
        private int _bestDistance;

        public NearestNeighbours()
        {
        }

        public async Task<(List<Node> path, int distance)> SolveTSP(List<Node> nodes, Node startNode)
        {
            return await Task.Run(() =>
            {
                _bestPath.Clear();
                _bestDistance = 0;

                if (nodes.Count == 0)
                    return (new List<Node>(), 0);

                var unvisitedNodes = new List<Node>(nodes);
                var currentPath = new List<Node> { startNode };
                unvisitedNodes.Remove(startNode);

                var currentNode = startNode;
                var totalDistance = 0;

                // Nearest neighbor algorithm
                while (unvisitedNodes.Count > 0)
                {
                    Node? nearestNode = null;
                    int nearestDistance = int.MaxValue;

                    // Find the nearest unvisited node
                    foreach (var node in unvisitedNodes)
                    {
                        var connection = currentNode.GetConnectionTo(node);
                        if (connection != null && connection.Weight < nearestDistance)
                        {
                            nearestDistance = connection.Weight;
                            nearestNode = node;
                        }
                    }

                    if (nearestNode != null)
                    {
                        currentPath.Add(nearestNode);
                        unvisitedNodes.Remove(nearestNode);
                        totalDistance += nearestDistance;
                        currentNode = nearestNode;
                    }
                    else
                    {
                        // No connection found, algorithm fails
                        break;
                    }
                }

                // Return to start node
                var returnConnection = currentNode.GetConnectionTo(startNode);
                if (returnConnection != null)
                {
                    currentPath.Add(startNode);
                    totalDistance += returnConnection.Weight;
                }

                _bestPath = currentPath;
                _bestDistance = totalDistance;

                return (_bestPath, _bestDistance);
            });
        }

        public string FindBestPath(Graph graph)
        {
            _currentGraph = graph;
            _bestDistance = int.MaxValue;
            _bestPath.Clear();

            var result = new StringBuilder();
            result.AppendLine("TSP Nearest Neighbours Algorithm");
            result.AppendLine();

            // Try starting from each node in the graph
            foreach (var startNode in graph.Nodes)
            {
                var path = FindPathFromNode(startNode);
                var distance = CalculatePathDistance(path);

                result.AppendLine($"Starting from {startNode.NodeName}: Distance = {distance}");

                if (distance < _bestDistance && distance > 0)
                {
                    _bestDistance = distance;
                    _bestPath = new List<Node>(path);
                }
            }

            result.AppendLine();
            result.AppendLine($"Best distance: {_bestDistance}");
            result.AppendLine("Best path:");

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

        private List<Node> FindPathFromNode(Node startNode)
        {
            var currentNode = startNode;
            var visitedNodes = new List<Node>();
            var path = new List<Node>();

            // Reset graph visualization
            if (_currentGraph != null)
            {
                foreach (var node in _currentGraph.Nodes)
                {
                    node.ResetPathfinding();
                }
            }

            visitedNodes.Add(currentNode);
            path.Add(currentNode);

            // Continue until all nodes are visited
            while (visitedNodes.Count < _currentGraph?.Nodes.Count)
            {
                Node? nearestNode = null;
                int shortestDistance = int.MaxValue;

                // Find the nearest unvisited node
                foreach (var connection in currentNode.Connections)
                {
                    var targetNode = connection.Target;
                    if (!visitedNodes.Contains(targetNode) && connection.Weight < shortestDistance)
                    {
                        shortestDistance = connection.Weight;
                        nearestNode = targetNode;
                    }
                }

                if (nearestNode != null)
                {
                    visitedNodes.Add(nearestNode);
                    path.Add(nearestNode);
                    currentNode = nearestNode;
                }
                else
                {
                    // No more reachable nodes - incomplete path
                    break;
                }
            }

            return path;
        }

        private int CalculatePathDistance(List<Node> path)
        {
            if (path.Count < 2) return int.MaxValue;

            int totalDistance = 0;

            // Calculate distance for each step in the path
            for (int i = 0; i < path.Count - 1; i++)
            {
                var connection = path[i].GetConnectionTo(path[i + 1]);
                if (connection == null) return int.MaxValue; // Invalid path
                totalDistance += connection.Weight;
            }

            // Add distance to return to start
            var returnConnection = path.LastOrDefault()?.GetConnectionTo(path[0]);
            if (returnConnection == null) return int.MaxValue; // Can't return to start
            totalDistance += returnConnection.Weight;

            return totalDistance;
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
                    connection.DrawOrangeHighlightedLine(); // Orange for heuristic TSP path
                }
            }

            // Highlight return connection to start
            if (_bestPath.Count > 0)
            {
                var returnConnection = _bestPath[^1].GetConnectionTo(_bestPath[0]);
                if (returnConnection != null)
                {
                    returnConnection.DrawOrangeHighlightedLine();
                }
            }

            // Mark start node as source
            if (_bestPath.Count > 0)
            {
                _bestPath[0].SetAsSource();
            }

            // Trigger redraw to show highlighted connections
            _currentGraph?.TriggerRedraw();
        }
    }
}
