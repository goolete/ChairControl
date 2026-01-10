Imports System.Threading
Imports System.Threading.Tasks

''' <summary>
''' 自动控制模式窗体 (AutoControlActivity)
''' </summary>
Public Class AutoControlForm
    Private Const MAX_RETRIES As Integer = 1
    Private Const RETRY_TIMEOUT As Integer = 800 ' 毫秒

    Private networkUtils As NetworkUtils
    Private retryTimer As System.Windows.Forms.Timer
    Private retryCount As Integer = 0
    Private isConnected As Boolean = False
    Private isClosing As Boolean = False

    Private Sub AutoControlForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 创建网络工具实例（根据当前选择的手：左/右）
        networkUtils = New NetworkUtils(HandSelect.SelectedHand)
        AddHandler networkUtils.OnSuccess, AddressOf OnSuccess
        AddHandler networkUtils.OnFailure, AddressOf OnFailure

        ' 创建重试定时器
        retryTimer = New System.Windows.Forms.Timer()
        retryTimer.Interval = RETRY_TIMEOUT
        AddHandler retryTimer.Tick, AddressOf RetryTimer_Tick

        ' 连接并发送进入自动控制模式指令
        ConnectAndSendEnterCommand()
    End Sub

    ''' <summary>
    ''' 连接并发送进入自动控制模式指令
    ''' 指令格式: AA BB 01 CC
    ''' </summary>
    Private Async Sub ConnectAndSendEnterCommand()
        Dim connected = Await networkUtils.ConnectAsync()
        If connected Then
            isConnected = True
            retryCount = 0
            Await SendEnterCommand()
        End If
    End Sub

    ''' <summary>
    ''' 发送进入自动控制模式指令
    ''' </summary>
    Private Async Function SendEnterCommand() As Threading.Tasks.Task
        Dim command As Byte() = {&HAA, &HBB, &H1, &HCC}
        LogMessage("发送进入自动控制模式指令: " & NetworkUtils.BytesToHex(command))
        retryTimer.Start()
        Await networkUtils.SendDataAsync(command)
    End Function

    ''' <summary>
    ''' 成功响应处理
    ''' </summary>
    Private Sub OnSuccess(response As Byte())
        Dim responseStr = NetworkUtils.BytesToString(response)
        LogMessage("收到响应: " & responseStr)

        ' 验证状态: "state auto\r\n"
        If responseStr.Contains("state auto") Then
            retryTimer.Stop()
            retryCount = 0
            LogMessage("成功进入自动控制模式")
        End If
    End Sub

    ''' <summary>
    ''' 失败响应处理
    ''' </summary>
    Private Sub OnFailure(errorMsg As String)
        LogMessage("错误: " & errorMsg)
    End Sub

    ''' <summary>
    ''' 重试定时器
    ''' </summary>
    Private Async Sub RetryTimer_Tick(sender As Object, e As EventArgs)
        retryTimer.Stop()
        If retryCount < MAX_RETRIES Then
            retryCount += 1
            LogMessage("重试第 " & retryCount & " 次")
            Await SendEnterCommand()
        Else
            LogMessage("达到最大重试次数，放弃")
        End If
    End Sub

    ''' <summary>
    ''' 执行自动程序1
    ''' 指令格式: AA BB 01 FF CC
    ''' </summary>
    Private Async Sub BtnProgram1_Click(sender As Object, e As EventArgs) Handles btnProgram1.Click
        If Not isConnected Then Return
        Dim command As Byte() = {&HAA, &HBB, &H1, &HFF, &HCC}
        LogMessage("执行程序1: " & NetworkUtils.BytesToHex(command))
        Await networkUtils.SendDataAsync(command)
    End Sub

    ''' <summary>
    ''' 执行自动程序2
    ''' 指令格式: AA BB 02 FF CC
    ''' </summary>
    Private Async Sub BtnProgram2_Click(sender As Object, e As EventArgs) Handles btnProgram2.Click
        If Not isConnected Then Return
        Dim command As Byte() = {&HAA, &HBB, &H2, &HFF, &HCC}
        LogMessage("执行程序2: " & NetworkUtils.BytesToHex(command))
        Await networkUtils.SendDataAsync(command)
    End Sub

    ''' <summary>
    ''' 执行自动程序3
    ''' 指令格式: AA BB 03 FF CC
    ''' </summary>
    Private Async Sub BtnProgram3_Click(sender As Object, e As EventArgs) Handles btnProgram3.Click
        If Not isConnected Then Return
        Dim command As Byte() = {&HAA, &HBB, &H3, &HFF, &HCC}
        LogMessage("执行程序3: " & NetworkUtils.BytesToHex(command))
        Await networkUtils.SendDataAsync(command)
    End Sub

    ''' <summary>
    ''' 执行自动程序4
    ''' 指令格式: AA BB 04 FF CC
    ''' </summary>
    Private Async Sub BtnProgram4_Click(sender As Object, e As EventArgs) Handles btnProgram4.Click
        If Not isConnected Then Return
        Dim command As Byte() = {&HAA, &HBB, &H4, &HFF, &HCC}
        LogMessage("执行程序4: " & NetworkUtils.BytesToHex(command))
        Await networkUtils.SendDataAsync(command)
    End Sub

    ''' <summary>
    ''' 记录日志
    ''' </summary>
    Private Sub LogMessage(message As String)
        If isClosing OrElse Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return
        If txtLog Is Nothing OrElse txtLog.IsDisposed Then Return

        If txtLog.InvokeRequired Then
            Try
                txtLog.Invoke(New Action(Of String)(AddressOf LogMessage), message)
            Catch
            End Try
        Else
            Dim timestamp = DateTime.Now.ToString("HH:mm:ss")
            txtLog.AppendText("[" & timestamp & "] " & message & vbCrLf)
            txtLog.ScrollToCaret()
        End If
    End Sub

    ''' <summary>
    ''' 窗体关闭时发送退出指令并清理资源
    ''' 指令格式: AA BB FF CC
    ''' </summary>
    Private Async Sub AutoControlForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        isClosing = True
        If retryTimer IsNot Nothing Then retryTimer.Stop()
        If isConnected Then
            Dim exitCommand As Byte() = {&HAA, &HBB, &HFF, &HCC}
            LogMessage("发送退出自动控制模式指令: " & NetworkUtils.BytesToHex(exitCommand))
            Await networkUtils.SendDataAsync(exitCommand)
        End If
        If networkUtils IsNot Nothing Then networkUtils.Close()
    End Sub
End Class

