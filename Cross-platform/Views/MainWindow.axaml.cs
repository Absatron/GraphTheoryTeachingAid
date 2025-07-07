using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq;
using TeachingAidMac.Models;
using TeachingAidMac.Controls;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Avalonia.Data;
using System.Dynamic;
using Avalonia.Platform.Storage;

namespace TeachingAidMac.Views;

public partial class MainWindow : Window
{
    private Graph _graph = null!;
    private TSPBruteForce _bruteForceTSP = null!;
    private NearestNeighbours _nearestNeighbours = null!;
    private PathfindingAlgorithms _pathfinding = null!;
    private Node? _sourceNode;
    private Node? _targetNode;
    private bool _pixelMode = false;
    private int _animationDelay = 500;

    public MainWindow()
    {
        InitializeComponent();
        InitializeGraph();
        SetupEventHandlers();
        UpdateUI();
    }

    private void SetupEventHandlers()
    {
        var addNodeButton = this.FindControl<Button>("AddNodeButton");
        var removeNodeButton = this.FindControl<Button>("RemoveNodeButton");
        var loadGraphButton = this.FindControl<Button>("LoadGraphButton");
        var clearGraphButton = this.FindControl<Button>("ClearGraphButton");
        var switchModeButton = this.FindControl<Button>("SwitchModeButton");
        var fullyConnectButton = this.FindControl<Button>("FullyConnectButton");
        var saveGraphButton = this.FindControl<Button>("SaveGraphButton");
        var loadFileButton = this.FindControl<Button>("LoadFileButton");

        var tspButton = this.FindControl<Button>("TSPButton");
        var nearestNeighbourButton = this.FindControl<Button>("NearestNeighbourButton");
        var bfsButton = this.FindControl<Button>("BFSButton");
        var dfsButton = this.FindControl<Button>("DFSButton");
        var dijkstraButton = this.FindControl<Button>("DijkstraButton");
        var resetButton = this.FindControl<Button>("ResetButton");
        var findPathButton = this.FindControl<Button>("FindPathButton");

        var setSourceButton = this.FindControl<Button>("SetSourceButton");
        var setTargetButton = this.FindControl<Button>("SetTargetButton");
        var speedSlider = this.FindControl<Slider>("SpeedSlider");

        if (addNodeButton != null)
            addNodeButton.Click += AddNode_Click;
        if (removeNodeButton != null)
            removeNodeButton.Click += RemoveNode_Click;
        if (loadGraphButton != null)
            loadGraphButton.Click += LoadGraph_Click;
        if (clearGraphButton != null)
            clearGraphButton.Click += ClearGraph_Click;
        if (switchModeButton != null)
            switchModeButton.Click += SwitchMode_Click;
        if (fullyConnectButton != null)
            fullyConnectButton.Click += FullyConnect_Click;
        if (saveGraphButton != null)
            saveGraphButton.Click += SaveGraph_Click;
        if (loadFileButton != null)
            loadFileButton.Click += LoadFile_Click;

        if (tspButton != null)
            tspButton.Click += TSPBruteForce_Click;
        if (nearestNeighbourButton != null)
            nearestNeighbourButton.Click += TSPNearestNeighbour_Click;
        if (bfsButton != null)
            bfsButton.Click += BFS_Click;
        if (dfsButton != null)
            dfsButton.Click += DFS_Click;
        if (dijkstraButton != null)
            dijkstraButton.Click += Dijkstra_Click;
        if (resetButton != null)
            resetButton.Click += ResetNodes_Click;
        if (findPathButton != null)
            findPathButton.Click += FindPath_Click;

        if (setSourceButton != null)
            setSourceButton.Click += SetSource_Click;
        if (setTargetButton != null)
            setTargetButton.Click += SetTarget_Click;
        if (speedSlider != null)
            speedSlider.ValueChanged += SpeedSlider_ValueChanged;

        // Setup combo box event handlers
        var sourceCombo = this.FindControl<ComboBox>("SourceNodeCombo");
        var targetCombo = this.FindControl<ComboBox>("TargetNodeCombo");

        if (sourceCombo != null)
            sourceCombo.SelectionChanged += SourceCombo_SelectionChanged;
        if (targetCombo != null)
            targetCombo.SelectionChanged += TargetCombo_SelectionChanged;
    }

