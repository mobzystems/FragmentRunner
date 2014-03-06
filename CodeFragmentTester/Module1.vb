Option Strict On
Option Explicit On
Option Infer Off

Imports Mobzystems.CodeFragments

Module Module1

  Sub Main()
    Dim compiler As FragmentCompiler = FragmentCompiler.CreateCompiler(FragmentCompiler.LanguageTypeEnum.CSharp)
    Dim c As CodeFragment = compiler.CompileExpression(
      "if (args.Length == 0) return ""Nothing to do!"";" +
      "if (args.Length == 1) return args[0];" +
      "return args.Length;"
    )
    If c.Succeeded Then
      Console.WriteLine(c.Run(Nothing, Nothing)) '--> "Nothing to do!"
      Console.WriteLine(c.Run(Nothing, New String() {"and a one"})) ' --> "and a one"
      Console.WriteLine(c.Run(Nothing, New String() {"one", "two"})) ' --> "2"
    Else
      ' On errors, dump the first error
      Console.Error.WriteLine(c.Errors(0).ErrorText)
    End If
  End Sub
End Module
