Option Strict On
Option Explicit On
Option Infer Off

Imports System.CodeDom.Compiler
Imports System.Reflection

Namespace Global.Mobzystems.CodeFragments
  ''' <summary>
  ''' Code fragment. The result of the compilation of a fragment of source text
  ''' by a FragmentCompiler.
  ''' </summary>
  Public Class CodeFragment
    ''' <summary>
    ''' The compiler results for this fragment
    ''' </summary>
    Protected _results As CompilerResults
    ''' <summary>
    ''' The line number of the first user-written source code (e.g. not the generated part)
    ''' </summary>
    Protected _firstsourceLineNumber As Integer

    ''' <summary>
    ''' Constructor for a new fragment
    ''' </summary>
    Friend Sub New(results As CompilerResults, firstSourceLineNumber As Integer)
      Me._results = results
      Me._firstsourceLineNumber = firstSourceLineNumber
    End Sub

    ''' <summary>
    ''' Did this fragment compile successfully?
    ''' </summary>
    Public ReadOnly Property Succeeded As Boolean
      Get
        Return Me.Errors.Count = 0
      End Get
    End Property

    ''' <summary>
    ''' The errors collection of this fragment. Contains 0 errors if fragment compiled
    ''' successfully.
    ''' </summary>
    Public ReadOnly Property Errors As CompilerErrorCollection
      Get
        Return Me._results.Errors
      End Get
    End Property

    ''' <summary>
    ''' Get the first line number (1+) of the user defined part of the fragment
    ''' </summary>
    Public ReadOnly Property FirstSourceLineNumber As Integer
      Get
        Return Me._firstsourceLineNumber
      End Get
    End Property

    ''' <summary>
    ''' Run the fragment. Return whatever object the Main() method in the fragment returns
    ''' </summary>
    Public Function Run(outputWriter As Action(Of String), arguments() As String) As Object
      ' Get the assembly the fragment is in
      Dim a As Assembly = Me._results.CompiledAssembly
      ' Get the main type in the assembly
      Dim t As Type = a.GetType("Fragment.Program")
      ' Create an instance of the type
      Dim o As Object = Activator.CreateInstance(t, outputWriter)
      ' Call the MainEntry method on the object, supplying a new fragment context
      Try
        Return t.InvokeMember(
          "FragmentMain",
          BindingFlags.InvokeMethod,
          Nothing,
          o,
          New Object() {
            arguments
          }
        )
      Catch ex As Exception
        Throw New Exception("Fragment execution failed: " + ex.Message, ex)
      End Try
    End Function
  End Class
End Namespace
