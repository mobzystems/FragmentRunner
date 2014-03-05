Option Strict On
Option Explicit On
Option Infer Off

Imports System.CodeDom.Compiler
Imports System.Reflection

Imports Microsoft.JScript
Imports Microsoft.CSharp
Imports Microsoft.VisualBasic

Imports Mobzystems.CodeFragments

''' <summary>
''' Abstract base class for all fragment compilers
''' </summary>
Public MustInherit Class FragmentCompiler
  ''' <summary>
  '''  The actual compiler
  ''' </summary>
  Protected _compiler As CodeDomProvider
  ''' <summary>
  ''' The compiler parameters to use
  ''' </summary>
  Protected _parameters As CompilerParameters

  ''' <summary>
  ''' Create a new compiler with the supplied CodeDomProvider
  ''' </summary>
  Protected Sub New(compiler As CodeDomProvider)
    Me._compiler = compiler
    Me._parameters = New CompilerParameters() With {
      .GenerateInMemory = True,
      .IncludeDebugInformation = True,
      .GenerateExecutable = False
    }
    Me._parameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location)
    ' Me._parameters.ReferencedAssemblies.Add("System.Xml.dll")
  End Sub

  ''' <summary>
  ''' Overrides this method to provide the wrapper code around the fragment source
  ''' </summary>
  Protected MustOverride Function DecorateSource(source As String) As String
  ''' <summary>
  ''' Override this to provide the first actual user-defined source code line number
  ''' </summary>
  Protected MustOverride Function FirstSourceLineNumber() As Integer
  ''' <summary>
  ''' Override this to provide the template source code for an empty fragment for the compilers language
  ''' </summary>
  Public MustOverride Function Template() As String

  ''' <summary>
  ''' Compile the source code into an executable CodeFragment
  ''' </summary>
  ''' <remarks>Check the IsSucceeded property of the result!</remarks>
  Public Function Compile(source As String) As CodeFragment
    ' Decorate the source code, then compile it using default parameters
    Dim r As CompilerResults = Me._compiler.CompileAssemblyFromSource(Me._parameters, Me.DecorateSource(source))

    'If r.Errors.Count > 0 Then
    '  Throw New Exception("Errors were found during compilation")
    'End If

    ' Return a new CodeFragment with the results.
    ' NOTE: compilation may have failed!
    Return New CodeFragment(r, Me.FirstSourceLineNumber())
  End Function
End Class

''' <summary>
''' Implementation of a FragmentCompiler for JScript
''' </summary>
Public Class JScriptFragmentCompiler
  Inherits FragmentCompiler

  Public Sub New()
    MyBase.New(New JScriptCodeProvider())
  End Sub

  Protected Overrides Function DecorateSource(source As String) As String
    Return String.Format(
      "import System;" +
      "package Fragment {{" +
      "  class Program extends Mobzystems.CodeFragments.FragmentContext {{" +
      "    public function Program(outputWriter) {{ this._outputWriter = outputWriter; }}" +
      "    public function FragmentMain(args: String[]) : Object {{ return main(args); }}" + vbCrLf +
      "{0}" + vbCrLf +
      "  }}" +
      "}}",
    source
    )
  End Function

  Protected Overrides Function FirstSourceLineNumber() As Integer
    Return 1
  End Function

  Public Overrides Function Template() As String
    Return "class TestClass {" + vbCrLf +
      "  function test() { return ""Hello, world""; }" + vbCrLf +
      "}" + vbCrLf +
      vbCrLf +
      "public function main(args: String[]) {" + vbCrLf +
      "  // Your code here. Use Print() and PrintLine() to display output" + vbCrLf +
      "  var t: TestClass = new TestClass();" + vbCrLf +
      "  PrintLine(t.test());" + vbCrLf +
      "  for (var i = 0; i < args.length; i++)" + vbCrLf +
      "     PrintLine(""Argument[{0}] = '{1}'"", i, args[i]);" + vbCrLf +
      "  return args.length;" + vbCrLf +
      "}" + vbCrLf
  End Function
