Option Strict On
Option Explicit On
Option Infer Off

Imports System.CodeDom.Compiler
Imports System.Reflection

Namespace Global.Mobzystems.CodeFragments

  ''' <summary>
  ''' Abstract base class for all fragment compilers
  ''' </summary>
  Public MustInherit Class FragmentCompiler
    ''' <summary>
    ''' The list of supported languages
    ''' </summary>
    Public Enum LanguageTypeEnum
      CSharp
      VisualBasic
      JScript
    End Enum

    ''' <summary>
    ''' Information about a language
    ''' </summary>
    Public Class LanguageInfo
      Protected _languageType As LanguageTypeEnum
      Protected _language As String
      Protected _displayName As String
      Protected _compilerType As Type
      Protected _isValid As Boolean

      Public ReadOnly Property LanguageType As LanguageTypeEnum
        Get
          Return Me._languageType
        End Get
      End Property

      Public ReadOnly Property Language As String
        Get
          Return Me._language
        End Get
      End Property

      Public ReadOnly Property DisplayName As String
        Get
          Return Me._displayName
        End Get
      End Property

      Public ReadOnly Property IsValid As Boolean
        Get
          Return Me._isValid
        End Get
      End Property

      Protected Friend Sub New(languageType As LanguageTypeEnum, language As String, displayName As String, compilerType As Type)
        Me._languageType = languageType
        Me._language = language
        Me._displayName = displayName
        Me._compilerType = compilerType

        Dim ci As CompilerInfo = CodeDomProvider.GetCompilerInfo(language)
        Me._isValid = ci.IsCodeDomProviderTypeValid
      End Sub

      Protected Friend Function CreateCompiler() As FragmentCompiler
        Return DirectCast(Activator.CreateInstance(_compilerType), FragmentCompiler)
      End Function
    End Class

    ''' <summary>
    ''' The language info objects for each suported language in a dictionary
    ''' </summary>
    Protected Shared _languages As New Dictionary(Of LanguageTypeEnum, LanguageInfo) From {
      {LanguageTypeEnum.CSharp, New LanguageInfo(LanguageTypeEnum.CSharp, "c#", "C#", GetType(CSharpFragmentCompiler))},
      {LanguageTypeEnum.VisualBasic, New LanguageInfo(LanguageTypeEnum.VisualBasic, "visualbasic", "VB.NET", GetType(VisualBasicFragmentCompiler))},
      {LanguageTypeEnum.JScript, New LanguageInfo(LanguageTypeEnum.JScript, "jscript", "JScript", GetType(JScriptFragmentCompiler))}
    }

    ''' <summary>
    ''' Get information about a specific language
    ''' </summary>
    Public Shared Function GetLanguageInfo(language As LanguageTypeEnum) As LanguageInfo
      Return _languages(language)
    End Function

    ''' <summary>
    ''' Get the supported language types
    ''' </summary>
    Public Shared Function GetLanguageTypes() As IEnumerable(Of LanguageTypeEnum)
      Return _languages.Keys
    End Function

    ''' <summary>
    ''' Get the supported language types
    ''' </summary>
    Public Shared Function GetLanguages() As IEnumerable(Of LanguageInfo)
      Return _languages.Values
    End Function

    ''' <summary>
    ''' Create a compiler fot the supplied language
    ''' </summary>
    Public Shared Function CreateCompiler(language As LanguageTypeEnum) As FragmentCompiler
      Dim li As LanguageInfo = GetLanguageInfo(language)
      If Not li.IsValid Then
        Throw New NotImplementedException(String.Format("{0} is not a valid language on this computer", li.DisplayName))
      End If
      Return li.CreateCompiler()
    End Function

    ''' <summary>
    '''  The actual compiler
    ''' </summary>
    Protected _compiler As CodeDomProvider
    ''' <summary>
    ''' The compiler parameters to use
    ''' </summary>
    Protected _parameters As CompilerParameters

    ''' <summary>
    ''' The language of this compiler
    ''' </summary>
    Protected _language As String

    ''' <summary>
    ''' Create a new compiler with the supplied language
    ''' </summary>
    Protected Sub New(language As String)
      ' Store our language
      Me._language = language

      ' Create a compiler for the language
      Me._compiler = CodeDomProvider.CreateProvider(language)

      ' Get information about the language
      Dim ci As CompilerInfo = CodeDomProvider.GetCompilerInfo(language)
      ' Create parameters for the compiler
      Me._parameters = ci.CreateDefaultCompilerParameters()

      Me._parameters.GenerateInMemory = True
      Me._parameters.IncludeDebugInformation = False
      Me._parameters.GenerateExecutable = False

      Me._parameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location)

      ' Me._parameters.ReferencedAssemblies.Add("System.Xml.dll")
    End Sub

    ''' <summary>
    ''' Overrides this method to provide the wrapper code around the fragment source
    ''' </summary>
    Protected MustOverride Function DecorateSource(source As String) As String
    ''' <summary>
    ''' Overrides this method to provide the wrapper code around the fragment source
    ''' </summary>
    Protected MustOverride Function DecorateExpression(expression As String) As String
    ''' <summary>
    ''' Override this to provide the first actual user-defined source code line number
    ''' </summary>
    Protected MustOverride Function FirstSourceLineNumber() As Integer
    ''' <summary>
    ''' Override this to provide the template source code for an empty fragment for the compilers language
    ''' </summary>
    Public MustOverride Function Template() As String

    ''' <summary>
    ''' Compile the supplied source code into an executable CodeFragment.
    ''' The source code must be a complete implementation of the Main() method and optionally other user-defined classes
    ''' </summary>
    ''' <remarks>Check the Succeeded property of the result!</remarks>
    Public Function Compile(source As String) As CodeFragment
      ' Decorate the source code, then compile it using default parameters
      Dim r As CompilerResults = Me._compiler.CompileAssemblyFromSource(Me._parameters, Me.DecorateSource(source))

      ' Return a new CodeFragment with the results.
      ' NOTE: compilation may have failed!
      Return New CodeFragment(r, Me.FirstSourceLineNumber())
    End Function

    ''' <summary>
    ''' Compile a single Return statement, or a series of statements resulting in a return statement.
    ''' The statement(s) are compiled as the BODY of the Main() method
    ''' </summary>
    Public Function CompileExpression(expression As String) As CodeFragment
      Return Me.Compile(DecorateExpression(expression))
    End Function

    ''' <summary>
    ''' Implementation of a FragmentCompiler for CSharp
    ''' </summary>
    Protected Class CSharpFragmentCompiler
      Inherits FragmentCompiler

      Public Sub New()
        MyBase.New("c#")
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
          DecorateExpression(
            "  // Your code here. Use Print() and PrintLine() to display output" + vbCrLf +
            "  var t = new TestClass();" + vbCrLf +
            "  PrintLine(t.Test());" + vbCrLf +
            "  for (int i = 0; i < args.Length; i++)" + vbCrLf +
            "    PrintLine(""Argument[{0}] = '{1}'"", i, args[i]);" + vbCrLf +
            "  return args.Length;"
          ) + vbCrLf
      End Function

      Protected Overrides Function DecorateExpression(expression As String) As String
        Return "public object main(string[] args) {" + vbCrLf +
          expression + vbCrLf +
          "}"
      End Function
    End Class

    ''' <summary>
    ''' Implementation of a FragmentCompiler for Visual Basic
    ''' </summary>
    Protected Class VisualBasicFragmentCompiler
      Inherits FragmentCompiler

      Public Sub New()
        MyBase.New("visualbasic")
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

      Protected Overrides Function FirstSourceLineNumber() As Integer
        Return 10
      End Function

      Public Overrides Function Template() As String
        Return "Class TestClass" + vbCrLf +
          "  Public Function Test() As String" + vbCrLf +
          "    Return ""Hello, world""" + vbCrLf +
          "  End Function" + vbCrLf +
          "End Class" + vbCrLf +
          vbCrLf +
          DecorateExpression(
            "  ' Your code here. Use Print() and PrintLine() to display output" + vbCrLf +
            "  Dim t As New TestClass()" + vbCrLf +
            "  PrintLine(t.Test())" + vbCrLf +
            "  For I as Integer = 0 To Args.Length - 1" + vbCrLf +
            "    PrintLine(""Argument({0}) = '{1}'"", i, Args(i))" + vbCrLf +
            "  Next" + vbCrLf +
            "  Return Args.Length"
          ) + vbCrLf
      End Function

      Protected Overrides Function DecorateExpression(expression As String) As String
        Return "Function Main(Args() As String) As Object" + vbCrLf +
          expression + vbCrLf +
          "End Function"
      End Function
    End Class

    ''' <summary>
    ''' Implementation of a FragmentCompiler for JScript
    ''' </summary>
    Protected Class JScriptFragmentCompiler
      Inherits FragmentCompiler

      Public Sub New()
        MyBase.New("jscript")
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
          DecorateExpression(
            "  // Your code here. Use Print() and PrintLine() to display output" + vbCrLf +
            "  var t: TestClass = new TestClass();" + vbCrLf +
            "  PrintLine(t.test());" + vbCrLf +
            "  for (var i = 0; i < args.length; i++)" + vbCrLf +
            "     PrintLine(""Argument[{0}] = '{1}'"", i, args[i]);" + vbCrLf +
            "  return args.length;"
          ) + vbCrLf
      End Function

      Protected Overrides Function DecorateExpression(expression As String) As String
        Return "public function main(args: String[]) {" + vbCrLf +
          expression + vbCrLf +
          "}"
      End Function
    End Class

    ' ''' <summary>
    ' ''' Implementation of a FragmentCompiler for C++
    ' ''' </summary>
    'Public Class CppFragmentCompiler
    '  Inherits FragmentCompiler

    '  Public Sub New()
    '    MyBase.New("cpp")
    '  End Sub

    '  Protected Overrides Function DecorateSource(source As String) As String
    '    Return String.Format(
    '      "using System;" +
    '      "namespace Fragment {{" +
    '      "  class Program : Mobzystems.CodeFragments.FragmentContext {{" +
    '      "    public Program(Action<string> outputWriter) : base(outputWriter) {{ }}" +
    '      "    public override object FragmentMain(string[] args) {{" +
    '      "      return main(args);" +
    '      "    }}" +
    '      vbCrLf +
    '      "{0}" + vbCrLf +
    '      "  }}" +
    '      "}}",
    '    source
    '    )
    '  End Function

    '  Protected Overrides Function FirstSourceLineNumber() As Integer
    '    Return 1
    '  End Function

    '  Public Overrides Function Template() As String
    '    Return "class TestClass {" + vbCrLf +
    '      "  public string Test() { return ""Hello, world""; }" + vbCrLf +
    '      "}" + vbCrLf +
    '      vbCrLf +
    '      "public object main(string[] args) {" + vbCrLf +
    '      "  // Your code here. Use Print() and PrintLine() to display output" + vbCrLf +
    '      "  var t = new TestClass();" + vbCrLf +
    '      "  PrintLine(t.Test());" + vbCrLf +
    '      "  for (int i = 0; i < args.Length; i++)" + vbCrLf +
    '      "    PrintLine(""Argument[{0}] = '{1}'"", i, args[i]);" + vbCrLf +
    '      "  return args.Length;" + vbCrLf +
    '      "}" + vbCrLf
    '  End Function
    'End Class
  End Class
End Namespace
