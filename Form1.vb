Imports System.IO 

Imports System.Xml.Serialization 

 

Public Class Form1 

    Dim TheGraph As Graph 

    Dim SourceNode As Node 

    Dim TargetNode As Node 

    Dim PixelMode As Boolean = False 

    Dim SavedGraphData As New GraphData 

    Dim BruteForceTSP As New TSPBruteForce 

    Dim NNeighbours As New NearestNeighbours 

 

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown 

        Me.FormBorderStyle = FormBorderStyle.Fixed3D 

        Me.MaximizeBox = False 

        Me.Text = "NEA" 

 

        ClearGraphButton.FlatAppearance.BorderColor = Color.LightBlue 

        ClearGraphButton.BackColor = Color.Black 

        ClearGraphButton.ForeColor = Color.LightBlue 

        ClearGraphButton.Font = New Font("Stencil", 12, FontStyle.Bold) 

        ClearGraphButton.Cursor = Cursors.Hand 

 

        AddNodeButton.FlatAppearance.BorderColor = Color.LightBlue 

        AddNodeButton.BackColor = Color.Black 

        AddNodeButton.ForeColor = Color.LightBlue 

        AddNodeButton.Font = New Font("Stencil", 12, FontStyle.Bold) 

        AddNodeButton.Cursor = Cursors.Hand 

 

        RemoveNodeButton.FlatAppearance.BorderColor = Color.LightBlue 

        RemoveNodeButton.BackColor = Color.Black 

        RemoveNodeButton.ForeColor = Color.LightBlue 

        RemoveNodeButton.Font = New Font("Stencil", 12, FontStyle.Bold) 

        RemoveNodeButton.Cursor = Cursors.Hand 

 

        SwitchModeButton.FlatAppearance.BorderColor = Color.LightBlue 

        SwitchModeButton.BackColor = Color.Black 

        SwitchModeButton.ForeColor = Color.LightBlue 

        SwitchModeButton.Font = New Font("Stencil", 12, FontStyle.Bold) 

        SwitchModeButton.Cursor = Cursors.Hand 

 

        FullyConnectButton.FlatAppearance.BorderColor = Color.LightBlue 

        FullyConnectButton.BackColor = Color.Black 

        FullyConnectButton.ForeColor = Color.LightBlue 

        FullyConnectButton.Font = New Font("Stencil", 12, FontStyle.Bold) 

        FullyConnectButton.Cursor = Cursors.Hand 

 

        SetSourceNodeButton.FlatAppearance.BorderColor = Color.LightBlue 

        SetSourceNodeButton.BackColor = Color.Black 

        SetSourceNodeButton.ForeColor = Color.LightBlue 

        SetSourceNodeButton.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

        SetSourceNodeButton.Cursor = Cursors.Hand 

 

        SetTargetNodeButton.FlatAppearance.BorderColor = Color.LightBlue 

        SetTargetNodeButton.BackColor = Color.Black 

        SetTargetNodeButton.ForeColor = Color.LightBlue 

        SetTargetNodeButton.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

        SetTargetNodeButton.Cursor = Cursors.Hand 

 

        Breadthfirstsearch.FlatAppearance.BorderColor = Color.LightBlue 

        Breadthfirstsearch.BackColor = Color.Black 

        Breadthfirstsearch.ForeColor = Color.LightBlue 

        Breadthfirstsearch.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

        Breadthfirstsearch.Cursor = Cursors.Hand 

 

        DepthFirstSearch.FlatAppearance.BorderColor = Color.LightBlue 

        DepthFirstSearch.BackColor = Color.Black 

        DepthFirstSearch.ForeColor = Color.LightBlue 

        DepthFirstSearch.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

        DepthFirstSearch.Cursor = Cursors.Hand 

 

        DijkstrasAlgorithm.FlatAppearance.BorderColor = Color.LightBlue 

        DijkstrasAlgorithm.BackColor = Color.Black 

        DijkstrasAlgorithm.ForeColor = Color.LightBlue 

        DijkstrasAlgorithm.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

        DijkstrasAlgorithm.Cursor = Cursors.Hand 

 

        LoadInitialGraphButton.FlatAppearance.BorderColor = Color.LightBlue 

        LoadInitialGraphButton.BackColor = Color.Black 

        LoadInitialGraphButton.ForeColor = Color.LightBlue 

        LoadInitialGraphButton.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

        LoadInitialGraphButton.Cursor = Cursors.Hand 

 

        SaveCurrentGraphButton.FlatAppearance.BorderColor = Color.LightBlue 

        SaveCurrentGraphButton.BackColor = Color.Black 

        SaveCurrentGraphButton.ForeColor = Color.LightBlue 

        SaveCurrentGraphButton.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

        SaveCurrentGraphButton.Cursor = Cursors.Hand 

 

        LoadSavedGraphButton.FlatAppearance.BorderColor = Color.LightBlue 

        LoadSavedGraphButton.BackColor = Color.Black 

        LoadSavedGraphButton.ForeColor = Color.LightBlue 

        LoadSavedGraphButton.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

        LoadSavedGraphButton.Cursor = Cursors.Hand 

 

        TravellingSalesmanButton.FlatAppearance.BorderColor = Color.LightBlue 

        TravellingSalesmanButton.BackColor = Color.Black 

        TravellingSalesmanButton.ForeColor = Color.LightBlue 

        TravellingSalesmanButton.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

        TravellingSalesmanButton.Cursor = Cursors.Hand 

 

        SourceNodeLabel.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

        TargetNodeLabel.Font = New Font("Modern No. 20", 8, FontStyle.Bold) 

 

 

        TrackBarLabel.Font = New Font("Modern No. 20", 12, FontStyle.Bold) 

        TrackBarLabel.Text = "Speed of search:" 

 

        SleepTrackBar.BackColor = Color.Black 

        SleepTrackBar.Cursor = Cursors.Hand 

 

        TheGraph = New Graph 

        SourceNode = TheGraph.GetGraphNodes()(0) 

        TargetNode = TheGraph.GetGraphNodes()(6) 

 

        SourceNodeLabel.Text = "Source Node: " & SourceNode.GetName() 

        TargetNodeLabel.Text = "Target Node: " & TargetNode.GetName() 

 

 

 

        Adjacencymatrix.ReadOnly = True 

        Adjacencymatrix.AllowUserToResizeColumns = False 

        Adjacencymatrix.AllowUserToResizeRows = False 

        Adjacencymatrix.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing 

        Adjacencymatrix.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing 

        Adjacencymatrix.BorderStyle = BorderStyle.FixedSingle 

        Adjacencymatrix.BackgroundColor = Color.LightBlue 

        Adjacencymatrix.Cursor = Cursors.No 

 

        AdjacencyList.ReadOnly = True 

        AdjacencyList.AllowUserToResizeColumns = False 

        AdjacencyList.AllowUserToResizeRows = False 

        AdjacencyList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing 

        AdjacencyList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing 

        AdjacencyList.BorderStyle = BorderStyle.FixedSingle 

        AdjacencyList.BackgroundColor = Color.LightBlue 

        AdjacencyList.Cursor = Cursors.No 

 

        FillAdjacencyMatrix() 

        FillAdjacencyList() 

 

 

    End Sub 

    Public Function GetCurrentGraph() As Graph 

        Return Me.TheGraph 

    End Function 

    Public Function GetPixelMode() As Boolean 

        Return Me.PixelMode 

    End Function 

    Public Sub SetSourceNodeWhenRemoved() 

        Me.SourceNode = Nothing 

    End Sub 

    Public Sub SetTargetNodeWhenRemoved() 

        Me.TargetNode = Nothing 

    End Sub 

    Public Sub FillAdjacencyMatrix() 

        Adjacencymatrix.Columns.Clear() 

        Adjacencymatrix.Rows.Clear() 

        'sets the column headers with current nodes in the graph 

        Dim EmptyColumn As New DataGridViewTextBoxColumn() 

        EmptyColumn.Width = 40 

        Adjacencymatrix.Columns.Add(EmptyColumn) 

        For Each node As Node In TheGraph.GetGraphNodes() 

            Dim column As New DataGridViewTextBoxColumn() 

            column.Width = 40 

            column.HeaderText = node.GetName() 

            Adjacencymatrix.Columns.Add(column) 

        Next 

        'sets the rows based on the current nodes, current edges and the column headers 

        For Each node As Node In TheGraph.GetGraphNodes() 

            Dim row As DataGridViewRow = Adjacencymatrix.Rows(Adjacencymatrix.Rows.Add()) 

            row.Height = 20 

            row.Cells(0).Value = node.GetName() 

            For ColumnNumber = 1 To Adjacencymatrix.ColumnCount - 1 

                Dim MatrixConnection As Connection 

                MatrixConnection = node.GetConnections().Find(Function(x) x.GetStartNode().GetName() = node.GetName() And x.GetEndNode.GetName() = Adjacencymatrix.Columns(ColumnNumber).HeaderText) 

                If MatrixConnection Is Nothing Then 

                    row.Cells(ColumnNumber).Value = 0 

                Else 

                    row.Cells(ColumnNumber).Value = MatrixConnection.GetEdgeNumber() 

                End If 

 

            Next 

        Next 

 

    End Sub 

    Public Sub FillAdjacencyList() 

 

        AdjacencyList.Columns.Clear() 

        AdjacencyList.Rows.Clear() 

        'sets the the columns 

        Dim Column1 As New DataGridViewTextBoxColumn() 

        Column1.Width = 50 

        Column1.HeaderText = "Node" 

        AdjacencyList.Columns.Add(Column1) 

        Dim Column2 As New DataGridViewTextBoxColumn() 

        Column2.Width = 400 

        Column2.HeaderText = "Adjacent to" 

        AdjacencyList.Columns.Add(Column2) 

 

        For Each node As Node In TheGraph.GetGraphNodes() 

            ' sets the first cell of the rows to the node name 

            Dim row As DataGridViewRow = New DataGridViewRow() 

            row.Height = 20 

            row.Cells.Add(New DataGridViewTextBoxCell() With {.Value = node.GetName()}) 

            row.Cells.Add(New DataGridViewTextBoxCell) 

            row.Cells(1).Value = "" 

            For Each connection As Connection In node.GetConnections() 

                'lists the connected nodes and the corresponding edge number in the second cell of the rows   

                row.Cells(1).Value &= connection.GetEndNode().GetName() & "," & Str(connection.GetEdgeNumber()).Replace(" ", "") & "; " 

            Next 

            AdjacencyList.Rows.Add(row) 

        Next 

 

    End Sub 

    Private Sub BreadthfirstSearch_Click(sender As System.Object, e As System.EventArgs) Handles Breadthfirstsearch.Click 

        If IsNothing(SourceNode) Then 

            MsgBox("Set Source Node") 

        ElseIf IsNothing(TargetNode) Then 

            MsgBox("Set Target Node") 

        Else 

            Dim CurrentNode As Node 

            Dim TheQueue As New Queue 

            Dim Solution As String 

            Dim TheStack As New Stack 

 

            'resets graph 

            For Each node As Node In TheGraph.GetGraphNodes() 

                node.SetIsDisovered(False) 

                node.SetIsProcessed(False) 

            Next 

 

            For Each Node As Node In TheGraph.GetGraphNodes() 

                For Each Connection As Connection In Node.GetConnections() 

                    Connection.RemoveLine() 

                    Connection.DrawLine() 

                Next 

            Next 

 

            CurrentNode = SourceNode 

 

            TheQueue.Push(SourceNode) 

            SourceNode.SetIsDisovered(True) 

 

            Do 

                'processes all nodes by adding all discovered nodes to queue and working through them sequentially 

                CurrentNode = TheQueue.Pop 

                CurrentNode.DrawRedHighlightedNode() 

                Threading.Thread.Sleep(100 * SleepTrackBar.Value) 

 

                ' adds all connected nodes to the queue then sets node to processed 

                For Each connection As Connection In CurrentNode.GetConnections() 

                    connection.DrawRedHighlightedLine() 

                    Threading.Thread.Sleep(100 * SleepTrackBar.Value) 

 

                    If connection.GetEndNode().IsDiscovered = False Then 

                        CurrentNode.DrawRedHighlightedNode() 

                        Threading.Thread.Sleep(50 * SleepTrackBar.Value) 

 

                        TheQueue.Push(connection.GetEndNode()) 

                        connection.GetEndNode().SetIsDisovered(True) 

                        connection.GetEndNode().SetPreviousNode(CurrentNode) 

                    End If 

                Next 

 

                For Each connection As Connection In CurrentNode.GetConnections() 

                    connection.DrawLine() 

                Next 

 

                CurrentNode.SetIsProcessed(True) 

                CurrentNode.DrawGreenHighlightedNode() 

                Threading.Thread.Sleep(100 * SleepTrackBar.Value) 

 

            Loop Until TheQueue.IsEmpty = True 

 

            'checks if the target node has been found 

            If TargetNode.IsProcessed = False Then 

                MsgBox("There is no path from the source node to the target node") 

            Else 

 

                Dim ANode As Node 

                ANode = TargetNode 

                ' gets the path in reverse order by working bacwards from the source node 

                Do While ANode.GetName <> SourceNode.GetName 

 

                    TheStack.Push(ANode) 

 

                    ANode = ANode.GetPreviousNode 

 

                Loop 

                'gets the path in order - from the source node by popping from the stack 

                Solution = SourceNode.GetName 

                Do 

                    Dim PoppedNode As New Node(False) 

                    PoppedNode = TheStack.Pop 

                    Solution = Solution + " " + PoppedNode.GetName 

                Loop Until TheStack.IsEmpty = True 

 

                Dim Path As String 

                Path = Solution.Replace(" ", "") 

 

                For Each Node As Node In TheGraph.GetGraphNodes() 

                    For Each Connection As Connection In Node.GetConnections() 

                        Connection.RemoveLine() 

                        Connection.DrawLine() 

                    Next 

                Next 

 

                'highlights the path found 

                For looper = 0 To Len(Path) - 1 

 

                    If looper = Len(Path) - 1 Then 

 

                    Else 

                        Dim StartNode As Node 

                        StartNode = TheGraph.GetNodeFromName(Path(looper)) 

                        Dim EndNode As Node 

                        EndNode = TheGraph.GetNodeFromName(Path(looper + 1)) 

                        StartNode.GetConnections().Find(Function(x) x.GetEndNode().GetName = EndNode.GetName()).DrawHighlightedLine(StartNode, EndNode) 

                    End If 

                Next 

                MsgBox("Path found: " & Solution) 

            End If 

        End If 

 

        For Each node As Node In TheGraph.GetGraphNodes() 

            node.DrawNormalNode() 

        Next 

 

 

 

 

    End Sub 

 

    Private Sub DepthFirstSearch_Click(sender As System.Object, e As System.EventArgs) Handles DepthFirstSearch.Click 

        If IsNothing(SourceNode) Then 

            MsgBox("Set Source Node") 

        ElseIf IsNothing(TargetNode) Then 

            MsgBox("Set Target Node") 

        Else 

            Dim CurrentNode As Node 

            Dim Solution As String 

            Dim TheStack As New Stack 

 

            'resets graph 

            For Each node As Node In TheGraph.GetGraphNodes() 

                node.SetIsDisovered(False) 

                node.SetIsProcessed(False) 

            Next 

 

            For Each Node As Node In TheGraph.GetGraphNodes() 

                For Each Connection As Connection In Node.GetConnections() 

                    Connection.RemoveLine() 

                    Connection.DrawLine() 

                Next 

            Next 

 

            CurrentNode = SourceNode 

            ' this visits each discovered nodes immediate neighbour until it cannot 

            'it then goes back to the previous node and repeats the process for the next neoghbour if it has not been discovered on the previous series of visits 

            DepthFirstTraversal(CurrentNode) 

 

            For Each Node As Node In TheGraph.GetGraphNodes() 

                Node.DrawNormalNode() 

            Next 

 

            'checks if the target node has been found 

            If TargetNode.IsProcessed = False Then 

                MsgBox("There is no path from the source node to the target node") 

            Else 

                Dim ANode As Node 

                ANode = TargetNode 

                ' gets the path in reverse order by working bacwards from the source node 

                Do While ANode.GetName <> SourceNode.GetName 

 

                    TheStack.Push(ANode) 

 

                    ANode = ANode.GetPreviousNode 

 

                Loop 

                'gets the path in order - from the source node by popping from the stack 

                Solution = SourceNode.GetName 

                Do 

                    Dim PoppedNode As Node 

                    PoppedNode = TheStack.Pop 

                    Solution = Solution + " " + PoppedNode.GetName 

                Loop Until TheStack.IsEmpty = True 

 

                Dim Path As String 

                Path = Solution.Replace(" ", "") 

 

                For Each Node As Node In TheGraph.GetGraphNodes() 

                    For Each Connection As Connection In Node.GetConnections() 

                        Connection.RemoveLine() 

                        Connection.DrawLine() 

                    Next 

                Next 

 

                'highlights the path found 

                For looper = 0 To Len(Path) - 1 

 

                    If looper = Len(Path) - 1 Then 

 

                    Else 

                        Dim StartNode As Node 

                        StartNode = TheGraph.GetNodeFromName(Path(looper)) 

                        Dim EndNode As Node 

                        EndNode = TheGraph.GetNodeFromName(Path(looper + 1)) 

                        StartNode.GetConnections().Find(Function(x) x.GetEndNode().GetName = EndNode.GetName()).DrawHighlightedLine(StartNode, EndNode) 

                    End If 

                Next 

 

                MsgBox("Path found: " & Solution) 

            End If 

        End If 

 

 

 

    End Sub 

    Private Sub DepthFirstTraversal(CurrentNode As Node) 

        CurrentNode.SetIsDisovered(True) 

        CurrentNode.DrawRedHighlightedNode() 

        Threading.Thread.Sleep(50 * SleepTrackBar.Value) 

 

        For Each connection As Connection In CurrentNode.GetConnections() 

            connection.DrawRedHighlightedLine() 

            Threading.Thread.Sleep(100 * SleepTrackBar.Value) 

 

            connection.DrawLine() 

            If connection.GetEndNode().IsDiscovered = False Then 

                connection.GetEndNode().SetIsDisovered(True) 

                connection.GetEndNode().SetPreviousNode(CurrentNode) 

 

                DepthFirstTraversal(connection.GetEndNode()) 

            End If 

        Next 

        CurrentNode.SetIsProcessed(True) 

        CurrentNode.DrawGreenHighlightedNode() 

        Threading.Thread.Sleep(100 * SleepTrackBar.Value) 

    End Sub 

 

    Private Sub DijkstrasAlgorithm_Click(sender As System.Object, e As System.EventArgs) Handles DijkstrasAlgorithm.Click 

        If IsNothing(SourceNode) Then 

            MsgBox("Set Source Node") 

        ElseIf IsNothing(TargetNode) Then 

            MsgBox("Set Target Node") 

        Else 

            Dim TheStack As New Stack 

            Dim CurrentNode As Node 

            Dim AlternativeDistance As Integer 

            Dim NextNearestNode As Node 

            Dim EmptyNode As New Node(False) 

            Dim AllNodesProcessed As Boolean = False 

            Dim Solution As String 

            Dim ShortestPath As Integer 

 

            Dim ConnectedNodes As New List(Of Node) 

            Dim TheQueue As New Queue 

 

 

            ' reset the graph 

            For Each node As Node In TheGraph.GetGraphNodes() 

                node.SetIsDisovered(False) 

                node.SetIsProcessed(False) 

                node.SetShortestFromsource(1000000) 

            Next 

 

            For Each Node As Node In TheGraph.GetGraphNodes() 

                For Each Connection As Connection In Node.GetConnections() 

                    Connection.RemoveLine() 

                    Connection.DrawLine() 

                Next 

            Next 

 

            'gets the nodes connected to the source node - if target node not connected exception thrown 

            TheQueue.Push(SourceNode) 

            SourceNode.SetIsDisovered(True) 

            Do 

                CurrentNode = TheQueue.Pop 

                ConnectedNodes.Add(CurrentNode) 

                For Each connection As Connection In CurrentNode.GetConnections() 

                    If connection.GetEndNode().IsDiscovered = False Then 

                        TheQueue.Push(connection.GetEndNode()) 

                        connection.GetEndNode().SetIsDisovered(True) 

                    End If 

                Next 

            Loop Until TheQueue.IsEmpty = True 

 

            If TargetNode.IsDiscovered = False Then 

                MsgBox("There is no path from the source node to the target node") 

            Else 

 

 

 

                EmptyNode.SetShortestFromsource(10000) 

 

 

                CurrentNode = SourceNode 

                CurrentNode.SetShortestFromsource(0) 

                TargetNode.SetShortestFromsource(100000) 

                Do 

                    CurrentNode.DrawRedHighlightedNode() 

                    Threading.Thread.Sleep(50 * SleepTrackBar.Value) 

 

                    ' loop through the current node's connected nodes  

                    For Each connection As Connection In CurrentNode.GetConnections() 

                        connection.DrawRedHighlightedLine() 

                        Threading.Thread.Sleep(100 * SleepTrackBar.Value) 

 

                        If connection.GetEndNode().IsProcessed = False Then 

 

                            'update the shortest distance of the connected node from the source node 

                            AlternativeDistance = CurrentNode.GetShortestFromSource + connection.GetEdgeNumber() 

                            If AlternativeDistance < connection.GetEndNode().GetShortestFromSource Then 

                                connection.GetEndNode().SetShortestFromsource(AlternativeDistance) 

                                connection.GetEndNode().SetPreviousNode(CurrentNode) 

                            End If 

                        End If 

                    Next 

                    CurrentNode.SetIsProcessed(True) 

                    CurrentNode.DrawGreenHighlightedNode() 

 

                    For Each connection As Connection In CurrentNode.GetConnections() 

                        connection.DrawLine() 

                    Next 

 

                    NextNearestNode = EmptyNode 

 

                    ' Get the next nearest node - the node which has the shortest distance from the source node which hasn't been processed 

                    For Each node As Node In ConnectedNodes 

                        If node.IsProcessed = False Then 

                            If node.GetShortestFromSource < NextNearestNode.GetShortestFromSource Then 

                                NextNearestNode = node 

                            End If 

                        End If 

                    Next 

                    CurrentNode = NextNearestNode 

 

                    ' checks if all connected nodes have been processed 

                    AllNodesProcessed = True 

                    For Each node As Node In ConnectedNodes 

                        If node.IsProcessed = False Then 

                            AllNodesProcessed = False 

                        End If 

 

                    Next 

 

                Loop Until AllNodesProcessed = True 

 

 

                Dim ANode As Node 

                ANode = TargetNode 

 

                ' gets the path in reverse order by working bacwards from the source node 

                Do While ANode.GetName <> SourceNode.GetName 

 

                    TheStack.Push(ANode) 

 

                    ANode = ANode.GetPreviousNode 

 

                Loop 

 

                'gets the path in order - from the source node by popping from the stack 

                Solution = SourceNode.GetName 

                Do 

                    Dim PoppedNode As Node 

                    PoppedNode = TheStack.Pop 

                    Solution = Solution + " " + PoppedNode.GetName 

                Loop Until TheStack.IsEmpty = True 

 

                Dim Path As String 

                Path = Solution.Replace(" ", "") 

 

                For Each Node As Node In TheGraph.GetGraphNodes() 

                    Node.DrawNormalNode() 

                    For Each Connection As Connection In Node.GetConnections() 

                        Connection.RemoveLine() 

                        Connection.DrawLine() 

                    Next 

                Next 

 

                'highlights the path found 

                For looper = 0 To Len(Path) - 1 

 

                    If looper = Len(Path) - 1 Then 

 

                    Else 

                        Dim StartNode As Node 

                        StartNode = TheGraph.GetNodeFromName(Path(looper)) 

                        Dim EndNode As Node 

                        EndNode = TheGraph.GetNodeFromName(Path(looper + 1)) 

                        StartNode.GetConnections().Find(Function(x) x.GetEndNode().GetName = EndNode.GetName()).DrawHighlightedLine(StartNode, EndNode) 

                    End If 

                Next 

 

                ShortestPath = TargetNode.GetShortestFromSource 

                MsgBox("Shortest path found: (" & Solution & ") with a distance of " & Str(ShortestPath)) 

            End If 

        End If 

 

    End Sub 

 

    Private Sub AddNodeButton_Click(sender As Object, e As EventArgs) Handles AddNodeButton.Click 

        Dim AddedNodeName As String 

        AddedNodeName = UCase(InputBox("Enter the letter of the node you want added:")) 

        TheGraph.AddNode(AddedNodeName) 

        FillAdjacencyMatrix() 

        FillAdjacencyList() 

 

    End Sub 

 

    Private Sub SetSourceNodeButton_Click(sender As Object, e As EventArgs) Handles SetSourceNodeButton.Click 

        Dim SourceNodeName As String 

        Dim IsInGraph As Boolean = False 

        Dim NewSourceNode As Node 

        SourceNodeName = InputBox("Enter the letter of the source node: ") 

        If SourceNodeName = "" Then 

 

        Else 

            'checks if node in in the graph 

            For Each node As Node In TheGraph.GetGraphNodes() 

                If node.GetName = SourceNodeName Then 

                    NewSourceNode = node 

                    IsInGraph = True 

                End If 

            Next 

            If IsInGraph = False Then 

                MsgBox("This node is not in the graph") 

            ElseIf IsNothing(TargetNode) = False Then 

                If SourceNodeName = TargetNode.GetName Then 

                    MsgBox("Source Node cannot be the target node") 

                Else 

                    SourceNode = NewSourceNode 

                    SourceNodeLabel.Text = "Source Node: " & SourceNode.GetName() 

                End If 

            Else 

                ' if there is no source node, it does not check if the source node is also the target node 

                SourceNode = NewSourceNode 

                SourceNodeLabel.Text = "Source Node: " & SourceNode.GetName() 

            End If 

        End If 

 

    End Sub 

 

    Private Sub SetTargetNodeButton_Click(sender As Object, e As EventArgs) Handles SetTargetNodeButton.Click 

        Dim TargetNodeName As String 

        Dim IsInGraph As Boolean = False 

        Dim NewTargetNode As Node 

        TargetNodeName = InputBox("Enter the letter of the target node: ") 

        If TargetNodeName = "" Then 

 

        Else 

            'checks if node in in the graph 

            For Each node As Node In TheGraph.GetGraphNodes() 

                If node.GetName = TargetNodeName Then 

                    NewTargetNode = node 

                    IsInGraph = True 

                End If 

            Next 

            If IsInGraph = False Then 

                MsgBox("This node is not in the graph") 

            ElseIf IsNothing(SourceNode) = False Then 

                If TargetNodeName = SourceNode.GetName Then 

                    MsgBox("Target Node cannot be the Source Node") 

                Else 

                    Me.TargetNode = NewTargetNode 

                    TargetNodeLabel.Text = "Target Node: " & TargetNode.GetName() 

                End If 

 

            Else 

                ' if there is no source node, it does not check if the target node is also the source node 

                Me.TargetNode = NewTargetNode 

                TargetNodeLabel.Text = "Target Node: " & TargetNode.GetName() 

            End If 

        End If 

 

 

    End Sub 

 

    Private Sub RemoveNodeButton_Click(sender As Object, e As EventArgs) Handles RemoveNodeButton.Click 

        Dim RemovedNodeName As String 

        RemovedNodeName = InputBox("Enter the letter of the node that you would like to be removed: ") 

        TheGraph.RemoveNode(RemovedNodeName) 

        FillAdjacencyMatrix() 

        FillAdjacencyList() 

 

    End Sub 

    Private Sub SwitchModeButton_Click(sender As Object, e As EventArgs) Handles SwitchModeButton.Click 

 

        If PixelMode = True Then 

            PixelMode = False 

        Else 

            PixelMode = True 

        End If 

        'redraws each connection based in the node 

        For Each node As Node In TheGraph.GetGraphNodes() 

            For Each connection As Connection In node.GetConnections 

                connection.RemoveLine() 

                connection.SetPixelMode(PixelMode) 

                connection.DrawLine() 

            Next 

        Next 

        FillAdjacencyMatrix() 

        FillAdjacencyList() 

    End Sub 

 

    Private Sub ClearGraphButton_Click(sender As Object, e As EventArgs) Handles ClearGraphButton.Click 

        TheGraph.ClearGraph() 

 

    End Sub 

 

    Private Sub Adjacencymatrix_SelectionChanged(sender As Object, e As EventArgs) Handles Adjacencymatrix.SelectionChanged 

        Adjacencymatrix.ClearSelection() 

 

    End Sub 

    Private Sub AdjacencyList_SelectionChanged(Sender As Object, e As EventArgs) Handles AdjacencyList.SelectionChanged 

        AdjacencyList.ClearSelection() 

    End Sub 

 

    Private Sub LoadInitialGraphButton_Click(sender As Object, e As EventArgs) Handles LoadInitialGraphButton.Click 

        TheGraph.ClearGraph() 

        TheGraph.SetInitialGraph() 

        SourceNode = TheGraph.GetGraphNodes()(0) 

        TargetNode = TheGraph.GetGraphNodes()(6) 

        SourceNodeLabel.Text = "Source Node: " & SourceNode.GetName() 

        TargetNodeLabel.Text = "Target Node: " & TargetNode.GetName() 

        FillAdjacencyMatrix() 

        FillAdjacencyList() 

    End Sub 

 

    Private Sub SaveCurrentGraphButton_Click(sender As Object, e As EventArgs) Handles SaveCurrentGraphButton.Click 

        SavedGraphData.ClearGraph() 

 

        For Each node As Node In TheGraph.GetGraphNodes() 

            'fills the nodeData class with the current nodes and adds it to the GraphData class 

            Dim nodeData As New NodeData 

            nodeData.SetName(node.GetName) 

            nodeData.AddLocation(node.Location) 

 

            'adds connection data for each nodeData object 

            For Each connection As Connection In node.GetConnections() 

                Dim connectiondata As New ConnectionData 

                connectiondata.SetStartNodeLetter(connection.GetStartNode().GetName()) 

                connectiondata.SetEndNodeLetter(connection.GetEndNode().GetName()) 

                connectiondata.SetEdgeNumber(connection.GetEdgeNumber()) 

                connectiondata.SetPreviousEdgeNumber(connection.GetPreviousEdgeNumber()) 

                connectiondata.SetPixelMode(connection.GetPixelMode()) 

                nodeData.AddConnection(connectiondata) 

            Next 

            SavedGraphData.AddNode(nodeData) 

        Next 

 

        'saves the source and target node with the graph data 

        SavedGraphData.SetSourceNodeLetter(SourceNode.GetName()) 

        SavedGraphData.SetTargetNodeLetter(TargetNode.GetName()) 

 

        ' Create an instance of the XmlSerializer class and specify the type of the object you want to serialize 

        Dim serializer As New XmlSerializer(GetType(GraphData)) 

 

        ' Open a file stream to write the serialized data to 

        Using stream As New FileStream("graphdata.xml", FileMode.Create) 

            ' Serialize the data and write it to the stream 

            serializer.Serialize(stream, SavedGraphData) 

        End Using 

 

        ' The data has now been serialized and written to the graphdata.xml file 

 

    End Sub 

 

    Private Sub LoadSavedGraphButton_Click(sender As Object, e As EventArgs) Handles LoadSavedGraphButton.Click 

 

 

        ' Create an instance of the XmlSerializer class and specify the type of the object you want to deserialize 

        Dim serializer As New XmlSerializer(GetType(GraphData)) 

 

        ' Open a file stream to read the serialized data from 

        Using stream As New FileStream("graphdata.xml", FileMode.Open) 

            ' Deserialize the data and cast it to the GraphData type 

            Dim SavedGraphData As GraphData = DirectCast(serializer.Deserialize(stream), GraphData) 

        End Using 

 

        If SavedGraphData.GetGraphNodes().Count = 0 Then 

            MsgBox("There is no saved graph") 

 

        Else 

            TheGraph.ClearGraph() 

            'adds saved nodes to current graph 

            For Each nodeData As NodeData In SavedGraphData.GetGraphNodes() 

                Dim node As New Node(True) 

                node.SetName(nodeData.GetName) 

                node.Location = nodeData.GetLocation() 

                TheGraph.AddSpecificNode(node) 

            Next 

 

            For Each nodeData As NodeData In SavedGraphData.GetGraphNodes() 

                Dim node As New Node(False) 

 

                'gets connection data from each NodeData object 

                For Each connectionData As ConnectionData In nodeData.GetConnections() 

                    Dim connection As New Connection 

                    connection.SetStartNode(TheGraph.GetNodeFromName(connectionData.GetStartNodeLetter())) 

                    connection.SetEndNode(TheGraph.GetNodeFromName(connectionData.GetEndNodeLetter())) 

                    connection.SetEdgeNumber(connectionData.GetEdgeNumber()) 

                    connection.SetPreviousEdgeNumber(connectionData.GetPreviousEdgeNumber()) 

                    connection.SetPixelMode(connectionData.GetPixelMode()) 

                    node.AddSpecificConnection(connection) 

                Next 

 

                'adds the connection data to the node in the current graph who has the same name as the NodeData object 

                For Each connection As Connection In node.GetConnections() 

                    TheGraph.GetNodeFromName(nodeData.GetName()).AddSpecificConnection(connection) 

                Next 

            Next 

 

            SourceNode = TheGraph.GetNodeFromName(SavedGraphData.GetSourceNodeLetter) 

            TargetNode = TheGraph.GetNodeFromName(SavedGraphData.GetTargetNodeLetter) 

            SourceNodeLabel.Text = "Source Node: " & SourceNode.GetName() 

            TargetNodeLabel.Text = "Target Node: " & TargetNode.GetName() 

 

            For Each node As Node In TheGraph.GetGraphNodes() 

                For Each connection As Connection In node.GetConnections() 

                    connection.DrawLine() 

                Next 

            Next 

            FillAdjacencyMatrix() 

            FillAdjacencyList() 

        End If 

 

 

    End Sub 

 

    Private Sub TravellingSalesmanButton_Click(sender As Object, e As EventArgs) Handles TravellingSalesmanButton.Click 

        If PixelMode = False Then 

            MsgBox("The graph must be fully connected and in pixel mode") 

        Else 

            TheGraph.FullyConnectNodes() 

            If TheGraph.GetGraphNodes().Count < 6 Then 

                'find all possible combinations if No of nodes < 6 

                BruteForceTSP.FindBestPath(TheGraph) 

                Dim BestPath As List(Of Node) = New List(Of Node)(BruteForceTSP.GetBestPath()) 

                Dim path As String 

                Dim Shortestdistance As Integer = BruteForceTSP.GetBestDistance() 

 

                For Each Node As Node In TheGraph.GetGraphNodes() 

                    Node.DrawNormalNode() 

                    For Each Connection As Connection In Node.GetConnections() 

                        Connection.DrawLine() 

                    Next 

                Next 

 

                For Each node As Node In BestPath 

                    path &= node.GetName() & " " 

                Next 

 

                Dim _path 

                _path = path.Replace(" ", "") 

                For looper = 0 To Len(_path) - 1 

 

                    If looper = Len(_path) - 1 Then 

 

                    Else 

                        Dim StartNode As Node 

                        StartNode = TheGraph.GetNodeFromName(_path(looper)) 

                        Dim EndNode As Node 

                        EndNode = TheGraph.GetNodeFromName(_path(looper + 1)) 

                        StartNode.GetConnections().Find(Function(x) x.GetEndNode().GetName = EndNode.GetName()).DrawHighlightedLine(StartNode, EndNode) 

                    End If 

                Next 

 

                MsgBox("Shortest path found: (" & path & ") with a distance of " & Str(Shortestdistance)) 

            Else 

                'use the nearest neighbours heuristic if No of nodes > 5 

                NNeighbours.FindBestPath(TheGraph) 

                Dim BestPath As List(Of Node) = New List(Of Node)(NNeighbours.GetBestPath()) 

                Dim path As String 

                Dim Shortestdistance As Integer = NNeighbours.GetBestDistance() 

 

                For Each Node As Node In TheGraph.GetGraphNodes() 

                    Node.DrawNormalNode() 

                    For Each Connection As Connection In Node.GetConnections() 

                        Connection.DrawLine() 

                    Next 

                Next 

 

                For Each node As Node In BestPath 

                    path &= node.GetName() & " " 

                Next 

 

                Dim _path 

                _path = path.Replace(" ", "") 

                For looper = 0 To Len(_path) - 1 

 

                    If looper = Len(_path) - 1 Then 

 

                    Else 

                        Dim StartNode As Node 

                        StartNode = TheGraph.GetNodeFromName(_path(looper)) 

                        Dim EndNode As Node 

                        EndNode = TheGraph.GetNodeFromName(_path(looper + 1)) 

                        StartNode.GetConnections().Find(Function(x) x.GetEndNode().GetName = EndNode.GetName()).DrawHighlightedLine(StartNode, EndNode) 

                    End If 

                Next 

 

                MsgBox("The approximate shortest path found: (" & path & ") with a distance of " & Str(Shortestdistance)) 

            End If 

 

 

        End If 

 

 

 

 

 

    End Sub 

 

    Private Sub FullyConnectButton_Click(sender As Object, e As EventArgs) Handles FullyConnectButton.Click 

        If PixelMode = False Then 

            MsgBox("The graph must be in pixel mode") 

        Else 

            TheGraph.FullyConnectNodes() 

        End If 

    End Sub 

End Class 