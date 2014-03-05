Option Strict On
Option Explicit On
Option Infer Off

Namespace Global.Mobzystems.CodeFragments
  ' ''' <summary>
  ' ''' Event argument class for the OutputWritten event
  ' ''' </summary>
  ' ''' <remarks></remarks>
  'Public Class OutputWrittenEventArgs
  '  Inherits EventArgs

  '  ''' <summary>
  '  ''' The actual output
  '  ''' </summary>
  '  Protected _output As String

  '  ''' <summary>
  '  ''' The string that should was sent to output
  '  ''' </summary>
  '  Public ReadOnly Property Output As String
  '    Get
  '      Return Me._output
  '    End Get
  '  End Property

  '  ''' <summary>
  '  ''' Constructor with an output string
  '  ''' </summary>
  '  Protected Friend Sub New(s As String)
  '    Me._output = s
  '  End Sub
  'End Class

  ''' <summary>
  ''' The execution context of a fragment. 
  ''' Abstract, because a compiled code fragment will inherit from this class and supply a Main method
  ''' </summary>
  Public MustInherit Class FragmentContext
    Protected _outputWriter As Action(Of String)

    ''' <summary>
    ''' This method is defined in the fragment framework code. It calls main(args).
    ''' The first argument 
    ''' </summary>
    Public MustOverride Function FragmentMain(args() As String) As Object

    Public Sub New(outputWriter As Action(Of String))
      Me._outputWriter = outputWriter
    End Sub

    ''' <summary>
    ''' Print a string to the context output
    ''' </summary>
    Public Sub Print(s As String)
      Output(s)
    End Sub

    ''' <summary>
    ''' Print an Object to the context output
    ''' </summary>
    Public Sub Print(o As Object)
      Me.Output(o.ToString())
    End Sub

    ''' <summary>
    ''' Print a string using string.Format syntax
    ''' </summary>
    Public Sub Print(format As String, ParamArray args() As Object)
      Me.Output(String.Format(format, args))
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
    ''' <remarks>Simply forwards the output to the Output method of the fragment</remarks>
    Protected Sub Output(s As String)
      Me._outputWriter(s)
    End Sub

    ''' <summary>
    ''' Output a string to the context with a newline
    ''' </summary>
    Protected Sub OutputLine(s As String)
      Me.Output(s + vbCrLf)
    End Sub
  End Class
End Namespace
