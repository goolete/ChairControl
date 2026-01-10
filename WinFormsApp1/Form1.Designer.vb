<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.btnAutoControl = New System.Windows.Forms.Button()
        Me.btnHandControl = New System.Windows.Forms.Button()
        Me.btnSetting = New System.Windows.Forms.Button()
        Me.btnSwitchHand = New System.Windows.Forms.Button()
        Me.lblCurrentHand = New System.Windows.Forms.Label()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.lblLog = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnAutoControl
        '
        Me.btnAutoControl.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnAutoControl.Location = New System.Drawing.Point(50, 50)
        Me.btnAutoControl.Name = "btnAutoControl"
        Me.btnAutoControl.Size = New System.Drawing.Size(200, 60)
        Me.btnAutoControl.TabIndex = 0
        Me.btnAutoControl.Text = "自动控制模式"
        Me.btnAutoControl.UseVisualStyleBackColor = True
        '
        'btnHandControl
        '
        Me.btnHandControl.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnHandControl.Location = New System.Drawing.Point(300, 50)
        Me.btnHandControl.Name = "btnHandControl"
        Me.btnHandControl.Size = New System.Drawing.Size(200, 60)
        Me.btnHandControl.TabIndex = 1
        Me.btnHandControl.Text = "手动控制模式"
        Me.btnHandControl.UseVisualStyleBackColor = True
        '
        'btnSetting
        '
        Me.btnSetting.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnSetting.Location = New System.Drawing.Point(550, 50)
        Me.btnSetting.Name = "btnSetting"
        Me.btnSetting.Size = New System.Drawing.Size(200, 60)
        Me.btnSetting.TabIndex = 2
        Me.btnSetting.Text = "设置模式"
        Me.btnSetting.UseVisualStyleBackColor = True
        '
        'btnSwitchHand
        '
        Me.btnSwitchHand.Font = New System.Drawing.Font("Microsoft YaHei UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.btnSwitchHand.Location = New System.Drawing.Point(50, 150)
        Me.btnSwitchHand.Name = "btnSwitchHand"
        Me.btnSwitchHand.Size = New System.Drawing.Size(150, 40)
        Me.btnSwitchHand.TabIndex = 3
        Me.btnSwitchHand.Text = "切换左右手"
        Me.btnSwitchHand.UseVisualStyleBackColor = True
        '
        'lblCurrentHand
        '
        Me.lblCurrentHand.AutoSize = True
        Me.lblCurrentHand.Font = New System.Drawing.Font("Microsoft YaHei UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lblCurrentHand.Location = New System.Drawing.Point(220, 160)
        Me.lblCurrentHand.Name = "lblCurrentHand"
        Me.lblCurrentHand.Size = New System.Drawing.Size(163, 20)
        Me.lblCurrentHand.TabIndex = 4
        Me.lblCurrentHand.Text = "当前: 左手 (端口3001)"
        '
        'txtLog
        '
        Me.txtLog.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.txtLog.Location = New System.Drawing.Point(50, 230)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(700, 200)
        Me.txtLog.TabIndex = 5
        '
        'lblLog
        '
        Me.lblLog.AutoSize = True
        Me.lblLog.Font = New System.Drawing.Font("Microsoft YaHei UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lblLog.Location = New System.Drawing.Point(50, 207)
        Me.lblLog.Name = "lblLog"
        Me.lblLog.Size = New System.Drawing.Size(69, 20)
        Me.lblLog.TabIndex = 6
        Me.lblLog.Text = "通信日志:"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.lblLog)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.lblCurrentHand)
        Me.Controls.Add(Me.btnSwitchHand)
        Me.Controls.Add(Me.btnSetting)
        Me.Controls.Add(Me.btnHandControl)
        Me.Controls.Add(Me.btnAutoControl)
        Me.Name = "Form1"
        Me.Text = "机械手控制系统 - 主界面"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnAutoControl As Button
    Friend WithEvents btnHandControl As Button
    Friend WithEvents btnSetting As Button
    Friend WithEvents btnSwitchHand As Button
    Friend WithEvents lblCurrentHand As Label
    Friend WithEvents txtLog As TextBox
    Friend WithEvents lblLog As Label
End Class
