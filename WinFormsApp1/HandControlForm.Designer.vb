<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HandControlForm
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
        Me.sliderThumb = New System.Windows.Forms.TrackBar()
        Me.sliderIndex = New System.Windows.Forms.TrackBar()
        Me.sliderMiddle = New System.Windows.Forms.TrackBar()
        Me.sliderRing = New System.Windows.Forms.TrackBar()
        Me.sliderPinky = New System.Windows.Forms.TrackBar()
        Me.sliderElbow = New System.Windows.Forms.TrackBar()
        Me.lblThumb = New System.Windows.Forms.Label()
        Me.lblIndex = New System.Windows.Forms.Label()
        Me.lblMiddle = New System.Windows.Forms.Label()
        Me.lblRing = New System.Windows.Forms.Label()
        Me.lblPinky = New System.Windows.Forms.Label()
        Me.lblElbow = New System.Windows.Forms.Label()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        CType(Me.sliderThumb, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sliderIndex, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sliderMiddle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sliderRing, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sliderPinky, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sliderElbow, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'sliderThumb
        '
        Me.sliderThumb.Location = New System.Drawing.Point(100, 60)
        Me.sliderThumb.Maximum = 30
        Me.sliderThumb.Minimum = 1
        Me.sliderThumb.Name = "sliderThumb"
        Me.sliderThumb.Size = New System.Drawing.Size(200, 45)
        Me.sliderThumb.TabIndex = 0
        Me.sliderThumb.Value = 1
        '
        'sliderIndex
        '
        Me.sliderIndex.Location = New System.Drawing.Point(100, 110)
        Me.sliderIndex.Maximum = 30
        Me.sliderIndex.Minimum = 1
        Me.sliderIndex.Name = "sliderIndex"
        Me.sliderIndex.Size = New System.Drawing.Size(200, 45)
        Me.sliderIndex.TabIndex = 1
        Me.sliderIndex.Value = 1
        '
        'sliderMiddle
        '
        Me.sliderMiddle.Location = New System.Drawing.Point(100, 160)
        Me.sliderMiddle.Maximum = 30
        Me.sliderMiddle.Minimum = 1
        Me.sliderMiddle.Name = "sliderMiddle"
        Me.sliderMiddle.Size = New System.Drawing.Size(200, 45)
        Me.sliderMiddle.TabIndex = 2
        Me.sliderMiddle.Value = 1
        '
        'sliderRing
        '
        Me.sliderRing.Location = New System.Drawing.Point(400, 60)
        Me.sliderRing.Maximum = 30
        Me.sliderRing.Minimum = 1
        Me.sliderRing.Name = "sliderRing"
        Me.sliderRing.Size = New System.Drawing.Size(200, 45)
        Me.sliderRing.TabIndex = 3
        Me.sliderRing.Value = 1
        '
        'sliderPinky
        '
        Me.sliderPinky.Location = New System.Drawing.Point(400, 110)
        Me.sliderPinky.Maximum = 30
        Me.sliderPinky.Minimum = 1
        Me.sliderPinky.Name = "sliderPinky"
        Me.sliderPinky.Size = New System.Drawing.Size(200, 45)
        Me.sliderPinky.TabIndex = 4
        Me.sliderPinky.Value = 1
        '
        'sliderElbow
        '
        Me.sliderElbow.Location = New System.Drawing.Point(400, 160)
        Me.sliderElbow.Maximum = 20
        Me.sliderElbow.Minimum = 1
        Me.sliderElbow.Name = "sliderElbow"
        Me.sliderElbow.Size = New System.Drawing.Size(200, 45)
        Me.sliderElbow.TabIndex = 5
        Me.sliderElbow.Value = 1
        '
        'lblThumb
        '
        Me.lblThumb.AutoSize = True
        Me.lblThumb.Location = New System.Drawing.Point(50, 65)
        Me.lblThumb.Name = "lblThumb"
        Me.lblThumb.Size = New System.Drawing.Size(32, 17)
        Me.lblThumb.TabIndex = 6
        Me.lblThumb.Text = "拇指"
        '
        'lblIndex
        '
        Me.lblIndex.AutoSize = True
        Me.lblIndex.Location = New System.Drawing.Point(50, 115)
        Me.lblIndex.Name = "lblIndex"
        Me.lblIndex.Size = New System.Drawing.Size(32, 17)
        Me.lblIndex.TabIndex = 7
        Me.lblIndex.Text = "食指"
        '
        'lblMiddle
        '
        Me.lblMiddle.AutoSize = True
        Me.lblMiddle.Location = New System.Drawing.Point(50, 165)
        Me.lblMiddle.Name = "lblMiddle"
        Me.lblMiddle.Size = New System.Drawing.Size(32, 17)
        Me.lblMiddle.TabIndex = 8
        Me.lblMiddle.Text = "中指"
        '
        'lblRing
        '
        Me.lblRing.AutoSize = True
        Me.lblRing.Location = New System.Drawing.Point(350, 65)
        Me.lblRing.Name = "lblRing"
        Me.lblRing.Size = New System.Drawing.Size(44, 17)
        Me.lblRing.TabIndex = 9
        Me.lblRing.Text = "无名指"
        '
        'lblPinky
        '
        Me.lblPinky.AutoSize = True
        Me.lblPinky.Location = New System.Drawing.Point(350, 115)
        Me.lblPinky.Name = "lblPinky"
        Me.lblPinky.Size = New System.Drawing.Size(32, 17)
        Me.lblPinky.TabIndex = 10
        Me.lblPinky.Text = "小指"
        '
        'lblElbow
        '
        Me.lblElbow.AutoSize = True
        Me.lblElbow.Location = New System.Drawing.Point(350, 165)
        Me.lblElbow.Name = "lblElbow"
        Me.lblElbow.Size = New System.Drawing.Size(32, 17)
        Me.lblElbow.TabIndex = 11
        Me.lblElbow.Text = "手肘"
        '
        'txtLog
        '
        Me.txtLog.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.txtLog.Location = New System.Drawing.Point(50, 230)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(550, 250)
        Me.txtLog.TabIndex = 12
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft YaHei UI", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.lblTitle.Location = New System.Drawing.Point(50, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(193, 26)
        Me.lblTitle.TabIndex = 13
        Me.lblTitle.Text = "手动控制模式"
        '
        'HandControlForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(650, 500)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.lblElbow)
        Me.Controls.Add(Me.lblPinky)
        Me.Controls.Add(Me.lblRing)
        Me.Controls.Add(Me.lblMiddle)
        Me.Controls.Add(Me.lblIndex)
        Me.Controls.Add(Me.lblThumb)
        Me.Controls.Add(Me.sliderElbow)
        Me.Controls.Add(Me.sliderPinky)
        Me.Controls.Add(Me.sliderRing)
        Me.Controls.Add(Me.sliderMiddle)
        Me.Controls.Add(Me.sliderIndex)
        Me.Controls.Add(Me.sliderThumb)
        Me.Name = "HandControlForm"
        Me.Text = "手动控制模式"
        CType(Me.sliderThumb, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sliderIndex, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sliderMiddle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sliderRing, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sliderPinky, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sliderElbow, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents sliderThumb As TrackBar
    Friend WithEvents sliderIndex As TrackBar
    Friend WithEvents sliderMiddle As TrackBar
    Friend WithEvents sliderRing As TrackBar
    Friend WithEvents sliderPinky As TrackBar
    Friend WithEvents sliderElbow As TrackBar
    Friend WithEvents lblThumb As Label
    Friend WithEvents lblIndex As Label
    Friend WithEvents lblMiddle As Label
    Friend WithEvents lblRing As Label
    Friend WithEvents lblPinky As Label
    Friend WithEvents lblElbow As Label
    Friend WithEvents txtLog As TextBox
    Friend WithEvents lblTitle As Label
End Class

