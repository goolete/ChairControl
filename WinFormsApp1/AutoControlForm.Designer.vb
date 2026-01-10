<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AutoControlForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnProgram1 = New System.Windows.Forms.Button()
        Me.btnProgram2 = New System.Windows.Forms.Button()
        Me.btnProgram3 = New System.Windows.Forms.Button()
        Me.btnProgram4 = New System.Windows.Forms.Button()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnProgram1
        '
        Me.btnProgram1.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnProgram1.Location = New System.Drawing.Point(50, 80)
        Me.btnProgram1.Name = "btnProgram1"
        Me.btnProgram1.Size = New System.Drawing.Size(150, 80)
        Me.btnProgram1.TabIndex = 0
        Me.btnProgram1.Text = "程序1"
        Me.btnProgram1.UseVisualStyleBackColor = True
        '
        'btnProgram2
        '
        Me.btnProgram2.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnProgram2.Location = New System.Drawing.Point(230, 80)
        Me.btnProgram2.Name = "btnProgram2"
        Me.btnProgram2.Size = New System.Drawing.Size(150, 80)
        Me.btnProgram2.TabIndex = 1
        Me.btnProgram2.Text = "程序2"
        Me.btnProgram2.UseVisualStyleBackColor = True
        '
        'btnProgram3
        '
        Me.btnProgram3.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnProgram3.Location = New System.Drawing.Point(410, 80)
        Me.btnProgram3.Name = "btnProgram3"
        Me.btnProgram3.Size = New System.Drawing.Size(150, 80)
        Me.btnProgram3.TabIndex = 2
        Me.btnProgram3.Text = "程序3"
        Me.btnProgram3.UseVisualStyleBackColor = True
        '
        'btnProgram4
        '
        Me.btnProgram4.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnProgram4.Location = New System.Drawing.Point(590, 80)
        Me.btnProgram4.Name = "btnProgram4"
        Me.btnProgram4.Size = New System.Drawing.Size(150, 80)
        Me.btnProgram4.TabIndex = 3
        Me.btnProgram4.Text = "程序4"
        Me.btnProgram4.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.txtLog.Location = New System.Drawing.Point(50, 200)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(690, 250)
        Me.txtLog.TabIndex = 4
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft YaHei UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.lblTitle.Location = New System.Drawing.Point(50, 30)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(193, 26)
        Me.lblTitle.TabIndex = 5
        Me.lblTitle.Text = "自动控制模式"
        '
        'AutoControlForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 500)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.btnProgram4)
        Me.Controls.Add(Me.btnProgram3)
        Me.Controls.Add(Me.btnProgram2)
        Me.Controls.Add(Me.btnProgram1)
        Me.Name = "AutoControlForm"
        Me.Text = "自动控制模式"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnProgram1 As Button
    Friend WithEvents btnProgram2 As Button
    Friend WithEvents btnProgram3 As Button
    Friend WithEvents btnProgram4 As Button
    Friend WithEvents txtLog As TextBox
    Friend WithEvents lblTitle As Label
End Class

