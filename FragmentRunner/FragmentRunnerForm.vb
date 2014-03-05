Option Strict On
Option Explicit On
Option Infer Off

Imports System.CodeDom.Compiler

Public Class FragmentRunnerForm
  Private Sub runButton_Click(sender As Object, e As EventArgs) Handles runButton.Click
    If Me.mainTabControl.SelectedTab Is Nothing Then
      MessageBox.Show(Me, "Create a new fragment first!", "No fragment to run", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      Return
    End If

    Dim editor As FragmentEditor = DirectCast(Me.mainTabControl.SelectedTab.Tag, FragmentEditor)
    Try
      editor.CompileAndRun()
    Catch ex As Exception
      MessageBox.Show(Me, ex.Message, "Run fragment", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
  End Sub

  Public Sub New()
    InitializeComponent()

    Me.languageList.SelectedIndex = 0

    Me.CreateNewEditor("C#", New CSharpFragmentCompiler())
    Me.CreateNewEditor("VB.NET", New VisualBasicFragmentCompiler())
    Me.CreateNewEditor("JScript", New JScriptFragmentCompiler())

    Me.mainTabControl.SelectedIndex = 0
  End Sub

  Protected Function GetCurrentCompiler() As FragmentCompiler
    Select Case Me.languageList.SelectedIndex
      Case 0
        Return New CSharpFragmentCompiler
      Case 1
        Return New VisualBasicFragmentCompiler
      Case 2
        Return New JScriptFragmentCompiler
      Case Else
        Throw New Exception("Unknown language")
    End Select
  End Function

  Protected Sub CreateNewEditor(title As String, compiler As FragmentCompiler)
    Dim page As New TabPage
    page.Text = title

    Dim editor As New FragmentEditor(compiler)
    editor.Dock = DockStyle.Fill

    page.Controls.Add(editor)
    page.Tag = editor

    Me.mainTabControl.TabPages.Add(page)

    Me.mainTabControl.SelectedTab = page
  End Sub

  Private Sub newFragmentButton_Click(sender As Object, e As EventArgs) Handles newFragmentButton.Click
    CreateNewEditor(Me.languageList.Text, GetCurrentCompiler())
  End Sub

  Private Sub FragmentRunnerForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
    If e.KeyData = Keys.F5 Then
      e.Handled = True
      runButton.PerformClick()
    End If
  End Sub
End Class
