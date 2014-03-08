<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FuncPlotterForm
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
    Me.Panel1 = New System.Windows.Forms.Panel()
    Me.fillCheckBox = New System.Windows.Forms.CheckBox()
    Me.plotButton = New System.Windows.Forms.Button()
    Me.functionTextBox = New System.Windows.Forms.TextBox()
    Me.plotPictureBox = New System.Windows.Forms.PictureBox()
    Me.Panel1.SuspendLayout()
    CType(Me.plotPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'Panel1
    '
    Me.Panel1.Controls.Add(Me.fillCheckBox)
    Me.Panel1.Controls.Add(Me.plotButton)
    Me.Panel1.Controls.Add(Me.functionTextBox)
    Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
    Me.Panel1.Location = New System.Drawing.Point(0, 0)
    Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
    Me.Panel1.Name = "Panel1"
    Me.Panel1.Size = New System.Drawing.Size(1068, 63)
    Me.Panel1.TabIndex = 0
    '
    'fillCheckBox
    '
    Me.fillCheckBox.AutoSize = True
    Me.fillCheckBox.Location = New System.Drawing.Point(13, 39)
    Me.fillCheckBox.Name = "fillCheckBox"
    Me.fillCheckBox.Size = New System.Drawing.Size(42, 21)
    Me.fillCheckBox.TabIndex = 2
    Me.fillCheckBox.Text = "Fill"
    Me.fillCheckBox.UseVisualStyleBackColor = True
    '
    'plotButton
    '
    Me.plotButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.plotButton.Location = New System.Drawing.Point(976, 4)
    Me.plotButton.Margin = New System.Windows.Forms.Padding(4)
    Me.plotButton.Name = "plotButton"
    Me.plotButton.Size = New System.Drawing.Size(88, 30)
    Me.plotButton.TabIndex = 1
    Me.plotButton.Text = "Plot"
    Me.plotButton.UseVisualStyleBackColor = True
    '
    'functionTextBox
    '
    Me.functionTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.functionTextBox.Location = New System.Drawing.Point(13, 8)
    Me.functionTextBox.Margin = New System.Windows.Forms.Padding(4)
    Me.functionTextBox.Name = "functionTextBox"
    Me.functionTextBox.Size = New System.Drawing.Size(955, 25)
    Me.functionTextBox.TabIndex = 0
    Me.functionTextBox.Text = "Math.Sin(x*Math.PI)*Math.Sin(y*Math.PI)"
    '
    'plotPictureBox
    '
    Me.plotPictureBox.BackColor = System.Drawing.Color.Black
    Me.plotPictureBox.Dock = System.Windows.Forms.DockStyle.Fill
    Me.plotPictureBox.Location = New System.Drawing.Point(0, 63)
    Me.plotPictureBox.Margin = New System.Windows.Forms.Padding(4)
    Me.plotPictureBox.Name = "plotPictureBox"
    Me.plotPictureBox.Size = New System.Drawing.Size(1068, 651)
    Me.plotPictureBox.TabIndex = 1
    Me.plotPictureBox.TabStop = False
    '
    'FuncPlotterForm
    '
    Me.AcceptButton = Me.plotButton
    Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1068, 714)
    Me.Controls.Add(Me.plotPictureBox)
    Me.Controls.Add(Me.Panel1)
    Me.Font = New System.Drawing.Font("Segoe UI Symbol", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Margin = New System.Windows.Forms.Padding(4)
    Me.Name = "FuncPlotterForm"
    Me.Text = "Mobzystems.CodeFragments Function Plotter"
    Me.Panel1.ResumeLayout(False)
    Me.Panel1.PerformLayout()
    CType(Me.plotPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents Panel1 As System.Windows.Forms.Panel
  Friend WithEvents plotButton As System.Windows.Forms.Button
  Friend WithEvents functionTextBox As System.Windows.Forms.TextBox
  Friend WithEvents plotPictureBox As System.Windows.Forms.PictureBox
  Friend WithEvents fillCheckBox As System.Windows.Forms.CheckBox

End Class
