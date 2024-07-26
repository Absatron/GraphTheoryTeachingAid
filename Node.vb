Public Class Node 

    Inherits Panel 

 

    Private isDragging As Boolean = False 

    Private StartPoint As Point 

 

    Private Name As String 

    Private Connections As New List(Of Connection) 

    Private PreviousNode As Node 

    Private ShortestFromSource As Integer = 1000000 

    Private Processed As Boolean = False 

    Private Discovered As Boolean = False 

 

    Private IsGraphNode As Boolean 

 

    Private Menu As New ContextMenuStrip 

    Private MenuOptions As List(Of ToolStripMenuItem) 

 

    Dim CurrentGraph As Graph 

 

 

 

    Public Sub New(IsGraphNode) 

 

        Me.IsGraphNode = IsGraphNode 

 

        ' if node is in the graph then put it on the form 

        If IsGraphNode = True Then 

            Me.Width = 60 

            Me.Height = Me.Width 

            Me.Cursor = Cursors.Cross 

            Me.Location = New Point(20, 20) 

            Form1.Controls.Add(Me) 

        End If 

 

 

 

 

 

    End Sub 

 

    Protected Overrides Sub OnPaint(e As PaintEventArgs) 

        ' Get the graphics object for the panel 

        Dim g As Graphics = e.Graphics 

 

        ' Set the pen to use for drawing the border 

        Dim pen As New Pen(Color.Black, 4) 

 

        ' Fill the panel with a solid color 

        Dim brush As New SolidBrush(Color.AliceBlue) 

        g.FillEllipse(brush, 0, 0, Me.Width, Me.Height) 

 

        ' Draw the circular border around the panel 

        g.DrawEllipse(pen, 0, 0, Me.Width, Me.Height) 

 

        ' Set the font and color to use for the letter 

        Dim font As New Font("Tahoma", 30) 

        Dim brush2 As New SolidBrush(Color.Black) 

 

        ' Measure the size of the letter 

        Dim size As SizeF = g.MeasureString(Me.Name, font) 

 

        ' Calculate the position of the letter 

        Dim x As Single = (Me.Width - size.Width) / 2 

        Dim y As Single = (Me.Height - size.Height) / 2 

 

        ' Draw the letter in the middle of the panel 

        g.DrawString(Me.Name, font, brush2, x, y) 

 

        ' Call the base class's OnPaint method to paint the rest of the panel 

        MyBase.OnPaint(e) 

    End Sub 

    Public Sub DrawRedHighlightedNode() 

        Dim g As Graphics = Me.CreateGraphics 

 

        ' Set the pen to use for drawing the border 

        Dim pen As New Pen(Color.Black, 4) 

 

        Dim brush As New SolidBrush(Color.PaleVioletRed) 

        g.FillEllipse(brush, 0, 0, Me.Width, Me.Height) 

 

        ' Draw the circular border around the panel 

        g.DrawEllipse(Pen, 0, 0, Me.Width, Me.Height) 

 

        ' Set the font and color to use for the letter 

        Dim font As New Font("Tahoma", 30) 

        Dim brush2 As New SolidBrush(Color.Black) 

 

        ' Measure the size of the letter 

        Dim size As SizeF = g.MeasureString(Me.Name, font) 

 

        ' Calculate the position of the letter 

        Dim x As Single = (Me.Width - size.Width) / 2 

        Dim y As Single = (Me.Height - size.Height) / 2 

 

        ' Draw the letter in the middle of the panel 

        g.DrawString(Me.Name, font, brush2, x, y) 

    End Sub 

    Public Sub DrawGreenHighlightedNode() 

        Dim g As Graphics = Me.CreateGraphics 

 

        ' Set the pen to use for drawing the border 

        Dim pen As New Pen(Color.Black, 4) 

 

        Dim brush As New SolidBrush(Color.LightGreen) 

        g.FillEllipse(brush, 0, 0, Me.Width, Me.Height) 

 

        ' Draw the circular border around the panel 

        g.DrawEllipse(pen, 0, 0, Me.Width, Me.Height) 

 

        ' Set the font and color to use for the letter 

        Dim font As New Font("Tahoma", 30) 

        Dim brush2 As New SolidBrush(Color.Black) 

 

        ' Measure the size of the letter 

        Dim size As SizeF = g.MeasureString(Me.Name, font) 

 

        ' Calculate the position of the letter 

        Dim x As Single = (Me.Width - size.Width) / 2 

        Dim y As Single = (Me.Height - size.Height) / 2 

 

        ' Draw the letter in the middle of the panel 

        g.DrawString(Me.Name, font, brush2, x, y) 

    End Sub 

    Public Sub DrawOrangeHighlightedNode() 

        Dim g As Graphics = Me.CreateGraphics 

 

        ' Set the pen to use for drawing the border 

        Dim pen As New Pen(Color.Black, 4) 

 

        Dim brush As New SolidBrush(Color.OrangeRed) 

        g.FillEllipse(brush, 0, 0, Me.Width, Me.Height) 

 

        ' Draw the circular border around the panel 

        g.DrawEllipse(pen, 0, 0, Me.Width, Me.Height) 

 

        ' Set the font and color to use for the letter 

        Dim font As New Font("Tahoma", 30) 

        Dim brush2 As New SolidBrush(Color.Black) 

 

        ' Measure the size of the letter 

        Dim size As SizeF = g.MeasureString(Me.Name, font) 

 

        ' Calculate the position of the letter 

        Dim x As Single = (Me.Width - size.Width) / 2 

        Dim y As Single = (Me.Height - size.Height) / 2 

 

        ' Draw the letter in the middle of the panel 

        g.DrawString(Me.Name, font, brush2, x, y) 

    End Sub 

    Public Sub DrawNormalNode() 

        ' Get the graphics object for the panel 

        Dim g As Graphics = Me.CreateGraphics 

 

        ' Set the pen to use for drawing the border 

        Dim pen As New Pen(Color.Black, 4) 

 

        ' Fill the panel with a solid color 

        Dim brush As New SolidBrush(Color.AliceBlue) 

        g.FillEllipse(brush, 0, 0, Me.Width, Me.Height) 

 

        ' Draw the circular border around the panel 

        g.DrawEllipse(pen, 0, 0, Me.Width, Me.Height) 

 

        ' Set the font and color to use for the letter 

        Dim font As New Font("Tahoma", 30) 

        Dim brush2 As New SolidBrush(Color.Black) 

 

        ' Measure the size of the letter 

        Dim size As SizeF = g.MeasureString(Me.Name, font) 

 

        ' Calculate the position of the letter 

        Dim x As Single = (Me.Width - size.Width) / 2 

        Dim y As Single = (Me.Height - size.Height) / 2 

 

        ' Draw the letter in the middle of the panel 

        g.DrawString(Me.Name, font, brush2, x, y) 

    End Sub 

    Private Sub ConfigureMenu() 

        Dim RemovedItem As ToolStripMenuItem 

        'checks all nodes in the menu that are currently connected to the node 

        For Each item As ToolStripMenuItem In Me.MenuOptions 

            For Each connection As Connection In Me.Connections 

                If connection.GetEndNode.GetName() = item.Text.Last Then 

                    item.Checked = True 

                End If 

            Next 

            If item.Text.Last = Me.Name Then 

                RemovedItem = item 

            End If 

        Next 

        ' removes itself from the menu 

        Me.MenuOptions.Remove(RemovedItem) 

    End Sub 

    Sub GetMenuItem(ByVal sender As Object, ByVal e As EventArgs) 

        ' gets the node that was clicked  

        Dim MenuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem) 

        ' adds or removes the connection based on whether the node was checked or unchecked 

        If MenuItem.Checked = True Then 

            AddConnection(MenuItem.Text.Last) 

        Else 

            RemoveConnection(MenuItem.Text.Last) 

        End If 

 

    End Sub 

    Sub AddConnection(ByVal NodeName As String) 

        Dim CurrentGraphNodes As List(Of Node) 

        Dim NewConnectedNode As Node 

        Dim EdgeNumber As String 

 

        CurrentGraph = Form1.GetCurrentGraph() 

        CurrentGraphNodes = Form1.GetCurrentGraph().GetGraphNodes() 

 

        'finds the node in the current graph 

        For Each node As Node In CurrentGraphNodes 

            If NodeName = node.GetName() Then 

                NewConnectedNode = node 

            End If 

        Next 

 

        If Form1.GetPixelMode() = True Then 

            EdgeNumber = 0 

            SetConnection(NewConnectedNode, Int(EdgeNumber)) 

            'adds a connection from this node to the new connected node 

 

            Me.Connections(Me.Connections.Count - 1).SetPixelMode(Form1.GetPixelMode) 

            Me.Connections(Me.Connections.Count - 1).DrawLine() 

 

            CurrentGraph.UpdateAddedConnection(NewConnectedNode, EdgeNumber, Me) 

            'adds a connection from the new connected node to this node 

 

            Form1.FillAdjacencyMatrix() 

            Form1.FillAdjacencyList() 

        Else 

            While IsNumeric(EdgeNumber) = False 

                EdgeNumber = (InputBox("enter the edge number: ")) 

                If EdgeNumber = "" Then 

                    Exit While 

                End If 

            End While 

            If EdgeNumber = "" Then 

 

            Else 

                SetConnection(NewConnectedNode, Int(EdgeNumber)) 

                Me.Connections(Me.Connections.Count - 1).DrawLine() 

                Me.Connections(Me.Connections.Count - 1).SetPixelMode(Form1.GetPixelMode) 

                'adds a connection from this node to the new connected node 

 

                CurrentGraph.UpdateAddedConnection(NewConnectedNode, EdgeNumber, Me) 

                'adds a connection from the new connected node to this node 

 

                Form1.FillAdjacencyMatrix() 

                Form1.FillAdjacencyList() 

            End If 

        End If 

 

 

 

 

    End Sub 

    Sub AddSpecificConnection(AddedConnection As Connection) 

        Me.Connections.Add(AddedConnection) 

    End Sub 

    Sub RemoveConnection(ByVal NodeName As String) 

 

        Dim CurrentGraphNodes As List(Of Node) 

        Dim NodeUnconnected As Node 

        Dim RemovedConnection As Connection 

 

 

        CurrentGraph = Form1.GetCurrentGraph() 

        CurrentGraphNodes = Form1.GetCurrentGraph.GetGraphNodes() 

 

        'finds the node in the current graph 

        For Each node As Node In CurrentGraphNodes 

            If NodeName = node.GetName() Then 

                NodeUnconnected = node 

            End If 

 

        Next 

        For Each connection As Connection In Me.Connections 

            If connection.GetEndNode().GetName = NodeUnconnected.GetName() Then 

                RemovedConnection = connection 

                connection.RemoveLine() 

            End If 

        Next 

 

        Me.Connections.Remove(RemovedConnection) 

        'removes connection from this node to the new unconnected node 

        CurrentGraph.UpdateRemovedConnection(NodeUnconnected, Me) 

        'removes connection from new unconnected node to this node 

        Form1.FillAdjacencyMatrix() 

        Form1.FillAdjacencyList() 

    End Sub 

    Private Sub Me_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown 

        CurrentGraph = Form1.GetCurrentGraph() 

        'shows a drop down menu where the panel is right clicked 

        'the menu shows all nodes in the graph and the ones that are currently connected to this node are checked 

        If e.Button = MouseButtons.Right Then 

            CurrentGraph = Form1.GetCurrentGraph() 

            Me.Menu.Items.Clear() 

            Me.MenuOptions = CurrentGraph.LoadMenu() 

            ConfigureMenu() 

            For Each item As ToolStripMenuItem In MenuOptions 

                Me.Menu.Items.Add(item) 

            Next 

            Me.Menu.Show(Me, e.Location) 

            For Each item As ToolStripMenuItem In Menu.Items 

                AddHandler item.CheckedChanged, AddressOf GetMenuItem 

            Next 

        Else 

            ' Set isDragging to true and store the starting point of the drag operation 

            isDragging = True 

            StartPoint = New Point(e.X, e.Y) 

            'removes drawing of connections 

            For Each Node As Node In CurrentGraph.GetGraphNodes() 

                For Each Connection As Connection In Node.GetConnections() 

                    Connection.RemoveLine() 

                Next 

            Next 

 

        End If 

 

    End Sub 

 

 

 

    Private Sub Me_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove 

        If isDragging Then 

            ' Calculate the new position of the panel based on the current mouse position and the starting point of the drag operation 

            If Cursor.Position.X < Form1.Location.X + 50 OrElse Cursor.Position.X > Form1.Location.X + Form1.Size.Width - 50 OrElse 
        Cursor.Position.Y < Form1.Location.Y + 50 OrElse Cursor.Position.Y > Form1.Location.Y + Form1.Size.Height - 50 Then 

                isDragging = False 

            End If 

            Dim p As Panel = CType(sender, Panel) 

            p.Location = New Point(p.Location.X + (e.X - StartPoint.X), p.Location.Y + (e.Y - StartPoint.Y)) 

        End If 

    End Sub 

 

    Private Sub Me_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp 

        CurrentGraph = Form1.GetCurrentGraph() 

        ' Set isDragging to false to end the drag operation 

        isDragging = False 

        'redraws graph 

        For Each Node As Node In CurrentGraph.GetGraphNodes() 

            For Each Connection As Connection In Node.GetConnections() 

                Connection.UpdateEdgeNumber() 

                Connection.DrawLine() 

            Next 

        Next 

        If Form1.GetPixelMode() = True Then 

            Form1.FillAdjacencyMatrix() 

            Form1.FillAdjacencyList() 

        End If 

    End Sub 

 

    Public Sub SetName(Name) 

        Me.Name = Name 

    End Sub 

    Public Sub SetPreviousNode(PreviousNode As Node) 

        Me.PreviousNode = PreviousNode 

    End Sub 

    Public Sub SetShortestFromsource(ShortestFromsource As Integer) 

        Me.ShortestFromSource = ShortestFromsource 

    End Sub 

    Public Sub SetIsProcessed(Processed As Boolean) 

        Me.Processed = Processed 

    End Sub 

    Public Sub SetIsDisovered(Discovered As Boolean) 

        Me.Discovered = Discovered 

    End Sub 

    Public Sub SetConnection(ConnectedNode As Node, EdgeNumber As Integer) 

        Dim NewConnection As New Connection 

        NewConnection.SetStartNode(Me) 

        NewConnection.SetEndNode(ConnectedNode) 

        NewConnection.SetEdgeNumber(EdgeNumber) 

        Me.Connections.Add(NewConnection) 

    End Sub 

    Public Sub SetSpecificConnection(Connection As Connection) 

        Me.Connections.Add(Connection) 

    End Sub 

 

    Public Function GetName() As String 

        Return Me.Name 

    End Function 

    Public Function GetPreviousNode() As Node 

        Return Me.PreviousNode 

    End Function 

    Public Function GetShortestFromSource() As Integer 

        Return Me.ShortestFromSource 

    End Function 

    Public Function IsProcessed() As Boolean 

        Return Me.Processed 

    End Function 

    Public Function IsDiscovered() As Boolean 

        Return Me.Discovered 

    End Function 

    Public Function GetConnections() As List(Of Connection) 

        Return Me.Connections 

    End Function 

 

    Public Sub RemoveRemovedNodeConnections(RemovedNode As Node) 

        'removes connections to the removed node  

        Dim RemovedConnections As New List(Of Connection) 

        For Each connection As Connection In Me.Connections 

            If connection.GetEndNode().GetName = RemovedNode.GetName Then 

                RemovedConnections.Add(connection) 

            End If 

        Next 

        For Each removedconnection As Connection In RemovedConnections 

            Me.Connections.Remove(removedconnection) 

        Next 

    End Sub 

End Class 