<Serializable> 
Public Class NodeData 

    Private Name As String 

    Private Connections As New List(Of ConnectionData) 

    Private LocationOnGraph As Point 

 

    Public Sub SetName(Name As String) 

        Me.Name = Name 

    End Sub 

    Public Sub AddConnection(Connection As ConnectionData) 

        Me.Connections.Add(Connection) 

    End Sub 

    Public Sub AddLocation(MyLocation As Point) 

        Me.LocationOnGraph = MyLocation 

    End Sub 

 

    Public Function GetName() As String 

        Return Me.Name 

    End Function 

    Public Function GetConnections() As List(Of ConnectionData) 

        Return Me.Connections 

    End Function 

    Public Function GetLocation() As Point 

        Return Me.LocationOnGraph 

    End Function 

End Class