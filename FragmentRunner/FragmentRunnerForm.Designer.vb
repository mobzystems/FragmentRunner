<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FragmentRunnerForm
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FragmentRunnerForm))
    Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
    Me.runButton = New System.Windows.Forms.ToolStripButton()
    Me.languageList = New System.Windows.Forms.ToolStripComboBox()
    Me.newFragmentButton = New System.Windows.Forms.ToolStripButton()
    Me.mainTabControl = New System.Windows.Forms.TabControl()
    Me.ToolStrip1.SuspendLayout()
    Me.SuspendLayout()
    '
    'ToolStrip1
    '
    Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
    Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.runButton, Me.languageList, Me.newFragmentButton})
    Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
    Me.ToolStrip1.Name = "ToolStrip1"
    Me.ToolStrip1.Size = New System.Drawing.Size(871, 25)
    Me.ToolStrip1.Stretch = True
    Me.ToolStrip1.TabIndex = 0
    Me.ToolStrip1.Text = "ToolStrip1"
    '
    'runButton
    '
    Me.runButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
    Me.runButton.Image = CType(resources.GetObject("runButton.Image"), System.Drawing.Image)
    Me.runButton.ImageTransparentColor = System.Drawing.Color.Magenta
    Me.runButton.Name = "runButton"
    Me.runButton.Size = New System.Drawing.Size(55, 22)
    Me.runButton.Text = "Run (F5)"
    '
    'languageList
    '
    Me.languageList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.languageList.Items.AddRange(New Object() {"C#", "VB.NET", "JScript"})
    Me.languageList.Name = "languageList"
    Me.languageList.Size = New System.Drawing.Size(121, 25)
    '
    'newFragmentButton
    '
    Me.newFragmentButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
    Me.newFragmentButton.Image = CType(resources.GetObject("newFragmentButton.Image"), System.Drawing.Image)
    Me.newFragmentButton.Name = "newFragmentButton"
    Me.newFragmentButton.Size = New System.Drawing.Size(87, 22)
    Me.newFragmentButton.Text = "New fragment"
    '
    'mainTabControl
    '
    Me.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill
    Me.mainTabControl.Location = New System.Drawing.Point(0, 25)
    Me.mainTabControl.Name = "mainTabControl"
    Me.mainTabControl.SelectedIndex = 0
    Me.mainTabControl.Size = New System.Drawing.Size(871, 590)
    Me.mainTabControl.TabIndex = 1
    '
    'FragmentRunnerForm
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(871, 615)
    Me.Controls.Add(Me.mainTabControl)
    Me.Controls.Add(Me.ToolStrip1)
    Me.KeyPreview = True
    Me.Name = "FragmentRunnerForm"
    Me.Text = "Fragment Runner by MOBZystems"
    Me.ToolStrip1.ResumeLayout(False)
    Me.ToolStrip1.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
  Friend WithEvents runButton As System.Windows.Forms.ToolStripButton
  Friend WithEvents languageList As System.Windows.Forms.ToolStripComboBox
  Friend WithEvents mainTabControl As System.Windows.Forms.TabControl
  Friend WithEvents newFragmentButton As System.Windows.Forms.ToolStripButton

End Class