    private void InitializeGraph()
    {
        _graph = new Graph();

        _bruteForceTSP = new TSPBruteForce();
        _nearestNeighbours = new NearestNeighbours();
        _pathfinding = new PathfindingAlgorithms(_graph);

        var graphCanvas = this.FindControl<GraphCanvas>("GraphCanvas");
        if (graphCanvas != null)
        {
            graphCanvas.Graph = _graph;
            // Now that canvas is set, initialize the graph
            _graph.SetInitialGraph();
            // Force immediate visual update
            graphCanvas.InvalidateVisual();
        }

        // Set default source and target nodes
        var nodes = _graph.GetGraphNodes();
        if (nodes.Count > 0)
        {
            _sourceNode = nodes[0]; // Node A
            _sourceNode.VisualState = Node.NodeVisualState.Source;
        }
        if (nodes.Count > 6)
        {
            _targetNode = nodes[6]; // Node G
            _targetNode.VisualState = Node.NodeVisualState.Target;
        }
    }

    private void UpdateUI()
    {
        UpdateComboBoxes();
        UpdateStatistics();
        UpdateModeLabel();
        UpdateAdjacencyMatrix();
        UpdateStatus("Ready");
    }

    private void UpdateComboBoxes()
    {
        var sourceCombo = this.FindControl<ComboBox>("SourceNodeCombo");
        var targetCombo = this.FindControl<ComboBox>("TargetNodeCombo");

        var nodeNames = _graph.GetGraphNodes().Select(n => n.NodeName).ToList();

        if (sourceCombo != null)
        {
            sourceCombo.ItemsSource = nodeNames;
            sourceCombo.SelectedItem = _sourceNode?.NodeName;
        }

        if (targetCombo != null)
        {
            targetCombo.ItemsSource = nodeNames;
            targetCombo.SelectedItem = _targetNode?.NodeName;
        }
    }

    private void UpdateStatistics()
    {
        var nodeCountText = this.FindControl<TextBlock>("NodeCountText");
        var connectionCountText = this.FindControl<TextBlock>("ConnectionCountText");

        if (nodeCountText != null)
            nodeCountText.Text = $"Nodes: {_graph.GetGraphNodes().Count}";

        if (connectionCountText != null)
        {
            var connectionCount = _graph.GetAllConnections().Count;
            connectionCountText.Text = $"Connections: {connectionCount}";
        }
    }

    private void UpdateModeLabel()
    {
        var modeLabel = this.FindControl<TextBlock>("ModeLabel");
        if (modeLabel != null)
            modeLabel.Text = _pixelMode ? "Pixel" : "Manual";
    }

    private void UpdateStatus(string message)
    {
        var statusText = this.FindControl<TextBlock>("StatusText");
        if (statusText != null)
            statusText.Text = message;
    }

    private void UpdateOutput(string message)
    {
        var output = this.FindControl<TextBlock>("AlgorithmOutput");
        if (output != null)
            output.Text = message;
    }

    // Event Handlers
    private async void AddNode_Click(object? sender, RoutedEventArgs e)
    {
        var nodeNames = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var usedNames = _graph.GetGraphNodes().Select(n => n.NodeName[0]).ToHashSet();

        var nextName = nodeNames.FirstOrDefault(c => !usedNames.Contains(c));
        if (nextName != default(char))
        {
            _graph.AddNode(nextName.ToString());
            UpdateUI();
            UpdateStatus($"Added node {nextName}");
        }
        else
        {
            UpdateStatus("Graph is full (26 nodes maximum)");
        }
    }

