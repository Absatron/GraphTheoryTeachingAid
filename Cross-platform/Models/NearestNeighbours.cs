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

        public async Task<(List<Node> path, int distance)> SolveTSP(List<Node> nodes, Node startNode, int animationDelay = 500)
        {
            return await Task.Run(async () =>
            {
                _bestPath.Clear();
                _bestDistance = int.MaxValue;

                if (nodes.Count == 0)
                    return (new List<Node>(), 0);

                // Try starting from each node to find the best path
                foreach (var tryStartNode in nodes)
                {
                    // Reset graph visualization
                    foreach (var node in nodes)
                    {
                        node.DrawNormalNode();
                        foreach (var connection in node.GetConnections())
                        {
                            connection.DrawLine();
                        }
                    }

                    var currentPath = await FindPathWithVisualization(tryStartNode, nodes, animationDelay);
                    var distance = CalculatePathDistance(currentPath);

                    if (distance < _bestDistance && distance > 0)
                    {
                        _bestDistance = distance;
                        _bestPath = new List<Node>(currentPath);
                        await HighlightBestPath(_bestPath, tryStartNode, animationDelay);
                        _bestPath.Add(tryStartNode); // Complete the cycle
                    }
                    else if (currentPath.Count > 0)
                    {
                        await HighlightPathFound(currentPath, tryStartNode, animationDelay);
                    }
                }

                return (_bestPath, _bestDistance == int.MaxValue ? 0 : _bestDistance);
            });
        }

        private async Task<List<Node>> FindPathWithVisualization(Node startNode, List<Node> allNodes, int animationDelay)
        {
            var unvisitedNodes = new List<Node>(allNodes);
            var currentPath = new List<Node> { startNode };
            unvisitedNodes.Remove(startNode);

            var currentNode = startNode;

            // Highlight starting node
            startNode.DrawRedHighlightedNode();
            await Task.Delay(animationDelay / 5);

            while (unvisitedNodes.Count > 0)
            {
                Node? nearestNode = null;
                int nearestDistance = int.MaxValue;
                Connection? nearestConnection = null;

                // Find the nearest unvisited node with visualization
                foreach (var node in unvisitedNodes)
                {
                    var connection = currentNode.GetConnectionTo(node);
                    if (connection != null)
                    {
                        // Highlight connection being explored
                        connection.DrawRedHighlightedLine();
                        node.DrawRedHighlightedNode();
                        await Task.Delay(animationDelay / 5); // Animation delay

                        // Reset highlighting
                        connection.DrawLine();
                        node.DrawNormalNode();

                        if (connection.Weight < nearestDistance)
                        {
                            nearestDistance = connection.Weight;
                            nearestNode = node;
                            nearestConnection = connection;
                        }
                    }
                }

                if (nearestNode != null && nearestConnection != null)
                {
                    // Move to the nearest node
                    currentPath.Add(nearestNode);
                    unvisitedNodes.Remove(nearestNode);
                    currentNode = nearestNode;

                    // Highlight the chosen path
                    currentNode.DrawRedHighlightedNode();
                    nearestConnection.DrawRedHighlightedLine();
                    await Task.Delay(animationDelay / 5);
                }
                else
                {
                    // No connection found, algorithm fails
                    break;
                }

                // Check if we need to return to start for complete cycle
                if (unvisitedNodes.Count == 0)
                {
                    var returnConnection = currentNode.GetConnectionTo(startNode);
                    if (returnConnection != null)
                    {
                        returnConnection.DrawRedHighlightedLine();
                        await Task.Delay(animationDelay / 5);
                    }
                }
            }

            return currentPath;
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

        private async Task HighlightBestPath(List<Node> path, Node startNode, int animationDelay)
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
                var returnConnection = path[path.Count - 1].GetConnectionTo(startNode);
                returnConnection?.DrawGreenHighlightedLine();
            }

            await Task.Delay(animationDelay / 2); // Similar to VB.NET threading delay
        }

        private async Task HighlightPathFound(List<Node> path, Node startNode, int animationDelay)
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
                var returnConnection = path[path.Count - 1].GetConnectionTo(startNode);
                returnConnection?.DrawOrangeHighlightedLine();
            }

            await Task.Delay(animationDelay / 2); // Similar to VB.NET threading delay
        }
    }
}
