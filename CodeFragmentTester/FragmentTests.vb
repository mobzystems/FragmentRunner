Option Strict On
Option Explicit On
Option Infer Off

Imports Mobzystems.CodeFragments

Module FragmentTests

  Sub Main()
    ' Create a compiler for C#. Also supports VB.NET and JScript
    Dim compiler As FragmentCompiler =
      FragmentCompiler.CreateCompiler(FragmentCompiler.LanguageTypeEnum.CSharp)

    ' Compile a source code fragment that returns something based on its arguments
    Dim c As CodeFragment = compiler.CompileExpression(
      "PrintLine(""Executed at "" + DateTime.Now.Ticks.ToString());" +
      "if (args.Length == 0) return ""Nothing to do!"";" +
      "if (args.Length == 1) return args[0];" +
      "return args.Length;"
    )

    ' If compiling succeeded, run the fragment three times and display the result
    If c.Succeeded Then
      '--> "Nothing to do!"
      Console.WriteLine(c.Run())
      Console.Write(c.Output)
      ' --> "and a one"
      Console.WriteLine(c.Run(New String() {"and a one"}))
      Console.Write(c.Output)
      ' --> "2"
      Console.WriteLine(c.Run(New String() {"one", "two"}))
      Console.Write(c.Output)
    Else
      ' On errors, dump the first error
      Console.Error.WriteLine(c.Errors(0).ErrorText)
    End If
  End Sub
End Module
