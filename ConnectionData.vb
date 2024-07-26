<Serializable> 
Public Class ConnectionData 

    Public StartNodeLetter As String 

    Public EndNodeLetter As String 

    Public EdgeNumber As Integer 

    Public PreviousEdgeNumber As Integer 

    Public PixelMode As Boolean 

 

    Public Sub SetStartNodeLetter(StartNodeLetter As String) 

        Me.StartNodeLetter = StartNodeLetter 

    End Sub 

    Public Sub SetEndNodeLetter(EndNodeLetter As String) 

        Me.EndNodeLetter = EndNodeLetter 

    End Sub 

    Public Sub SetEdgeNumber(EdgeNumber As Integer) 

        Me.EdgeNumber = EdgeNumber 

    End Sub 

    Public Sub SetPreviousEdgeNumber(PreviousEdgeNumber As Integer) 

        Me.PreviousEdgeNumber = PreviousEdgeNumber 

    End Sub 

    Public Sub SetPixelMode(PixelMode As Boolean) 

        Me.PixelMode = PixelMode 

    End Sub 

    Public Function GetStartNodeLetter() As String 

        Return Me.StartNodeLetter 

    End Function 

    Public Function GetEndNodeLetter() As String 

        Return Me.EndNodeLetter 

    End Function 

    Public Function GetEdgeNumber() As Integer 

        Return Me.EdgeNumber 

    End Function 

    Public Function GetPreviousEdgeNumber() As Integer 

        Return Me.PreviousEdgeNumber 

    End Function 

    Public Function GetPixelMode() As Boolean 

        Return Me.PixelMode 

    End Function 

End Class 