    private async void RemoveNode_Click(object? sender, RoutedEventArgs e)
    {
        var nodes = _graph.GetGraphNodes();
        if (nodes.Count > 0)
        {
            var lastNode = nodes[^1];
            _graph.RemoveNode(lastNode.NodeName);

            if (_sourceNode == lastNode)
                _sourceNode = null;
            if (_targetNode == lastNode)
                _targetNode = null;

            UpdateUI();
            UpdateStatus($"Removed node {lastNode.NodeName}");
        }
        else
        {
            UpdateStatus("No nodes to remove");
        }
    }

    private void LoadGraph_Click(object? sender, RoutedEventArgs e)
    {
        _graph.ClearGraph();
        _graph.SetInitialGraph();

        // Set default source and target nodes
        var nodes = _graph.GetGraphNodes();
        if (nodes.Count > 0)
        {
            if (_sourceNode != null)
                _sourceNode.VisualState = Node.NodeVisualState.Normal;
            _sourceNode = nodes[0]; // Node A
            _sourceNode.VisualState = Node.NodeVisualState.Source;
        }
        if (nodes.Count > 6)
        {
            if (_targetNode != null)
                _targetNode.VisualState = Node.NodeVisualState.Normal;
            _targetNode = nodes[6]; // Node G
            _targetNode.VisualState = Node.NodeVisualState.Target;
        }

        UpdateUI();
        UpdateStatus("Sample graph loaded");
    }

    private void ClearGraph_Click(object? sender, RoutedEventArgs e)
    {
        _graph.ClearGraph();
        _sourceNode = null;
        _targetNode = null;
        UpdateUI();
        UpdateOutput("");
        UpdateStatus("Graph cleared");
    }

    private void SwitchMode_Click(object? sender, RoutedEventArgs e)
    {
        _pixelMode = !_pixelMode;
        _graph.PixelMode = _pixelMode;
        UpdateModeLabel();
        UpdateStatus($"Switched to {(_pixelMode ? "Pixel" : "Manual")} mode");
    }

    private void FullyConnect_Click(object? sender, RoutedEventArgs e)
    {
        _graph.FullyConnectNodes();
        UpdateUI();
        UpdateStatus("Graph fully connected");
    }

