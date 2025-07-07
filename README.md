# Graph Theory Teacher Aid

A comprehensive Visual Basic .NET application designed to help teachers and students visualize and understand fundamental graph theory algorithms through interactive demonstrations.

## Overview

This educational tool provides a visual, interactive environment for exploring graph theory concepts, making abstract algorithms tangible and easier to understand. The application features a graphical interface where users can create, modify, and analyze graphs while watching algorithms execute step-by-step.

## Features

### Graph Creation and Editing

- **Interactive Node Management**: Add, remove, and rename nodes (A-Z) with visual circular representations
- **Dynamic Edge Connections**: Create weighted connections between nodes with customizable edge weights
- **Drag and Drop Interface**: Move nodes around the canvas to create optimal graph layouts
- **Two Edge Weight Modes**:
  - **Manual Mode**: Set custom edge weights
  - **Pixel Mode**: Automatically calculate edge weights based on visual distance between nodes

### Algorithm Visualizations

#### Pathfinding Algorithms

- **Breadth-First Search (BFS)**: Find paths using queue-based level-by-level exploration
- **Depth-First Search (DFS)**: Explore paths using recursive depth-first traversal
- **Dijkstra's Algorithm**: Find shortest paths with weighted edges, displaying optimal routes and distances

#### Optimization Algorithms

- **Traveling Salesman Problem (TSP)**:
  - **Brute Force**: Exact solution for small graphs (< 6 nodes) by checking all permutations
  - **Nearest Neighbor Heuristic**: Approximate solution for larger graphs using greedy approach

### Visual Features

- **Color-Coded Algorithm Steps**:
  - Red: Currently processing/exploring
  - Green: Best path found or processed nodes
  - Orange: Alternative paths explored
  - Yellow: Highlighted final solutions
- **Adjustable Animation Speed**: Control visualization speed with trackbar
- **Real-Time Path Highlighting**: See algorithms work step-by-step with visual feedback

### Graph Representations

- **Adjacency Matrix**: Tabular view showing connections and weights between all nodes
- **Adjacency List**: List format displaying each node's connections and corresponding weights
- **Auto-updating**: Both representations update automatically as the graph changes

### Data Management

- **Save/Load Functionality**: Preserve graph configurations using XML serialization
- **Predefined Sample Graph**: Quick-start with a pre-configured 7-node example graph
- **Graph Reset Options**: Clear graph or reload initial configuration

## Technical Architecture

### Core Classes

- **`Graph`**: Main graph container managing nodes and connections
- **`Node`**: Interactive UI elements representing graph vertices with drag-and-drop functionality
- **`Connection`**: Represents weighted edges between nodes with visual rendering
- **`Stack`** and **`Queue`**: Custom data structures for algorithm implementations

### Algorithm Implementations

- **`TSPBruteForce`**: Exhaustive search for optimal TSP solutions
- **`NearestNeighbours`**: Heuristic approach for approximate TSP solutions

### Data Persistence

- **`GraphData`**, **`NodeData`**, **`ConnectionData`**: Serializable classes for saving/loading graph states

## Usage Instructions

### Getting Started

1. Run the application to see the default 7-node sample graph
2. Use "Load Initial Graph" to reset to the sample configuration
3. Experiment with the pre-loaded graph or create your own

### Creating Custom Graphs

1. Click "Add Node" and enter a capital letter (A-Z)
2. Right-click any node to see connection options
3. Check/uncheck other nodes to create/remove connections
4. Enter edge weights when prompted (in manual mode)
5. Use "Switch Mode" to toggle between manual and pixel-based edge weights

### Running Algorithms

1. Set source and target nodes using the respective buttons
2. Choose an algorithm (BFS, DFS, Dijkstra's, or TSP)
3. Adjust the speed slider to control animation speed
4. Watch the algorithm visualize its execution process
5. View results in message boxes showing paths and distances

### Advanced Features

- **Fully Connect**: Automatically create connections between all nodes (pixel mode only)
- **Save/Load**: Preserve interesting graph configurations for later use
- **Multiple Representations**: Switch between visual, matrix, and list views

## Educational Value

This tool is ideal for:

- **Computer Science Courses**: Demonstrating algorithm complexity and behavior
- **Mathematics Education**: Visualizing graph theory concepts
- **Self-Study**: Interactive learning of pathfinding and optimization algorithms
- **Algorithm Comparison**: Side-by-side analysis of different approaches

## Technical Requirements

- **Platform**: Windows with .NET Framework
- **Language**: Visual Basic .NET
- **UI Framework**: Windows Forms
- **File I/O**: XML serialization for data persistence

> **Note**: A cross-platform version of this project has been roughly converted to C# using Avalonia UI framework and can be found in the `Cross-platform/` directory. This version allows the application to run on macOS, Linux, and Windows.

## Algorithm Complexity Demonstrations

- **BFS/DFS**: O(V + E) time complexity visualization
- **Dijkstra's**: O(V²) or O(E + V log V) depending on implementation
- **TSP Brute Force**: O(n!) factorial complexity demonstration
- **Nearest Neighbor**: O(n²) heuristic approach

This application bridges the gap between theoretical computer science concepts and practical understanding through interactive visualization and hands-on experimentation.
