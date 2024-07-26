Public Class Queue 

    Protected Nodes() As Node 

    Protected ThePointer As Integer 

    Protected EndPointer As Integer 

 

    Public Sub New() 

        ReDim Nodes(0) 

        Me.ThePointer = 0 

        Me.EndPointer = -1 

    End Sub 

    Public Sub Push(Node As Node) 

        Dim PushedNode As Node 

        PushedNode = Node 

        ReDim Preserve Nodes(EndPointer + 1) 

        Me.Nodes(EndPointer + 1) = PushedNode 

        Me.EndPointer += 1 

 

    End Sub 

    Public Function Pop() 

        Dim PoppedNode As Node 

        If Me.IsEmpty = True Then 

            Return IsEmpty() 

        Else 

            PoppedNode = Nodes(ThePointer) 

            ShuffleForwards() 

            Return PoppedNode 

        End If 

    End Function 

    Public Function IsEmpty() As Boolean 

        If EndPointer = -1 Then 

            Return True 

        Else 

            Return False 

        End If 

    End Function 

    Sub ShuffleForwards() 

        For looper = ThePointer To EndPointer - 1 

            Nodes(looper) = Nodes(looper + 1) 

        Next 

        EndPointer -= 1 

    End Sub 

End Class 