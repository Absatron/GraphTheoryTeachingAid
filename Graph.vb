Public Class Graph 

    Private Nodes As New List(Of Node) 

 

    Public Sub New() 

        SetInitialGraph() 

    End Sub 

 

    Public Sub SetInitialGraph() 

        Dim NodeA As New Node(True) 

        Dim NodeB As New Node(True) 

        Dim NodeC As New Node(True) 

        Dim NodeD As New Node(True) 

        Dim NodeE As New Node(True) 

        Dim NodeF As New Node(True) 

        Dim NodeG As New Node(True) 

 

        NodeA.SetName("A") 

        NodeA.SetConnection(NodeB, 3) 

        NodeA.SetConnection(NodeD, 6) 

        NodeA.SetConnection(NodeC, 5) 

        NodeA.Location = New Point(100, 100) 

 

        NodeB.SetName("B") 

        NodeB.SetConnection(NodeA, 3) 

        NodeB.SetConnection(NodeD, 2) 

        NodeB.Location = New Point(100, 300) 

 

        NodeC.SetName("C") 

        NodeC.SetConnection(NodeA, 5) 

        NodeC.SetConnection(NodeD, 2) 

        NodeC.SetConnection(NodeF, 3) 

        NodeC.SetConnection(NodeG, 7) 

        NodeC.SetConnection(NodeE, 6) 

        NodeC.Location = New Point(300, 100) 

 

        NodeD.SetName("D") 

        NodeD.SetConnection(NodeB, 2) 

        NodeD.SetConnection(NodeA, 6) 

        NodeD.SetConnection(NodeC, 2) 

        NodeD.SetConnection(NodeF, 9) 

        NodeD.Location = New Point(200, 300) 

 

        NodeE.SetName("E") 

        NodeE.SetConnection(NodeC, 6) 

        NodeE.SetConnection(NodeF, 5) 

        NodeE.SetConnection(NodeG, 2) 

        NodeE.Location = New Point(600, 100) 

 

        NodeF.SetName("F") 

        NodeF.SetConnection(NodeD, 9) 

        NodeF.SetConnection(NodeC, 3) 

        NodeF.SetConnection(NodeE, 5) 

        NodeF.SetConnection(NodeG, 1) 

        NodeF.Location = New Point(450, 300) 

 

        NodeG.SetName("G") 

        NodeG.SetConnection(NodeF, 1) 

        NodeG.SetConnection(NodeE, 2) 

        NodeG.SetConnection(NodeC, 7) 

        NodeG.Location = New Point(600, 300) 

 

        Me.Nodes.Add(NodeA) 

        Me.Nodes.Add(NodeB) 

        Me.Nodes.Add(NodeC) 

        Me.Nodes.Add(NodeD) 

        Me.Nodes.Add(NodeE) 

        Me.Nodes.Add(NodeF) 

        Me.Nodes.Add(NodeG) 

 

        For Each node As Node In Me.Nodes 

            For Each connection As Connection In node.GetConnections 

                connection.DrawLine() 

            Next 

        Next 

 

    End Sub 

    Public Function GetNodeFromName(NodeName As String) As Node 

        For Each node As Node In Me.Nodes 

            If node.GetName() = NodeName Then 

                Return node 

            End If 

        Next 

    End Function 

    Public Function LoadMenu() As List(Of ToolStripMenuItem) 

        Dim NodeNames As New List(Of ToolStripMenuItem) 

        ' gets the nodes of this graph to be displayed in the frop down menu 

        For Each Node As Node In Me.Nodes 

            Dim NodeName As New ToolStripMenuItem 

            NodeName.Text = "Node: " & Node.GetName() 

            NodeName.CheckOnClick = True 

            NodeNames.Add(NodeName) 

        Next 

        Return NodeNames 

    End Function 

    Public Function GetGraphNodes() As List(Of Node) 

        Return Me.Nodes 

    End Function 

    Public Sub ClearGraph() 

        'removes all nodes 

        For Each node As Node In Me.Nodes 

            For Each connection As Connection In node.GetConnections() 

                connection.RemoveLine() 

            Next 

            Form1.Controls.Remove(node) 

        Next 

        Me.Nodes.Clear() 

 

        'updates the source and target nodes 

        Form1.SourceNodeLabel.Text = "undefined" 

        Form1.SetSourceNodeWhenRemoved() 

        Form1.TargetNodeLabel.Text = "undefined" 

        Form1.SetTargetNodeWhenRemoved() 

        Form1.FillAdjacencyMatrix() 

        Form1.FillAdjacencyList() 

    End Sub 

    Public Sub UpdateAddedConnection(StartNode As Node, EdgeNumber As Integer, EndNode As Node) 

        'used by the node add connections both ways 

        StartNode.SetConnection(EndNode, EdgeNumber) 

        StartNode.GetConnections(StartNode.GetConnections.Count - 1).SetPixelMode(Form1.GetPixelMode) 

        StartNode.GetConnections(StartNode.GetConnections.Count - 1).DrawLine() 

    End Sub 

    Public Sub UpdateRemovedConnection(NodeUnconnected As Node, UpdatedNode As Node) 

        'used by the node remove connections both ways 

        Dim RemovedConnection As Connection 

        For Each connection As Connection In NodeUnconnected.GetConnections() 

            If connection.GetEndNode().GetName = UpdatedNode.GetName() Then 

                RemovedConnection = connection 

                connection.RemoveLine() 

            End If 

        Next 

        NodeUnconnected.GetConnections().Remove(RemovedConnection) 

    End Sub 

    Public Sub AddNode(AddedNodeName) 

 

        If Me.Nodes.Count = 26 Then 

            MsgBox("The graph is full") 

        ElseIf AddedNodeName = "" Then 

 

        ElseIf Asc(AddedNodeName) < 65 Or Asc(AddedNodeName) > 90 Then 'only capital letters 

            MsgBox("This node name is invalid") 

        Else 

 

 

            Dim InUse As Boolean = False 

            'checks if the node is in use 

            For Each node As Node In Me.Nodes 

                If node.GetName = AddedNodeName Then 

                    InUse = True 

                End If 

            Next 

 

            If InUse = True Then 

                MsgBox("This node is in use") 

            Else 

                Dim NewNode As New Node(True) 

                NewNode.SetName(AddedNodeName) 

                Me.Nodes.Add(NewNode) 

            End If 

        End If 

 

 

 

    End Sub 

    Public Sub AddSpecificNode(AddedNode As Node) 

        Me.Nodes.Add(AddedNode) 

    End Sub 

 

    Public Sub RemoveNode(RemovedNodeName As String) 

        If RemovedNodeName = "" Then 

 

        Else 

            Dim IsInGraph As Boolean = False 

            Dim RemovedNode As Node 

            'checks if node is in the graph 

            For Each node As Node In Me.Nodes 

                If node.GetName = RemovedNodeName Then 

                    IsInGraph = True 

                    RemovedNode = node 

                End If 

            Next 

            If IsInGraph = False Then 

                MsgBox("This node is not in the graph") 

            Else 

                'updates source or target node if neccessary 

                If RemovedNode.GetName = Form1.SourceNodeLabel.Text.Last Then 

                    Form1.SourceNodeLabel.Text = "undefined" 

                    Form1.SetSourceNodeWhenRemoved() 

                ElseIf RemovedNode.GetName = Form1.TargetNodeLabel.Text.Last Then 

                    Form1.TargetNodeLabel.Text = "undefined" 

                    Form1.SetTargetNodeWhenRemoved() 

                End If 

                'removes the node as well as the connection 

                For Each connection As Connection In RemovedNode.GetConnections 

                    connection.RemoveLine() 

                Next 

                Me.Nodes.Remove(RemovedNode) 

                Form1.Controls.Remove(RemovedNode) 

 

                For Each Node As Node In Me.Nodes 

                    Node.RemoveRemovedNodeConnections(RemovedNode) 

                Next 

                For Each Node As Node In Me.GetGraphNodes() 

                    For Each Connection As Connection In Node.GetConnections() 

                        Connection.DrawLine() 

                    Next 

                Next 

            End If 

        End If 

 

    End Sub 

    Sub FullyConnectNodes() 

        For Each node As Node In Me.Nodes 

            For Each otherNode As Node In Me.Nodes 

                If node.GetName = otherNode.GetName Then 

 

                ElseIf IsNothing(node.GetConnections().Find(Function(x) x.GetEndNode().GetName = otherNode.GetName)) = True Then 

                    node.AddConnection(otherNode.GetName) 

                End If 

            Next 

        Next 

    End Sub 

End Class 