Option Strict On
Option Explicit On
Option Infer Off

Namespace Global.Mobzystems.CodeFragments
  ''' <summary>
  ''' The execution context of a fragment. 
  ''' Abstract, because a compiled code fragment will inherit from this class and supply a Main method.
  ''' Therefore, this class must be Public!
  ''' </summary>
  Public MustInherit Class FragmentContext
    Protected _outputWriter As Action(Of String)
    Protected _outputBuilder As System.Text.StringBuilder

    ''' <summary>
    ''' This method is defined in the fragment framework code. It calls main(args).
    ''' The first argument 
    ''' </summary>
    Public MustOverride Function FragmentMain(args() As String) As Object

    Public Sub New()
      Me._outputBuilder = New System.Text.StringBuilder()
    End Sub

    Public Sub New(outputWriter As Action(Of String))
      MyClass.New()
      Me._outputWriter = outputWriter
    End Sub

    ''' <summary>
    ''' Print a string to the context output
    ''' </summary>
    Public Sub Print(s As String)
      PerformOutput(s)
    End Sub

    ''' <summary>
    ''' Print an Object to the context output
    ''' </summary>
    Public Sub Print(o As Object)
      Me.PerformOutput(o.ToString())
    End Sub

    ''' <summary>
    ''' Print a string using string.Format syntax
    ''' </summary>
    Public Sub Print(format As String, ParamArray args() As Object)
      Me.PerformOutput(String.Format(format, args))
    End Sub

    ''' <summary>
    ''' Print a string to the context output with a newline
    ''' </summary>
    Public Sub PrintLine(s As String)
      Me.OutputLine(s)
    End Sub

    ''' <summary>
    ''' Print an object to the context output with a newline
    ''' </summary>
    Public Sub PrintLine(o As Object)
      Me.OutputLine(o.ToString())
    End Sub

    ''' <summary>
    ''' Print a line using string.Format syntax with a newline
    ''' </summary>
    Public Sub PrintLine(format As String, ParamArray args() As Object)
      Me.OutputLine(String.Format(format, args))
    End Sub

    ''' <summary>
    ''' Output a string to the context
    ''' </summary>
    ''' <remarks>
    ''' Simply forwards the output to the Output method of the fragment,
    ''' unless there is no output writer
    ''' </remarks>
    Protected Sub PerformOutput(s As String)
      'If _outputWriter Is Nothing Then
      '  Throw New InvalidOperationException("The string '" + s + "' was written to output using Print() or PrintLine(), but Run() was called with a null OutputWriter")
      'End If
      ' Store the output in the string builder
      Me._outputBuilder.Append(s)
      ' And write it to the output writer - if there is one
      If _outputWriter IsNot Nothing Then
        Me._outputWriter(s)
      End If
    End Sub

    ''' <summary>
    ''' Output a string to the context with a newline
    ''' </summary>
    Protected Sub OutputLine(s As String)
      Me.PerformOutput(s + vbCrLf)
    End Sub

    Friend Function GetFinalOutput() As String
      Return Me._outputBuilder.ToString()
    End Function
  End Class
End Namespace
