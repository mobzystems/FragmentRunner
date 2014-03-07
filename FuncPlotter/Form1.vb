Imports Mobzystems.CodeFragments

Public Class Form1
  Protected _fragment As CodeFragment
  Protected _compiler As FragmentCompiler
  Protected _values As Double(,)

  Protected _xMin As Double = -1
  Protected _xMax As Double = 1
  Protected _xSteps As Integer = 100 + 1

  Protected _yMin As Double = -1
  Protected _yMax As Double = 1
  Protected _ySteps As Integer = 100 + 1


  Protected _cosA As Double = Math.Cos(Math.PI / 180 * 30)
  Protected _sinA As Double = Math.Sin(Math.PI / 180 * 30)

  Public Sub New()
    InitializeComponent()

    _compiler = FragmentCompiler.CreateCompiler(FragmentCompiler.LanguageTypeEnum.CSharp)
  End Sub

  Private Sub plotButton_Click(sender As Object, e As EventArgs) Handles plotButton.Click
    Dim f As String =
      "double xmin = double.Parse(args[0]); double xmax = double.Parse(args[1]); int nx = int.Parse(args[2]); double ymin = double.Parse(args[3]); double ymax = double.Parse(args[4]); int ny = int.Parse(args[5]);" +
      "double dx = (xmax - xmin) / (nx - 1); double dy = (ymax - ymin) / (ny - 1);" +
      "double[,] values = new double[nx, ny];" +
      "double x = xmin;" +
      "for (int ix = 0; ix < nx; ix++) {" +
      "  double y = ymin;" +
      "  for (int iy = 0; iy < ny; iy++) {" +
      "    values[ix, iy] = " + TextBox1.Text + ";" +
      "    y += dy;" +
      "  }" +
      "  x += dx;" +
      "}" +
      "return values;"

    Dim fragment As CodeFragment = _compiler.CompileExpression(f)
    If fragment.Succeeded Then
      Dim o As Object = fragment.Run(New String() {
        CStr(_xMin), CStr(_xMax), CStr(_xSteps),
        CStr(_yMin), CStr(_yMax), CStr(_ySteps)
      })
      _values = DirectCast(o, Double(,))
      PictureBox1.Invalidate()
    Else
      MsgBox(fragment.Errors(0).ErrorText)
    End If
  End Sub

  Private Function Project(p As Point, z As Double) As Point
    Dim s As Size = PictureBox1.ClientSize
    Dim cx As Integer = CInt(s.Width / 2)
    Dim cy As Integer = CInt(s.Height / 2)
    Return New Point(cx + p.X + CInt(p.Y / 2), cy - CInt(p.Y / 2))
  End Function

  Private Sub plotButton_Paint(sender As Object, e As PaintEventArgs) Handles plotButton.Paint
    Using g As Graphics = Me.PictureBox1.CreateGraphics()
      If _values IsNot Nothing Then
        Dim px(_xSteps - 1, _ySteps - 1) As Double
        Dim py(_xSteps - 1, _ySteps - 1) As Double

        Dim pxMin As Double = Double.MaxValue
        Dim pxMax As Double = Double.MinValue
        Dim pyMin As Double = Double.MaxValue
        Dim pyMax As Double = Double.MinValue

        Dim dx As Double = (_xMax - _xMin) / (_xSteps - 1)
        Dim dy As Double = (_yMax - _yMin) / (_ySteps - 1)

        Dim xr As Double = _xMin
        For x As Integer = 0 To _xSteps - 1

          Dim yr As Double = _yMin
          For y As Integer = 0 To _ySteps - 1

            Dim zr As Double = _values(x, y)

            px(x, y) = xr + yr * _cosA
            py(x, y) = yr * _sinA + zr

            pxMin = Math.Min(pxMin, px(x, y))
            pxMax = Math.Max(pxMax, px(x, y))
            pyMin = Math.Min(pyMin, py(x, y))
            pyMax = Math.Max(pyMax, py(x, y))

            yr += dy
          Next

          xr += dx

        Next

        Dim s As Size = PictureBox1.ClientSize
        Dim cx As Integer = 0 'CInt(s.Width / 2)
        Dim cy As Integer = s.Height 'CInt(s.Height / 2)

        Dim xScale As Double = s.Width / (pxMax - pxMin)
        Dim yScale As Double = s.Height / (pyMax - pyMin)

        If xScale > yScale Then
          xScale = yScale
        Else
          yScale = xScale
        End If

        Dim points(_ySteps - 1) As Point
        For x As Integer = 0 To _xSteps - 1

          For y As Integer = 0 To _ySteps - 1
            points(y) = New Point(CInt((px(x, y) - pxMin) * xScale), CInt(cy - (py(x, y) - pyMin) * yScale))
            If y > 0 Then
              Dim c As Integer = 255 - CInt((y / _ySteps) * 128)
              g.DrawLine(New Pen(Color.FromArgb(c, c, c)), points(y - 1), points(y))
            End If
          Next y

          'g.DrawLines(Pens.White, points)
        Next x

        ReDim points(_xSteps - 1)

        For y As Integer = _ySteps - 1 To 0 Step -1
          For x As Integer = 0 To _xSteps - 1
            points(x) = New Point(CInt((px(x, y) - pxMin) * xScale), CInt(cy - (py(x, y) - pyMin) * yScale))
          Next x

          Dim c As Integer = 255 - CInt((y / _ySteps) * 128)
          g.DrawLines(New Pen(Color.FromArgb(c, c, c)), points)
        Next y

      Else
        g.DrawLine(Pens.Red, 0, 0, PictureBox1.ClientSize.Width - 1, PictureBox1.ClientSize.Height - 1)
      End If
    End Using
  End Sub

  Private Sub plotButton_SizeChanged(sender As Object, e As EventArgs) Handles plotButton.SizeChanged
    Me.PictureBox1.Invalidate()
    'Me.PictureBox1.Clear()
  End Sub
End Class