    private async void SaveGraph_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var saveFileDialog = new OpenFileDialog
            {
                Title = "Save Graph",
                AllowMultiple = false,
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "XML Files", Extensions = { "xml" } },
                    new FileDialogFilter { Name = "All Files", Extensions = { "*" } }
                }
            };

            // For now, just save to a default location
            var fileName = $"graph_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

            var graphData = CreateGraphData();

            var serializer = new XmlSerializer(typeof(GraphData));
            using (var writer = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(writer, graphData);
            }

            UpdateStatus($"Graph saved to {filePath}");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error saving graph: {ex.Message}");
        }
    }

    private async void LoadFile_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Load Graph",
                AllowMultiple = false,
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "XML Files", Extensions = { "xml" } },
                    new FileDialogFilter { Name = "All Files", Extensions = { "*" } }
                }
            };

            var files = await openFileDialog.ShowAsync(this);
            if (files != null && files.Length > 0)
            {
                var filePath = files[0];

                var serializer = new XmlSerializer(typeof(GraphData));
                using (var reader = new FileStream(filePath, FileMode.Open))
                {
                    var graphData = (GraphData)serializer.Deserialize(reader);
                    LoadGraphFromData(graphData);
                }

                UpdateStatus($"Graph loaded from {filePath}");
                UpdateUI();
            }
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading graph: {ex.Message}");
        }
    }

    private GraphData CreateGraphData()
    {
        var graphData = new GraphData();

        foreach (var node in _graph.GetGraphNodes())
        {
            var nodeData = new NodeData();
            nodeData.SetName(node.NodeName);
            nodeData.AddLocation(node.GetPosition());

            foreach (var connection in node.GetConnections())
            {
                var connectionData = new ConnectionData
                {
                    StartNodeLetter = connection.Source.NodeName,
                    EndNodeLetter = connection.Target.NodeName,
                    EdgeNumber = connection.Weight,
                    PreviousEdgeNumber = connection.PreviousWeight,
                    PixelMode = connection.PixelMode
                };
                nodeData.AddConnection(connectionData);
            }

            graphData.AddNode(nodeData);
        }

        if (_sourceNode != null)
            graphData.SetSourceNodeLetter(_sourceNode.NodeName);
        if (_targetNode != null)
            graphData.SetTargetNodeLetter(_targetNode.NodeName);

        return graphData;
    }

    private void LoadGraphFromData(GraphData graphData)
    {
        _graph.ClearGraph();

        // Create nodes first
        foreach (var nodeData in graphData.GetGraphNodes())
        {
            var node = new Node(true);
            node.SetName(nodeData.GetName());
            node.SetPosition(nodeData.GetLocation());
            _graph.AddNode(node);
        }

        // Then create connections
        foreach (var nodeData in graphData.GetGraphNodes())
        {
            var sourceNode = _graph.FindNodeByName(nodeData.GetName());
            if (sourceNode != null)
            {
                foreach (var connectionData in nodeData.GetConnections())
                {
                    var targetNode = _graph.FindNodeByName(connectionData.GetEndNodeLetter());
                    if (targetNode != null)
                    {
                        sourceNode.SetConnection(targetNode, connectionData.GetEdgeNumber());
                        var connection = sourceNode.GetConnectionTo(targetNode);
                        if (connection != null)
                        {
                            connection.PreviousWeight = connectionData.GetPreviousEdgeNumber();
                            connection.SetPixelMode(connectionData.GetPixelMode());
                        }
                    }
                }
            }
        }

        // Set source and target nodes
        if (!string.IsNullOrEmpty(graphData.GetSourceNodeLetter()))
        {
            _sourceNode = _graph.FindNodeByName(graphData.GetSourceNodeLetter());
            if (_sourceNode != null)
                _sourceNode.VisualState = Node.NodeVisualState.Source;
        }

        if (!string.IsNullOrEmpty(graphData.GetTargetNodeLetter()))
        {
            _targetNode = _graph.FindNodeByName(graphData.GetTargetNodeLetter());
            if (_targetNode != null)
                _targetNode.VisualState = Node.NodeVisualState.Target;
        }
    }

    private async void TSPBruteForce_Click(object? sender, RoutedEventArgs e)
    {
        if (_graph.GetGraphNodes().Count == 0)
        {
            UpdateStatus("No nodes in graph");
            return;
        }

        if (_graph.GetGraphNodes().Count > 10)
        {
            UpdateStatus("Too many nodes for brute force TSP (maximum 10)");
            return;
        }

        UpdateStatus("Running TSP Brute Force...");
        UpdateOutput("Starting TSP Brute Force algorithm...\n");

        try
        {
            var result = await _bruteForceTSP.SolveTSP(_graph.GetGraphNodes(), _animationDelay);

            if (result.path.Count > 0)
            {
                var pathStr = string.Join(" -> ", result.path.Select(n => n.NodeName));
                if (result.path.Count > 0)
                    pathStr += " -> " + result.path[0].NodeName; // Return to start

                UpdateOutput($"TSP Brute Force Results:\n\nBest path: {pathStr}\nTotal distance: {result.distance}\nNodes visited: {result.path.Count}\n\nComplexity: O(n!)");
                UpdateStatus($"TSP completed - Distance: {result.distance}");

                // The algorithm handles its own highlighting during execution
            }
            else
            {
                UpdateOutput("TSP Brute Force: No solution found");
                UpdateStatus("TSP failed");
            }
        }
        catch (Exception ex)
        {
            UpdateOutput($"Error running TSP: {ex.Message}");
            UpdateStatus("TSP failed");
        }
    }

    private async void TSPNearestNeighbour_Click(object? sender, RoutedEventArgs e)
    {
        if (_graph.GetGraphNodes().Count == 0)
        {
            UpdateStatus("No nodes in graph");
            return;
        }

        UpdateStatus("Running TSP Nearest Neighbour...");
        UpdateOutput("Starting TSP Nearest Neighbour algorithm...\n");

        try
        {
            var startNode = _sourceNode ?? _graph.GetGraphNodes().FirstOrDefault();
            if (startNode == null) return;

            var result = await _nearestNeighbours.SolveTSP(_graph.GetGraphNodes(), startNode, _animationDelay);

            if (result.path.Count > 0)
            {
                var pathStr = string.Join(" -> ", result.path.Select(n => n.NodeName));

                UpdateOutput($"TSP Nearest Neighbour Results:\n\nPath: {pathStr}\nTotal distance: {result.distance}\nNodes visited: {result.path.Count}\n\nComplexity: O(n²)");
                UpdateStatus($"TSP completed - Distance: {result.distance}");

                // The algorithm handles its own highlighting during execution
            }
            else
            {
                UpdateOutput("TSP Nearest Neighbour: No solution found");
                UpdateStatus("TSP failed");
            }
        }
        catch (Exception ex)
        {
            UpdateOutput($"Error running TSP: {ex.Message}");
            UpdateStatus("TSP failed");
        }
    }

    private async void BFS_Click(object? sender, RoutedEventArgs e)
    {
        if (_sourceNode == null || _targetNode == null)
        {
            UpdateStatus("Please set both source and target nodes");
            return;
        }

        UpdateStatus("Running BFS...");

        try
        {
            var result = await _pathfinding.BFS(_sourceNode, _targetNode, _animationDelay);
            UpdateOutput(result.output);

            if (result.path.Count > 0)
            {
                UpdateStatus($"BFS completed - Path length: {result.path.Count - 1}");
            }
            else
            {
                UpdateStatus("BFS completed - No path found");
            }
        }
        catch (Exception ex)
        {
            UpdateOutput($"Error running BFS: {ex.Message}");
            UpdateStatus("BFS failed");
        }
    }

    private async void DFS_Click(object? sender, RoutedEventArgs e)
    {
        if (_sourceNode == null || _targetNode == null)
        {
            UpdateStatus("Please set both source and target nodes");
            return;
        }

        UpdateStatus("Running DFS...");

        try
        {
            var result = await _pathfinding.DFS(_sourceNode, _targetNode, _animationDelay);
            UpdateOutput(result.output);

            if (result.path.Count > 0)
            {
                UpdateStatus($"DFS completed - Path length: {result.path.Count - 1}");
            }
            else
            {
                UpdateStatus("DFS completed - No path found");
            }
        }
        catch (Exception ex)
        {
            UpdateOutput($"Error running DFS: {ex.Message}");
            UpdateStatus("DFS failed");
        }
    }

    private async void Dijkstra_Click(object? sender, RoutedEventArgs e)
    {
        if (_sourceNode == null || _targetNode == null)
        {
            UpdateStatus("Please set both source and target nodes");
            return;
        }

        UpdateStatus("Running Dijkstra...");

        try
        {
            var result = await _pathfinding.Dijkstra(_sourceNode, _targetNode, _animationDelay);
            UpdateOutput(result.output);

            if (result.path.Count > 0)
            {
                UpdateStatus($"Dijkstra completed - Path length: {result.path.Count - 1}");
            }
            else
            {
                UpdateStatus("Dijkstra completed - No path found");
            }
        }
        catch (Exception ex)
        {
            UpdateOutput($"Error running Dijkstra: {ex.Message}");
            UpdateStatus("Dijkstra failed");
        }
    }

    private void ResetNodes_Click(object? sender, RoutedEventArgs e)
    {
        _graph.ResetAllNodes();
        UpdateStatus("All nodes reset");
    }

    private async void FindPath_Click(object? sender, RoutedEventArgs e)
    {
        // Use Dijkstra as the default pathfinding algorithm
        Dijkstra_Click(sender, e);
    }

    private void SetSource_Click(object? sender, RoutedEventArgs e)
    {
        var sourceCombo = this.FindControl<ComboBox>("SourceNodeCombo");
        if (sourceCombo?.SelectedItem is string nodeName)
        {
            var newSource = _graph.FindNodeByName(nodeName);
            if (newSource != null)
            {
                if (_sourceNode != null)
                    _sourceNode.VisualState = Node.NodeVisualState.Normal;

                _sourceNode = newSource;
                _sourceNode.VisualState = Node.NodeVisualState.Source;
                UpdateStatus($"Source node set to {nodeName}");
            }
        }
    }

    private void SetTarget_Click(object? sender, RoutedEventArgs e)
    {
        var targetCombo = this.FindControl<ComboBox>("TargetNodeCombo");
        if (targetCombo?.SelectedItem is string nodeName)
        {
            var newTarget = _graph.FindNodeByName(nodeName);
            if (newTarget != null)
            {
                if (_targetNode != null)
                    _targetNode.VisualState = Node.NodeVisualState.Normal;

                _targetNode = newTarget;
                _targetNode.VisualState = Node.NodeVisualState.Target;
                UpdateStatus($"Target node set to {nodeName}");
            }
        }
    }

    private void SpeedSlider_ValueChanged(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        _animationDelay = (int)e.NewValue;
        var speedLabel = this.FindControl<TextBlock>("SpeedLabel");
        if (speedLabel != null)
            speedLabel.Text = $"{_animationDelay}ms";
    }

    private void SourceCombo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox combo && combo.SelectedItem is string nodeName)
        {
            var newSource = _graph.FindNodeByName(nodeName);
            if (newSource != null)
            {
                if (_sourceNode != null)
                    _sourceNode.VisualState = Node.NodeVisualState.Normal;

                _sourceNode = newSource;
                _sourceNode.VisualState = Node.NodeVisualState.Source;
                UpdateStatus($"Source node set to {nodeName}");
            }
        }
    }

    private void TargetCombo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox combo && combo.SelectedItem is string nodeName)
        {
            var newTarget = _graph.FindNodeByName(nodeName);
            if (newTarget != null)
            {
                if (_targetNode != null)
                    _targetNode.VisualState = Node.NodeVisualState.Normal;

                _targetNode = newTarget;
                _targetNode.VisualState = Node.NodeVisualState.Target;
                UpdateStatus($"Target node set to {nodeName}");
            }
        }
    }

    // VB.NET compatibility methods
    public Graph GetCurrentGraph() => _graph;
    public bool GetPixelMode() => _pixelMode;
    public void SetPixelMode(bool pixelMode)
    {
        _pixelMode = pixelMode;
        _graph.PixelMode = pixelMode;
        UpdateModeLabel();
    }

    private void UpdateAdjacencyMatrix()
    {
        try
        {
            if (_graph == null) return;

            var matrixGrid = this.FindControl<DataGrid>("AdjacencyMatrix");
            if (matrixGrid != null)
            {
                var nodes = _graph.GetGraphNodes();

                if (nodes.Count > 0)
                {
                    // For now, just show the adjacency list in the algorithm output
                    var adjacencyList = _graph.GetAdjacencyList();
                    var output = this.FindControl<TextBlock>("AlgorithmOutput");
                    if (output != null && adjacencyList.Count > 0)
                    {
                        var matrixText = "Adjacency List:\n" + string.Join("\n", adjacencyList);
                        // Only update if it's not showing algorithm results
                        if (!output.Text.Contains("Algorithm") && !output.Text.Contains("TSP") && !output.Text.Contains("BFS"))
                        {
                            output.Text = matrixText;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions gracefully
            UpdateStatus($"Error updating adjacency matrix: {ex.Message}");
        }
    }
}