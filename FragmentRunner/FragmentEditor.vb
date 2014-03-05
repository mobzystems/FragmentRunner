Option Strict On
Option Explicit On
Option Infer Off

Imports System.CodeDom.Compiler
Imports Mobzystems.CodeFragments

Public Class FragmentEditor
  Protected _compiler As FragmentCompiler

  Public Sub New()
    InitializeComponent()
  End Sub

  Public Sub New(compiler As FragmentCompiler)
    MyClass.New()
    Me._compiler = compiler
    Me.fragmentTextBox.Text = compiler.Template()
    Me.fragmentTextBox.Select(0, 0)
  End Sub

  Public Property Source As String
    Get
      Return Me.fragmentTextBox.Text
    End Get
    Set(value As String)
      Me.fragmentTextBox.Text = value
    End Set
  End Property

  Public Sub CompileAndRun()
    Me.outputTextBox.Clear()
    Dim s As CodeFragment = Me._compiler.Compile(Me.Source)
    If s.Succeeded Then
      Me.outputTextBox.ForeColor = Color.White
      Me.outputTextBox.Text += String.Format("--- Fragment started at {0:HH':'MM':'ss} ---", DateTime.Now) + vbCrLf
      Try
        Dim arguments() As String
        If String.IsNullOrEmpty(Me.argumentsTextBox.Text) Then
          ' Empty array
          ReDim arguments(-1)
        Else
          arguments = Me.argumentsTextBox.Text.Split("|"c)
        End If

        Dim retVal As Object = s.Run(
          Sub(output)
            Me.outputTextBox.Text += output
            Me.outputTextBox.Select(Me.outputTextBox.Text.Length, 0)
          End Sub,
          arguments
        )
        If retVal IsNot Nothing Then
          Me.outputTextBox.Text += String.Format("--- Return value was {0}", retVal) + vbCrLf
        End If
      Catch ex As Exception
        Me.outputTextBox.Text += String.Format("--- Unhandled exception: {0}", ex.Message) + vbCrLf
        Me.outputTextBox.Text += ex.StackTrace + vbCrLf
      Finally
        Me.outputTextBox.Text += String.Format("--- Fragment terminated at {0:HH':'MM':'ss} ---", DateTime.Now) + vbCrLf
      End Try
    Else
      Me.outputTextBox.ForeColor = Color.Red
      For Each e As CompilerError In s.Errors
        Me.outputTextBox.Text += String.Format("Line {0}, column {1}: {2}", e.Line - s.FirstSourceLineNumber, e.Column, e.ErrorText) + vbCrLf
      Next
    End If
  End Sub

  Private Sub editorToolstrip_SizeChanged(sender As Object, e As EventArgs) Handles editorToolstrip.SizeChanged
    Me.argumentsTextBox.Size = New Size(Me.editorToolstrip.Right - Me.argumentsTextBox.Bounds.Left - editorToolstrip.Margin.Left - editorToolstrip.Margin.Right - 5, Me.editorToolstrip.Size.Height)
  End Sub
End Class
