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
    Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
    Me.fragmentTextBox = New System.Windows.Forms.TextBox()
    Me.outputTextBox = New System.Windows.Forms.TextBox()
    Me.Panel1 = New System.Windows.Forms.Panel()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.argumentsTextBox = New System.Windows.Forms.TextBox()
    CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SplitContainer1.Panel1.SuspendLayout()
    Me.SplitContainer1.Panel2.SuspendLayout()
    Me.SplitContainer1.SuspendLayout()
    Me.Panel1.SuspendLayout()
    Me.SuspendLayout()
    '
    'SplitContainer1
    '
    Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.SplitContainer1.Location = New System.Drawing.Point(10, 38)
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
    Me.SplitContainer1.Size = New System.Drawing.Size(393, 315)
    Me.SplitContainer1.SplitterDistance = 202
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
    Me.fragmentTextBox.Size = New System.Drawing.Size(393, 202)
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
    Me.outputTextBox.Size = New System.Drawing.Size(393, 109)
    Me.outputTextBox.TabIndex = 0
    Me.outputTextBox.WordWrap = False
    '
    'Panel1
    '
    Me.Panel1.Controls.Add(Me.argumentsTextBox)
    Me.Panel1.Controls.Add(Me.Label1)
    Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
    Me.Panel1.Location = New System.Drawing.Point(10, 10)
    Me.Panel1.Name = "Panel1"
    Me.Panel1.Size = New System.Drawing.Size(393, 28)
    Me.Panel1.TabIndex = 1
    '
    'Label1
    '
    Me.Label1.Location = New System.Drawing.Point(3, 7)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(162, 17)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "&Arguments (separate with |):"
    '
    'argumentsTextBox
    '
    Me.argumentsTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.argumentsTextBox.Location = New System.Drawing.Point(153, 4)
    Me.argumentsTextBox.Name = "argumentsTextBox"
    Me.argumentsTextBox.Size = New System.Drawing.Size(237, 20)
    Me.argumentsTextBox.TabIndex = 1
    Me.argumentsTextBox.Text = "One|Two|Three"
    '
    'FragmentEditor
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.SplitContainer1)
    Me.Controls.Add(Me.Panel1)
    Me.Name = "FragmentEditor"
    Me.Size = New System.Drawing.Size(413, 363)
    Me.SplitContainer1.Panel1.ResumeLayout(False)
    Me.SplitContainer1.Panel1.PerformLayout()
    Me.SplitContainer1.Panel2.ResumeLayout(False)
    Me.SplitContainer1.Panel2.PerformLayout()
    CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.SplitContainer1.ResumeLayout(False)
    Me.Panel1.ResumeLayout(False)
    Me.Panel1.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
  Friend WithEvents fragmentTextBox As System.Windows.Forms.TextBox
  Friend WithEvents outputTextBox As System.Windows.Forms.TextBox
  Friend WithEvents Panel1 As System.Windows.Forms.Panel
  Friend WithEvents argumentsTextBox As System.Windows.Forms.TextBox
  Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
