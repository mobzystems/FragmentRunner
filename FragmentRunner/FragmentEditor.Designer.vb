<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FragmentEditor
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
    Dim ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
    Me.fragmentTextBox = New System.Windows.Forms.TextBox()
    Me.outputTextBox = New System.Windows.Forms.TextBox()
    Me.editorToolstrip = New System.Windows.Forms.ToolStrip()
    Me.argumentsTextBox = New System.Windows.Forms.ToolStripTextBox()
    ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
    CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SplitContainer1.Panel1.SuspendLayout()
    Me.SplitContainer1.Panel2.SuspendLayout()
    Me.SplitContainer1.SuspendLayout()
    Me.editorToolstrip.SuspendLayout()
    Me.SuspendLayout()
    '
    'ToolStripLabel1
    '
    ToolStripLabel1.Name = "ToolStripLabel1"
    ToolStripLabel1.Size = New System.Drawing.Size(204, 22)
    ToolStripLabel1.Text = "Run with arguments (separate with |):"
    '
    'SplitContainer1
    '
    Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.SplitContainer1.Location = New System.Drawing.Point(0, 25)
    Me.SplitContainer1.Name = "SplitContainer1"
    Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
    '
    'SplitContainer1.Panel1
    '
    Me.SplitContainer1.Panel1.Controls.Add(Me.fragmentTextBox)
    '
    'SplitContainer1.Panel2
    '
    Me.SplitContainer1.Panel2.Controls.Add(Me.outputTextBox)
    Me.SplitContainer1.Size = New System.Drawing.Size(683, 480)
    Me.SplitContainer1.SplitterDistance = 309
    Me.SplitContainer1.TabIndex = 0
    '
    'fragmentTextBox
    '
    Me.fragmentTextBox.AcceptsReturn = True
    Me.fragmentTextBox.AcceptsTab = True
    Me.fragmentTextBox.Dock = System.Windows.Forms.DockStyle.Fill
    Me.fragmentTextBox.Font = New System.Drawing.Font("Lucida Console", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.fragmentTextBox.Location = New System.Drawing.Point(0, 0)
    Me.fragmentTextBox.Multiline = True
    Me.fragmentTextBox.Name = "fragmentTextBox"
    Me.fragmentTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
    Me.fragmentTextBox.Size = New System.Drawing.Size(683, 309)
    Me.fragmentTextBox.TabIndex = 0
    Me.fragmentTextBox.WordWrap = False
    '
    'outputTextBox
    '
    Me.outputTextBox.BackColor = System.Drawing.Color.Black
    Me.outputTextBox.Dock = System.Windows.Forms.DockStyle.Fill
    Me.outputTextBox.Font = New System.Drawing.Font("Lucida Console", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.outputTextBox.ForeColor = System.Drawing.Color.White
    Me.outputTextBox.Location = New System.Drawing.Point(0, 0)
    Me.outputTextBox.Multiline = True
    Me.outputTextBox.Name = "outputTextBox"
    Me.outputTextBox.ReadOnly = True
    Me.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
    Me.outputTextBox.Size = New System.Drawing.Size(683, 167)
    Me.outputTextBox.TabIndex = 0
    Me.outputTextBox.WordWrap = False
    '
    'editorToolstrip
    '
    Me.editorToolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
    Me.editorToolstrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {ToolStripLabel1, Me.argumentsTextBox})
    Me.editorToolstrip.Location = New System.Drawing.Point(0, 0)
    Me.editorToolstrip.Name = "editorToolstrip"
    Me.editorToolstrip.Size = New System.Drawing.Size(683, 25)
    Me.editorToolstrip.Stretch = True
    Me.editorToolstrip.TabIndex = 1
    Me.editorToolstrip.Text = "ToolStrip1"
    '
    'argumentsTextBox
    '
    Me.argumentsTextBox.AutoSize = False
    Me.argumentsTextBox.Name = "argumentsTextBox"
    Me.argumentsTextBox.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
    Me.argumentsTextBox.Size = New System.Drawing.Size(100, 25)
    Me.argumentsTextBox.Text = "One|Two|Three"
    Me.argumentsTextBox.ToolTipText = "Enter arguments, separated by a pipe symbol (|)"
    '
    'FragmentEditor
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.SplitContainer1)
    Me.Controls.Add(Me.editorToolstrip)
    Me.Name = "FragmentEditor"
    Me.Size = New System.Drawing.Size(683, 505)
    Me.SplitContainer1.Panel1.ResumeLayout(False)
    Me.SplitContainer1.Panel1.PerformLayout()
    Me.SplitContainer1.Panel2.ResumeLayout(False)
    Me.SplitContainer1.Panel2.PerformLayout()
    CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.SplitContainer1.ResumeLayout(False)
    Me.editorToolstrip.ResumeLayout(False)
    Me.editorToolstrip.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
  Friend WithEvents fragmentTextBox As System.Windows.Forms.TextBox
  Friend WithEvents outputTextBox As System.Windows.Forms.TextBox
  Friend WithEvents editorToolstrip As System.Windows.Forms.ToolStrip
  Friend WithEvents argumentsTextBox As System.Windows.Forms.ToolStripTextBox

End Class
