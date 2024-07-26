<Serializable> 
Public Class GraphData 

    Private Nodes As New List(Of NodeData) 

    Private SourceNodeLetter As String 

    Private TargetNodeLetter As String 

    Public Sub AddNode(AddedNode As NodeData) 

        Me.Nodes.Add(AddedNode) 

    End Sub 

    Public Sub SetSourceNodeLetter(SourceNodeLetter As String) 

        Me.SourceNodeLetter = SourceNodeLetter 

    End Sub 

    Public Sub SetTargetNodeLetter(TargetNodeLetter As String) 

        Me.TargetNodeLetter = TargetNodeLetter 

    End Sub 

    Public Function GetGraphNodes() As List(Of NodeData) 

        Return Me.Nodes 

    End Function 

    Public Sub ClearGraph() 

        Me.Nodes.Clear() 

    End Sub 

    Public Function GetSourceNodeLetter() As String 

        Return Me.SourceNodeLetter 

    End Function 

    Public Function GetTargetNodeLetter() As String 

        Return Me.TargetNodeLetter 

    End Function 

End Class 