End Class

''' <summary>
''' Implementation of a FragmentCompiler for CSharp
''' </summary>
Public Class CSharpFragmentCompiler
  Inherits FragmentCompiler

  Public Sub New()
    MyBase.New(New CSharpCodeProvider())
  End Sub

  Protected Overrides Function DecorateSource(source As String) As String
    Return String.Format(
      "using System;" +
      "namespace Fragment {{" +
      "  class Program : Mobzystems.CodeFragments.FragmentContext {{" +
      "    public Program(Action<string> outputWriter) : base(outputWriter) {{ }}" +
      "    public override object FragmentMain(string[] args) {{" +
      "      return main(args);" +
      "    }}" +
      vbCrLf +
      "{0}" + vbCrLf +
      "  }}" +
      "}}",
    source
    )
  End Function

  Protected Overrides Function FirstSourceLineNumber() As Integer
    Return 1
  End Function

  Public Overrides Function Template() As String
    Return "class TestClass {" + vbCrLf +
      "  public string Test() { return ""Hello, world""; }" + vbCrLf +
      "}" + vbCrLf +
      vbCrLf +
      "public object main(string[] args) {" + vbCrLf +
      "  // Your code here. Use Print() and PrintLine() to display output" + vbCrLf +
      "  var t = new TestClass();" + vbCrLf +
      "  PrintLine(t.Test());" + vbCrLf +
      "  for (int i = 0; i < args.Length; i++)" + vbCrLf +
      "    PrintLine(""Argument[{0}] = '{1}'"", i, args[i]);" + vbCrLf +
      "  return args.Length;" + vbCrLf +
      "}" + vbCrLf
  End Function
End Class

''' <summary>
''' Implementation of a FragmentCompiler for Visual Basic
''' </summary>
Public Class VisualBasicFragmentCompiler
  Inherits FragmentCompiler

  Public Sub New()
    MyBase.New(New VBCodeProvider())
  End Sub

  Protected Overrides Function DecorateSource(source As String) As String
    Return String.Format(
      "Imports System" + vbCrLf +
      "Namespace Global.Fragment" + vbCrLf +
      "  Public Class Program" + vbCrLf +
      "  Inherits Mobzystems.CodeFragments.FragmentContext" + vbCrLf +
      "  Public Sub New(outputWriter As Action(Of String))" + vbCrLf +
      "    MyBase.New(outputWriter)" + vbCrLf +
      "  End Sub" + vbCrLf +
      "  Public Overrides Function FragmentMain(args() As String) As Object" + vbCrLf +
      "    Return Main(args)" + vbCrLf +
      "  End Function" + vbCrLf +
      "{0}" + vbCrLf +
      "  End Class" + vbCrLf +
      "End Namespace",
    source
    )
  End Function

  Public Overrides Function Template() As String
    Return "Class TestClass" + vbCrLf +
      "  Public Function Test() As String" + vbCrLf +
      "    Return ""Hello, world""" + vbCrLf +
      "  End Function" + vbCrLf +
      "End Class" + vbCrLf +
      vbCrLf +
      "Function Main(Args() As String) As Object" + vbCrLf +
      "  ' Your code here. Use Print() and PrintLine() to display output" + vbCrLf +
      "  Dim t As New TestClass()" + vbCrLf +
      "  PrintLine(t.Test())" + vbCrLf +
      "  For I as Integer = 0 To Args.Length - 1" + vbCrLf +
      "    PrintLine(""Argument({0}) = '{1}'"", i, Args(i))" + vbCrLf +
      "  Next" + vbCrLf +
      "  Return Args.Length" + vbCrLf +
      "End Function" + vbCrLf
  End Function

  Protected Overrides Function FirstSourceLineNumber() As Integer
    Return 10
  End Function
End Class
