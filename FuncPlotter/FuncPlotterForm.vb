Imports Mobzystems.CodeFragments

Public Class FuncPlotterForm
  ' Margin of plotted picture in pixels
  Const MARGINX As Integer = 10
  Const MARGINY As Integer = 10

  ' The angle under which to display the Y axis. Defaults to 30
  ' Const XANGLE As Double = 0
  Const YANGLE As Double = 30

  ' Plot X and Y in this many steps
  Const STEPS As Integer = 100

  ' The fragment compiler to use
  Protected _compiler As FragmentCompiler
  ' The values returned from the fragment
  Protected _values As Double(,)

  ' Min/max of the 'real' x values to plot
  Protected _xMin As Double = -1
  Protected _xMax As Double = 1
  ' Steps to plot
  Protected _xSteps As Integer = STEPS + 1

  ' Min/max of the 'real' y values to plot
  Protected _yMin As Double = -1
  Protected _yMax As Double = 1
  Protected _ySteps As Integer = STEPS + 1

  ' 'Real' plotting coordinates
  Protected _px(,) As Double
  Protected _py(,) As Double

  ' Bounding box of the plotted area
  Protected _pxMin As Double
  Protected _pxMax As Double
  Protected _pyMin As Double
  Protected _pyMax As Double
  Protected _pzMin As Double
  Protected _pzMax As Double

  Const snor As String = "(x-1)*x*(x+1)+(y-1)*y*(y+1)"
  Const bult As String = "x*x+y*y"

  Public Sub New()
    InitializeComponent()

    ' Set up a CSharp-compiler
    Me._compiler = FragmentCompiler.CreateCompiler(FragmentCompiler.LanguageTypeEnum.CSharp)
  End Sub

  ''' <summary>
  ''' Create a fragment to calculate values for the function definition in the text box.
  ''' If it compiles, run it and store the values
  ''' </summary>
  Private Sub plotButton_Click(sender As Object, e As EventArgs) Handles plotButton.Click
    Dim func As String

    Select Case Me.functionTextBox.Text
      Case "snor"
        func = snor
      Case "bult"
        func = bult
      Case Else
        func = Me.functionTextBox.Text
    End Select

    Dim funcDef As String =
      "double xmin = double.Parse(args[0]);" +
      "double xmax = double.Parse(args[1]);" +
      "int nx = int.Parse(args[2]);" +
      "double ymin = double.Parse(args[3]);" +
      "double ymax = double.Parse(args[4]);" +
      "int ny = int.Parse(args[5]);" +
      "double dx = (xmax - xmin) / (nx - 1);" +
      "double dy = (ymax - ymin) / (ny - 1);" +
      "double[,] values = new double[nx, ny];" +
      "double x = xmin;" +
      "for (int ix = 0; ix < nx; ix++) {" +
      "  double y = ymin;" +
      "  for (int iy = 0; iy < ny; iy++) {" +
      "    values[ix, iy] = " + func + ";" +
      "    y += dy;" +
      "  }" +
      "  x += dx;" +
      "}" +
      "return values;"

    Dim fragment As CodeFragment = _compiler.CompileExpression(funcDef)

    If fragment.Succeeded Then
      ' Run the function definition and get the result into o
      Dim o As Object = fragment.Run(New String() {
        CStr(_xMin), CStr(_xMax), CStr(_xSteps),
        CStr(_yMin), CStr(_yMax), CStr(_ySteps)
      })

      ' Cast to an array of doubles and store in values()
      Me._values = DirectCast(o, Double(,))

      ' Calsulate plotted coordinates, bounding box, etc. so we don't have to when painting
      CalculatePlot()

      ' Make sure we repaint!
      Me.plotPictureBox.Invalidate()

    Else

      MessageBox.Show(Me, fragment.Errors(0).ErrorText, "Error in function definition", MessageBoxButtons.OK, MessageBoxIcon.Error)

    End If
  End Sub

  ''' <summary>
  ''' Calculate the plotted coordinated into px() and py().
  ''' Also determine bounding box of plot
  ''' </summary>
  Private Sub CalculatePlot()
    ' Cos and Sin of the Y angle
    'Dim sinX As Double = Math.Sin(Math.PI / 180 * XANGLE)
    'Dim cosX As Double = Math.Cos(Math.PI / 180 * XANGLE)
    Dim sinY As Double = Math.Sin(Math.PI / 180 * YANGLE)
    Dim cosY As Double = Math.Cos(Math.PI / 180 * YANGLE)

    ReDim _px(_xSteps - 1, _ySteps - 1)
    ReDim _py(_xSteps - 1, _ySteps - 1)

    _pxMin = Double.MaxValue
    _pxMax = Double.MinValue
    _pyMin = Double.MaxValue
    _pyMax = Double.MinValue
    _pzMin = Double.MaxValue
    _pzMax = Double.MinValue

    Dim dx As Double = (_xMax - _xMin) / (_xSteps - 1)
    Dim dy As Double = (_yMax - _yMin) / (_ySteps - 1)

    Dim xr As Double = _xMin
    For x As Integer = 0 To _xSteps - 1

      Dim yr As Double = _yMin
      For y As Integer = 0 To _ySteps - 1

        Dim zr As Double = _values(x, y)

        _px(x, y) = xr + yr * cosY
        _py(x, y) = yr * sinY + zr

        _pxMin = Math.Min(_pxMin, _px(x, y))
        _pxMax = Math.Max(_pxMax, _px(x, y))
        _pyMin = Math.Min(_pyMin, _py(x, y))
        _pyMax = Math.Max(_pyMax, _py(x, y))
        _pzMin = Math.Min(_pzMin, zr)
        _pzMax = Math.Max(_pzMax, zr)

        yr += dy
      Next

      xr += dx

    Next
  End Sub

  ''' <summary>
  ''' Paint the plot onto the picture box
  ''' </summary>
  Private Sub plotPictureBox_Paint(sender As Object, e As PaintEventArgs) Handles plotPictureBox.Paint
    ' Only paint if we have values
    If _values IsNot Nothing Then
      ' Size of the picture box
      Dim s As Size = plotPictureBox.ClientSize

      ' Center of the picture box
      Dim cx As Integer = CInt(s.Width / 2)
      Dim cy As Integer = CInt(s.Height / 2)

      ' Center of the plotted area
      Dim pcx As Double = (_pxMin + _pxMax) / 2
      Dim pcy As Double = (_pyMin + _pyMax) / 2

      ' Scale plotted area to picture box including margin
      Dim xScale As Double = (s.Width - 2 * MARGINX) / (_pxMax - _pxMin)
      Dim yScale As Double = (s.Height - 2 * MARGINY) / (_pyMax - _pyMin)

      ' Keep the aspect ratio 1:1
      If xScale > yScale Then
        xScale = yScale
      Else
        yScale = xScale
      End If

      If Me.fillCheckBox.Checked Then
        '' Show 2D bounding area
        'Dim pbb(3) As Point
        'pbb(0) = New Point(cx + CInt((_pxMin - pcx) * xScale), CInt(cy - (_pyMin - pcy) * yScale))
        'pbb(1) = New Point(cx + CInt((_pxMin - pcx) * xScale), CInt(cy - (_pyMax - pcy) * yScale))
        'pbb(2) = New Point(cx + CInt((_pxMax - pcx) * xScale), CInt(cy - (_pyMax - pcy) * yScale))
        'pbb(3) = New Point(cx + CInt((_pxMax - pcx) * xScale), CInt(cy - (_pyMin - pcy) * yScale))
        'e.Graphics.DrawPolygon(New Pen(Color.Red), pbb)

        ' Plot a polygon for each four point, +y to -y, then -x to +x
        Dim p2(_xSteps - 1, _ySteps - 1) As Point

        For y As Integer = _ySteps - 1 To 0 Step -1
          For x As Integer = 0 To _xSteps - 1
            p2(x, y) = New Point(cx + CInt((_px(x, y) - pcx) * xScale), CInt(cy - (_py(x, y) - pcy) * yScale))
          Next x
        Next y

        For y As Integer = _ySteps - 2 To 0 Step -1
          Dim c As Integer = 255 - CInt((y / _ySteps) * 128)
          For x As Integer = 0 To _xSteps - 2
            Dim pts(3) As Point
            pts(0) = p2(x, y)
            pts(2) = p2(x + 1, y + 1)
            pts(3) = p2(x, y + 1)
            pts(1) = p2(x + 1, y)

            Dim r As Integer = CInt(Int((_values(x, y) - _pzMin) / (_pzMax - _pzMin) * 255))
            If r < 0 Then r = 0
            If r > 255 Then r = 255
            e.Graphics.FillPolygon(New SolidBrush(Color.FromArgb(128 + 64, r, c, c)), pts)
            e.Graphics.DrawPolygon(New Pen(Color.FromArgb(c, 128, c)), pts)

          Next x
        Next y

      Else

        ' Plot back-to-front lines
        Dim points(_ySteps - 1) As Point

        For x As Integer = 0 To _xSteps - 1

          For y As Integer = 0 To _ySteps - 1
            points(y) = New Point(cx + CInt((_px(x, y) - pcx) * xScale), CInt(cy - (_py(x, y) - pcy) * yScale))
            If y > 0 Then
              Dim c As Integer = 255 - CInt((y / _ySteps) * 128)
              e.Graphics.DrawLine(New Pen(Color.FromArgb(c, c, c)), points(y - 1), points(y))
            End If
          Next y
        Next x

        ' Plot left-to-right lines in back-to-front order
        ReDim points(_xSteps - 1)

        For y As Integer = _ySteps - 1 To 0 Step -1
          For x As Integer = 0 To _xSteps - 1
            points(x) = New Point(cx + CInt((_px(x, y) - pcx) * xScale), CInt(cy - (_py(x, y) - pcy) * yScale))
          Next x

          Dim c As Integer = 255 - CInt((y / _ySteps) * 128)
          e.Graphics.DrawLines(New Pen(Color.FromArgb(c, c, c)), points)
        Next y
      End If
    End If
  End Sub

  ''' <summary>
  ''' Make sure we repaint the entire picture box when it resizes
  ''' </summary>
  Private Sub plotPictureBox_SizeChanged(sender As Object, e As EventArgs) Handles plotPictureBox.SizeChanged
    Me.plotPictureBox.Invalidate()
  End Sub

  Private Sub fillCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles fillCheckBox.CheckedChanged
    Me.plotPictureBox.Invalidate()
  End Sub
End Class
