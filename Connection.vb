Public Class Connection 

    Private StartNode As Node 

    Private EndNode As Node 

    Private EdgeNumber As Integer 

    Private PreviousEdgeNumber As Integer 

    Private StartPoint As Point 

    Private EndPoint As Point 

 

    Private PixelMode As Boolean 

 

    Public Sub New() 

 

    End Sub 

    Public Sub SetPixelMode(PixelMode As Boolean) 

        Me.PixelMode = PixelMode 

        ' sets the edge number based on the pixel mode 

        If PixelMode = True Then 

            Me.PreviousEdgeNumber = Me.EdgeNumber 

        Else 

            If Me.PreviousEdgeNumber = 0 Then 

 

            Else 

                Me.EdgeNumber = Me.PreviousEdgeNumber 

            End If 

 

        End If 

        UpdateEdgeNumber() 

    End Sub 

    Public Sub UpdateEdgeNumber() 

        If PixelMode = True Then 

            ' sets the edge number to the distance between the nodes in pixels 

            Me.EdgeNumber = 1 * Math.Round(Math.Sqrt(Math.Pow((Me.StartNode.Location.X - Me.EndNode.Location.X), 2) + Math.Pow((Me.StartNode.Location.Y - Me.EndNode.Location.Y), 2)) / 20, 1) 

        End If 

    End Sub 

 

    Public Sub DrawLine() 

 

        StartPoint = StartNode.Location 

        StartPoint.X += Me.StartNode.Width / 2 

        StartPoint.Y += Me.StartNode.Height / 2 

        EndPoint = EndNode.Location 

        EndPoint.X += Me.StartNode.Width / 2 

        EndPoint.Y += Me.StartNode.Height / 2 

        Dim g As Graphics = Form1.CreateGraphics() 

        Dim pen As New Pen(Color.Black, 4) 

 

 

        g.DrawLine(pen, StartPoint, EndPoint) 

        'draws line between the midpoint of the startnode and the midpoint of the endnode 

 

        Dim mp As Point = GetMidpoint(StartPoint, EndPoint) 

 

        g.FillRectangle(Brushes.White, mp.X - 5, mp.Y - 8, 10 * Me.EdgeNumber.ToString().Length, 16) 

        g.DrawString(Me.EdgeNumber, SystemFonts.DefaultFont, Brushes.Black, mp.X - 5, mp.Y - 8) 

        ' draws a rectangle at the midpoint of the connection line and draws the edge number in it 

        ' this rectangle expans depending on the edgenumber 

 

    End Sub 

    Public Sub DrawHighlightedLine(StartNode As Node, EndNode As Node) 

        StartPoint = StartNode.Location 

        StartPoint.X += Me.StartNode.Width / 2 

        StartPoint.Y += Me.StartNode.Height / 2 

        EndPoint = EndNode.Location 

        EndPoint.X += Me.StartNode.Width / 2 

        EndPoint.Y += Me.StartNode.Height / 2 

        Dim g As Graphics = Form1.CreateGraphics() 

        Dim pen As New Pen(Color.Yellow, 4) 

 

 

        g.DrawLine(pen, StartPoint, EndPoint) 

        'draws line between the midpoint of the startnode and the midpoint of the endnode 

 

        Dim mp As Point = GetMidpoint(StartPoint, EndPoint) 

 

        g.FillRectangle(Brushes.White, mp.X - 5, mp.Y - 8, 10 * Me.EdgeNumber.ToString().Length, 16) 

        g.DrawString(Me.EdgeNumber, SystemFonts.DefaultFont, Brushes.DarkBlue, mp.X - 5, mp.Y - 8) 

        ' draws a rectangle at the midpoint of the connection line and draws the edge number in it 

        ' this rectangle expans depending on the edgenumber 

    End Sub 

    Public Sub DrawRedHighlightedLine() 

        StartPoint = StartNode.Location 

        StartPoint.X += Me.StartNode.Width / 2 

        StartPoint.Y += Me.StartNode.Height / 2 

        EndPoint = EndNode.Location 

        EndPoint.X += Me.StartNode.Width / 2 

        EndPoint.Y += Me.StartNode.Height / 2 

        Dim g As Graphics = Form1.CreateGraphics() 

        Dim pen As New Pen(Color.PaleVioletRed, 4) 

 

 

        g.DrawLine(pen, StartPoint, EndPoint) 

        'draws line between the midpoint of the startnode and the midpoint of the endnode 

 

        Dim mp As Point = GetMidpoint(StartPoint, EndPoint) 

 

        g.FillRectangle(Brushes.White, mp.X - 5, mp.Y - 8, 10 * Me.EdgeNumber.ToString().Length, 16) 

        g.DrawString(Me.EdgeNumber, SystemFonts.DefaultFont, Brushes.Black, mp.X - 5, mp.Y - 8) 

        ' draws a rectangle at the midpoint of the connection line and draws the edge number in it 

        ' this rectangle expans depending on the edgenumber 

    End Sub 

    Public Sub DrawGreenHighlightedLine() 

        StartPoint = StartNode.Location 

        StartPoint.X += Me.StartNode.Width / 2 

        StartPoint.Y += Me.StartNode.Height / 2 

        EndPoint = EndNode.Location 

        EndPoint.X += Me.StartNode.Width / 2 

        EndPoint.Y += Me.StartNode.Height / 2 

        Dim g As Graphics = Form1.CreateGraphics() 

        Dim pen As New Pen(Color.LightGreen, 4) 

 

 

        g.DrawLine(pen, StartPoint, EndPoint) 

        'draws line between the midpoint of the startnode and the midpoint of the endnode 

 

        Dim mp As Point = GetMidpoint(StartPoint, EndPoint) 

 

        g.FillRectangle(Brushes.White, mp.X - 5, mp.Y - 8, 10 * Me.EdgeNumber.ToString().Length, 16) 

        g.DrawString(Me.EdgeNumber, SystemFonts.DefaultFont, Brushes.Black, mp.X - 5, mp.Y - 8) 

        ' draws a rectangle at the midpoint of the connection line and draws the edge number in it 

        ' this rectangle expans depending on the edgenumber 

    End Sub 

    Public Sub DrawOrangeHighlightedLine() 

        StartPoint = StartNode.Location 

        StartPoint.X += Me.StartNode.Width / 2 

        StartPoint.Y += Me.StartNode.Height / 2 

        EndPoint = EndNode.Location 

        EndPoint.X += Me.StartNode.Width / 2 

        EndPoint.Y += Me.StartNode.Height / 2 

        Dim g As Graphics = Form1.CreateGraphics() 

        Dim pen As New Pen(Color.OrangeRed, 4) 

 

 

        g.DrawLine(pen, StartPoint, EndPoint) 

        'draws line between the midpoint of the startnode and the midpoint of the endnode 

 

        Dim mp As Point = GetMidpoint(StartPoint, EndPoint) 

 

        g.FillRectangle(Brushes.White, mp.X - 5, mp.Y - 8, 10 * Me.EdgeNumber.ToString().Length, 16) 

        g.DrawString(Me.EdgeNumber, SystemFonts.DefaultFont, Brushes.Black, mp.X - 5, mp.Y - 8) 

        ' draws a rectangle at the midpoint of the connection line and draws the edge number in it 

        ' this rectangle expans depending on the edgenumber 

    End Sub 

    Public Sub RemoveLine() 

 

        StartPoint = StartNode.Location 

        StartPoint.X += Me.StartNode.Width / 2 

        StartPoint.Y += Me.StartNode.Height / 2 

        EndPoint = EndNode.Location 

        EndPoint.X += Me.StartNode.Width / 2 

        EndPoint.Y += Me.StartNode.Height / 2 

        Dim g As Graphics = Form1.CreateGraphics() 

        Dim pen As New Pen(Color.White, 4) 

 

        g.DrawLine(pen, StartPoint, EndPoint) 

 

        Dim mp As Point = GetMidpoint(StartPoint, EndPoint) 

 

        g.FillRectangle(Brushes.White, mp.X - 5, mp.Y - 8, 10 * Me.EdgeNumber.ToString().Length, 16) 

 

        ' draws over the line and the rectangle with the color of the form 

 

 

    End Sub 

 

 

    Private Function GetMidpoint(p1 As Point, p2 As Point) As Point 

        Dim x As Integer = (p1.X + p2.X) / 2 

        Dim y As Integer = (p1.Y + p2.Y) / 2 

        Return New Point(x, y) 

    End Function 

 

 

    Public Sub SetStartNode(StartNode As Node) 

        Me.StartNode = StartNode 

    End Sub 

    Public Sub SetEndNode(EndNode As Node) 

        Me.EndNode = EndNode 

    End Sub 

    Public Sub SetEdgeNumber(EdgeNumber As Integer) 

        Me.EdgeNumber = EdgeNumber 

    End Sub 

    Public Sub SetSpecificPixelMode(PixelMode As Boolean) 

        Me.PixelMode = PixelMode 

    End Sub 

    Public Sub SetPreviousEdgeNumber(PreviousEdgeNumber As Integer) 

        Me.PreviousEdgeNumber = PreviousEdgeNumber 

    End Sub 

 

    Public Function GetStartNode() As Node 

        Return Me.StartNode 

    End Function 

    Public Function GetEndNode() As Node 

        Return Me.EndNode 

    End Function 

    Public Function GetEdgeNumber() As Integer 

        Return Me.EdgeNumber 

    End Function 

    Public Function GetPixelMode() As Boolean 

        Return Me.PixelMode 

    End Function 

    Public Function GetPreviousEdgeNumber() As Integer 

        Return Me.PreviousEdgeNumber 

    End Function 

End Class 