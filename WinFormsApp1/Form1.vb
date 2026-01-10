Imports System.Threading
Imports System.Text

''' <summary>
''' 主界面窗体 (MainActivity)
''' </summary>
Public Class Form1
    Private Const MAX_RETRIES As Integer = 1
    Private Const RETRY_TIMEOUT As Integer = 800 ' 毫秒

    Private networkUtilsLeft As NetworkUtils ' 左手连接
    Private networkUtilsRight As NetworkUtils ' 右手连接
    Private retryTimerLeft As System.Windows.Forms.Timer
    Private retryTimerRight As System.Windows.Forms.Timer
    Private retryCountLeft As Integer = 0
    Private retryCountRight As Integer = 0
    Private isClosing As Boolean = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 初始化左右手选择
        HandSelect.Initialize()
        UpdateHandLabel()

        ' 如果不需要“设置”功能，可以直接隐藏按钮
        btnSetting.Visible = False

        ' 创建网络工具实例
        networkUtilsLeft = New NetworkUtils(False) ' 左手
        networkUtilsRight = New NetworkUtils(True) ' 右手

        ' 订阅事件
        AddHandler networkUtilsLeft.OnSuccess, AddressOf OnLeftHandSuccess
        AddHandler networkUtilsLeft.OnFailure, AddressOf OnLeftHandFailure
        AddHandler networkUtilsRight.OnSuccess, AddressOf OnRightHandSuccess
        AddHandler networkUtilsRight.OnFailure, AddressOf OnRightHandFailure

        ' 创建重试定时器
        retryTimerLeft = New System.Windows.Forms.Timer()
        retryTimerLeft.Interval = RETRY_TIMEOUT
        AddHandler retryTimerLeft.Tick, AddressOf RetryTimerLeft_Tick

        retryTimerRight = New System.Windows.Forms.Timer()
        retryTimerRight.Interval = RETRY_TIMEOUT
        AddHandler retryTimerRight.Tick, AddressOf RetryTimerRight_Tick

        ' 连接并发送进入主界面指令
        ConnectAndSendMainCommand()
    End Sub

    ''' <summary>
    ''' 连接并发送进入主界面指令
    ''' </summary>
    Private Async Sub ConnectAndSendMainCommand()
        ' 连接左手
        Dim leftConnected = Await networkUtilsLeft.ConnectAsync()
        If leftConnected Then
            retryCountLeft = 0
            Await SendMainCommandLeft()
        End If

        ' 连接右手
        Dim rightConnected = Await networkUtilsRight.ConnectAsync()
        If rightConnected Then
            retryCountRight = 0
            Await SendMainCommandRight()
        End If
    End Sub

    ''' <summary>
    ''' 发送进入主界面指令到左手
    ''' 指令格式: AA BB FF CC
    ''' </summary>
    Private Async Function SendMainCommandLeft() As Threading.Tasks.Task
        Dim command As Byte() = {&HAA, &HBB, &HFF, &HCC}
        LogMessage("左手发送指令: " & NetworkUtils.BytesToHex(command))
        retryTimerLeft.Start()
        Await networkUtilsLeft.SendDataAsync(command)
    End Function

    ''' <summary>
    ''' 发送进入主界面指令到右手
    ''' </summary>
    Private Async Function SendMainCommandRight() As Threading.Tasks.Task
        Dim command As Byte() = {&HAA, &HBB, &HFF, &HCC}
        LogMessage("右手发送指令: " & NetworkUtils.BytesToHex(command))
        retryTimerRight.Start()
        Await networkUtilsRight.SendDataAsync(command)
    End Function

    ''' <summary>
    ''' 左手成功响应
    ''' </summary>
    Private Sub OnLeftHandSuccess(response As Byte())
        retryTimerLeft.Stop()
        retryCountLeft = 0
        Dim responseStr = NetworkUtils.BytesToString(response)
        LogMessage("左手收到响应: " & responseStr)
    End Sub

    ''' <summary>
    ''' 左手失败响应
    ''' </summary>
    Private Sub OnLeftHandFailure(errorMsg As String)
        LogMessage("左手错误: " & errorMsg)
    End Sub

    ''' <summary>
    ''' 右手成功响应
    ''' </summary>
    Private Sub OnRightHandSuccess(response As Byte())
        retryTimerRight.Stop()
        retryCountRight = 0
        Dim responseStr = NetworkUtils.BytesToString(response)
        LogMessage("右手收到响应: " & responseStr)
    End Sub

    ''' <summary>
    ''' 右手失败响应
    ''' </summary>
    Private Sub OnRightHandFailure(errorMsg As String)
        LogMessage("右手错误: " & errorMsg)
    End Sub

    ''' <summary>
    ''' 左手重试定时器
    ''' </summary>
    Private Async Sub RetryTimerLeft_Tick(sender As Object, e As EventArgs)
        retryTimerLeft.Stop()
        If retryCountLeft < MAX_RETRIES Then
            retryCountLeft += 1
            LogMessage("左手重试第 " & retryCountLeft & " 次")
            Await SendMainCommandLeft()
        Else
            LogMessage("左手达到最大重试次数，放弃")
        End If
    End Sub

    ''' <summary>
    ''' 右手重试定时器
    ''' </summary>
    Private Async Sub RetryTimerRight_Tick(sender As Object, e As EventArgs)
        retryTimerRight.Stop()
        If retryCountRight < MAX_RETRIES Then
            retryCountRight += 1
            LogMessage("右手重试第 " & retryCountRight & " 次")
            Await SendMainCommandRight()
        Else
            LogMessage("右手达到最大重试次数，放弃")
        End If
    End Sub

    ''' <summary>
    ''' 打开自动控制模式（模态窗口，主界面不能点击）
    ''' </summary>
    Private Sub BtnAutoControl_Click(sender As Object, e As EventArgs) Handles btnAutoControl.Click
        Using autoForm As New AutoControlForm()
            autoForm.ShowDialog(Me)
        End Using
    End Sub

    ''' <summary>
    ''' 打开手动控制模式（模态窗口）
    ''' </summary>
    Private Sub BtnHandControl_Click(sender As Object, e As EventArgs) Handles btnHandControl.Click
        Using handForm As New HandControlForm()
            handForm.ShowDialog(Me)
        End Using
    End Sub

    ''' <summary>
    ''' 打开设置模式（如果你不需要，可以把按钮隐藏或删除）
    ''' </summary>
    Private Sub BtnSetting_Click(sender As Object, e As EventArgs) Handles btnSetting.Click
        Using settingForm As New SettingForm()
            settingForm.ShowDialog(Me)
        End Using
    End Sub

    ''' <summary>
    ''' 切换左右手
    ''' </summary>
    Private Sub BtnSwitchHand_Click(sender As Object, e As EventArgs) Handles btnSwitchHand.Click
        HandSelect.SelectedHand = Not HandSelect.SelectedHand
        UpdateHandLabel()
    End Sub

    ''' <summary>
    ''' 更新左右手标签
    ''' </summary>
    Private Sub UpdateHandLabel()
        lblCurrentHand.Text = If(HandSelect.SelectedHand, "当前: 右手 (端口4001)", "当前: 左手 (端口3001)")
    End Sub

    ''' <summary>
    ''' 记录日志
    ''' </summary>
    Private Sub LogMessage(message As String)
        ' 窗体或控件已关闭时不再写日志，避免 ObjectDisposedException
        If isClosing OrElse Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return
        If txtLog Is Nothing OrElse txtLog.IsDisposed Then Return

        If txtLog.InvokeRequired Then
            Try
                txtLog.Invoke(New Action(Of String)(AddressOf LogMessage), message)
            Catch
                ' 窗体正在关闭时可能仍然抛异常，直接忽略
            End Try
        Else
            Dim timestamp = DateTime.Now.ToString("HH:mm:ss")
            txtLog.AppendText("[" & timestamp & "] " & message & vbCrLf)
            txtLog.ScrollToCaret()
        End If
    End Sub

    ''' <summary>
    ''' 窗体关闭时清理资源
    ''' </summary>
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        isClosing = True

        ' 停止所有定时器
        If retryTimerLeft IsNot Nothing Then
            retryTimerLeft.Stop()
            retryTimerLeft.Dispose()
        End If
        If retryTimerRight IsNot Nothing Then
            retryTimerRight.Stop()
            retryTimerRight.Dispose()
        End If

        ' 关闭网络连接
        If networkUtilsLeft IsNot Nothing Then
            networkUtilsLeft.Close()
        End If
        If networkUtilsRight IsNot Nothing Then
            networkUtilsRight.Close()
        End If
    End Sub
End Class
