Public Class NearestNeighbours 

    Private CurrentGraph As Graph 

    Private BestPath As List(Of Node) 

    Private BestDistance As Integer 

 

    Public Sub New() 

 

    End Sub 

 

    Public Sub FindBestPath(CurrentGraph As Graph) 

        Me.CurrentGraph = CurrentGraph 

        BestDistance = Integer.MaxValue 

 

        For Each StartNode As Node In CurrentGraph.GetGraphNodes() 

            'checks a path from each node in the graph 

 

            Dim CurrentNode As Node = StartNode 

            Dim VisitedNodes As New List(Of Node) 

            Dim distance As Integer = 0 

 

            'resets graph 

            For Each Node As Node In CurrentGraph.GetGraphNodes() 

                Node.DrawNormalNode() 

                For Each Connection As Connection In Node.GetConnections() 

                    Connection.DrawLine() 

                Next 

            Next 

 

            StartNode.DrawRedHighlightedNode() 

 

 

            While VisitedNodes.Count < CurrentGraph.GetGraphNodes().Count 

                VisitedNodes.Add(CurrentNode) 

                ' repeats until it reaches the end of the path 

 

 

                Dim NearestNode As Node = Nothing 

                Dim NearestDistance As Integer = Integer.MaxValue 

                Dim NearestNodeConnection As Connection 

 

 

                For Each node As Node In CurrentGraph.GetGraphNodes() 

                    If VisitedNodes.Contains(node) Then 

                        Continue For 

                    End If 

 

 

                    Dim connection As Connection = CurrentNode.GetConnections().Find(Function(x) x.GetEndNode.GetName() = node.GetName()) 

 

                    connection.DrawRedHighlightedLine() 

                    node.DrawRedHighlightedNode() 

                    Threading.Thread.Sleep(100 * Form1.SleepTrackBar.Value) 

                    connection.DrawLine() 

                    node.DrawNormalNode() 

 

                    ' it checks the distance to each node from the current node and records the closest node 

                    If connection.GetEdgeNumber < NearestDistance Then 

                        NearestNode = node 

                        NearestNodeConnection = connection 

                        NearestDistance = connection.GetEdgeNumber() 

 

                    End If 

                Next 

 

 

                If VisitedNodes.Count = CurrentGraph.GetGraphNodes().Count Then 

                    Dim connection As Connection = CurrentNode.GetConnections().Find(Function(x) x.GetEndNode.GetName = StartNode.GetName) 

                    NearestDistance = connection.GetEdgeNumber() 

                    NearestNode = StartNode 

                End If 

 

                'current node goes to the next nearest node and repeats the process 

                distance += NearestDistance 

                CurrentNode = NearestNode 

 

                CurrentNode.DrawRedHighlightedNode() 

                NearestNodeConnection.DrawRedHighlightedLine() 

 

 

            End While 

 

            'if a path found has the shortest distance it is recorded 

            If distance < BestDistance Then 

                BestDistance = distance 

                BestPath = VisitedNodes 

                HiglightBestPath(VisitedNodes, CurrentNode) 

                BestPath.Add(StartNode) 

            Else 

                HiglightPathFound(VisitedNodes, CurrentNode) 

            End If 

        Next 

    End Sub 

    Private Sub HiglightBestPath(Path As List(Of Node), StartNode As Node) 

        For i = 0 To Path.Count - 2 

            Path(i).DrawGreenHighlightedNode() 

            Path(i).GetConnections.Find(Function(x) x.GetEndNode.GetName() = Path(i + 1).GetName()).DrawGreenHighlightedLine() 

        Next 

        Path(Path.Count - 1).DrawGreenHighlightedNode() 

        Path(Path.Count - 1).GetConnections.Find(Function(x) x.GetEndNode.GetName() = StartNode.GetName()).DrawGreenHighlightedLine() 

        Threading.Thread.Sleep(200 * Form1.SleepTrackBar.Value) 

 

 

    End Sub 

    Private Sub HiglightPathFound(Path As List(Of Node), StartNode As Node) 

        For i = 0 To Path.Count - 2 

            Path(i).DrawOrangeHighlightedNode() 

            Path(i).GetConnections.Find(Function(x) x.GetEndNode.GetName() = Path(i + 1).GetName()).DrawOrangeHighlightedLine() 

        Next 

        Path(Path.Count - 1).DrawOrangeHighlightedNode() 

        Path(Path.Count - 1).GetConnections.Find(Function(x) x.GetEndNode.GetName() = StartNode.GetName()).DrawOrangeHighlightedLine() 

        Threading.Thread.Sleep(200 * Form1.SleepTrackBar.Value) 

 

 

    End Sub 

    Public Function GetBestPath() As List(Of Node) 

        Return Me.BestPath 

    End Function 

    Public Function GetBestDistance() As Integer 

        Return Me.BestDistance 

    End Function 

End Class 