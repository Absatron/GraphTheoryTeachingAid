Public Class Stack 

    Protected Nodes() As Node 

    Protected ThePointer As Integer 

    Public Sub New() 

        ReDim Nodes(0) 

        Me.ThePointer = -1 

    End Sub 

 

    Public Sub Push(Node As Node) 

        Dim PushedNode As Node 

        PushedNode = Node 

        ReDim Preserve Nodes(ThePointer + 1) 

        Nodes(ThePointer + 1) = PushedNode 

        Me.ThePointer += 1 

    End Sub 

 

    Function Pop() 

        Dim PoppedNode As Node 

        If IsEmpty() = True Then 

            Return IsEmpty() 

        Else 

            PoppedNode = Nodes(ThePointer) 

            ThePointer -= 1 

            ReDim Preserve Nodes(ThePointer) 

            Return PoppedNode 

        End If 

    End Function 

 

    Function IsEmpty() As Boolean 

        If ThePointer = -1 Then 

            Return True 

        Else 

            Return False 

        End If 

    End Function 

End Class 