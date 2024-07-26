Public Class TSPBruteForce 

    Private CurrentGraph As Graph 

    Private BestPath As New List(Of Node) 

    Private BestDistance As Integer 

    Dim StartNode As Node 

 

    Public Sub New() 

 

    End Sub 

 

    Public Sub FindBestPath(CurrentGraph As Graph) 

        BestDistance = Integer.MaxValue 

 

        Me.CurrentGraph = CurrentGraph 

 

        'resets graph 

        For Each Node As Node In CurrentGraph.GetGraphNodes() 

            Node.DrawNormalNode() 

            For Each Connection As Connection In Node.GetConnections() 

                Connection.DrawLine() 

            Next 

        Next 

 

        ' starts search from the first node in the graph 

        StartNode = CurrentGraph.GetGraphNodes(0) 

        Dim UnvisitedNodes As List(Of Node) = New List(Of Node)(CurrentGraph.GetGraphNodes) 

        UnvisitedNodes.Remove(StartNode) 

        FindPath(StartNode, UnvisitedNodes, 0, New List(Of Node)) 

        BestPath.Add(StartNode) 

    End Sub 

 

    Private Sub FindPath(CurrentNode As Node, UnvisitedNodes As List(Of Node), distance As Integer, Path As List(Of Node)) 

        Path.Add(CurrentNode) 

        CurrentNode.DrawRedHighlightedNode() 

        Threading.Thread.Sleep(50 * Form1.SleepTrackBar.Value) 

 

 

 

        If UnvisitedNodes.Count = 0 Then 

            'base case 

            ' returns to where it was called to explore other possible paths after 

            distance += CurrentNode.GetConnections().Find(Function(x) x.GetEndNode.GetName() = CurrentGraph.GetGraphNodes(0).GetName()).GetEdgeNumber() 

 

            ' checks if the distance travelled is less than the current best distance 

            ' if so it saves the path as the best with the distance 

            If distance < BestDistance Then 

 

                BestDistance = distance 

                BestPath = New List(Of Node)(Path) 

                HiglightBestPath(BestPath) 

            Else 

                HiglightPathFound(Path) 

            End If 

        Else 

            For Each nextNode As Node In UnvisitedNodes 

                Dim NewUnvisitedNodes = New List(Of Node)(UnvisitedNodes) 

                NewUnvisitedNodes.Remove(nextNode) 

                Dim edge = CurrentNode.GetConnections.Find(Function(x) x.GetEndNode.GetName() = nextNode.GetName()) 

                edge.DrawRedHighlightedLine() 

                Threading.Thread.Sleep(100 * Form1.SleepTrackBar.Value) 

 

                'follows a possible path from the current node 

                'will do this until it reaches the end of the path which is the base case 

                FindPath(nextNode, NewUnvisitedNodes, distance + edge.GetEdgeNumber(), New List(Of Node)(Path)) 

                HiglightCurrentPath(Path) 

                CurrentNode.DrawRedHighlightedNode() 

            Next 

        End If 

    End Sub 

    Private Sub HiglightBestPath(Path As List(Of Node)) 

        For i = 0 To Path.Count - 2 

            Path(i).DrawGreenHighlightedNode() 

            Path(i).GetConnections.Find(Function(x) x.GetEndNode.GetName() = Path(i + 1).GetName()).DrawGreenHighlightedLine() 

        Next 

        Path(Path.Count - 1).DrawGreenHighlightedNode() 

        Path(Path.Count - 1).GetConnections.Find(Function(x) x.GetEndNode.GetName() = StartNode.GetName()).DrawGreenHighlightedLine() 

        Threading.Thread.Sleep(200 * Form1.SleepTrackBar.Value) 

 

 

    End Sub 

    Private Sub HiglightPathFound(Path As List(Of Node)) 

        For i = 0 To Path.Count - 2 

            Path(i).DrawOrangeHighlightedNode() 

            Path(i).GetConnections.Find(Function(x) x.GetEndNode.GetName() = Path(i + 1).GetName()).DrawOrangeHighlightedLine() 

        Next 

        Path(Path.Count - 1).DrawOrangeHighlightedNode() 

        Path(Path.Count - 1).GetConnections.Find(Function(x) x.GetEndNode.GetName() = StartNode.GetName()).DrawOrangeHighlightedLine() 

        Threading.Thread.Sleep(200 * Form1.SleepTrackBar.Value) 

 

 

    End Sub 

    Private Sub HiglightCurrentPath(Path As List(Of Node)) 

        For Each Node As Node In CurrentGraph.GetGraphNodes() 

            Node.DrawNormalNode() 

            For Each Connection As Connection In Node.GetConnections() 

                Connection.DrawLine() 

            Next 

        Next 

        For i = 0 To Path.Count - 2 

            Path(i).DrawRedHighlightedNode() 

            Path(i).GetConnections.Find(Function(x) x.GetEndNode.GetName() = Path(i + 1).GetName()).DrawRedHighlightedLine() 

        Next 

    End Sub 

 

    Public Function GetBestPath() As List(Of Node) 

        Return Me.BestPath 

    End Function 

    Public Function GetBestDistance() As Integer 

        Return Me.BestDistance 

    End Function 

End Class 