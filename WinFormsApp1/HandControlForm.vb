Imports System.Threading
Imports System.Threading.Tasks

''' <summary>
''' 手动控制模式窗体 (HandControlActivity)
''' </summary>
Public Class HandControlForm
    Private Const MAX_RETRIES As Integer = 1
    Private Const RETRY_TIMEOUT As Integer = 800 ' 毫秒

    Private networkUtils As NetworkUtils
    Private retryTimer As System.Windows.Forms.Timer
    Private retryCount As Integer = 0
    Private isConnected As Boolean = False
    Private isClosing As Boolean = False

    ' 电机序号映射
    Private Const MOTOR_THUMB As Byte = &H1   ' 拇指
    Private Const MOTOR_INDEX As Byte = &H2    ' 食指
    Private Const MOTOR_MIDDLE As Byte = &H3   ' 中指
    Private Const MOTOR_RING As Byte = &H4     ' 无名指
    Private Const MOTOR_PINKY As Byte = &H5    ' 小指
    Private Const MOTOR_ELBOW As Byte = &H6    ' 手肘

    ' 上次的值（用于避免重复发送）
    Private lastValues As New Dictionary(Of Byte, Integer)

    Private Sub HandControlForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 创建网络工具实例（根据当前选择的手：左/右）
        networkUtils = New NetworkUtils(HandSelect.SelectedHand)
        AddHandler networkUtils.OnSuccess, AddressOf OnSuccess
        AddHandler networkUtils.OnFailure, AddressOf OnFailure

        ' 创建重试定时器
        retryTimer = New System.Windows.Forms.Timer()
        retryTimer.Interval = RETRY_TIMEOUT
        AddHandler retryTimer.Tick, AddressOf RetryTimer_Tick

        ' 初始化上次值
        InitializeLastValues()

        ' 绑定滑块事件（仅在松开时发送）
        AddHandler sliderThumb.MouseUp, AddressOf Slider_MouseUp
        AddHandler sliderIndex.MouseUp, AddressOf Slider_MouseUp
        AddHandler sliderMiddle.MouseUp, AddressOf Slider_MouseUp
        AddHandler sliderRing.MouseUp, AddressOf Slider_MouseUp
        AddHandler sliderPinky.MouseUp, AddressOf Slider_MouseUp
        AddHandler sliderElbow.MouseUp, AddressOf Slider_MouseUp

        ' 连接并发送进入手动控制模式指令
        ConnectAndSendEnterCommand()
    End Sub

    ''' <summary>
    ''' 初始化上次值
    ''' </summary>
    Private Sub InitializeLastValues()
        lastValues(MOTOR_THUMB) = 0
        lastValues(MOTOR_INDEX) = 0
        lastValues(MOTOR_MIDDLE) = 0
        lastValues(MOTOR_RING) = 0
        lastValues(MOTOR_PINKY) = 0
        lastValues(MOTOR_ELBOW) = 0
    End Sub

    ''' <summary>
    ''' 连接并发送进入手动控制模式指令
    ''' 指令格式: AA BB 02 CC
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
    ''' 发送进入手动控制模式指令
    ''' </summary>
    Private Async Function SendEnterCommand() As Threading.Tasks.Task
        Dim command As Byte() = {&HAA, &HBB, &H2, &HCC}
        LogMessage("发送进入手动控制模式指令: " & NetworkUtils.BytesToHex(command))
        retryTimer.Start()
        Await networkUtils.SendDataAsync(command)
    End Function

    ''' <summary>
    ''' 成功响应处理
    ''' </summary>
    Private Sub OnSuccess(response As Byte())
        Dim responseStr = NetworkUtils.BytesToString(response)
        LogMessage("收到响应: " & responseStr)

        ' 验证状态: "state manual\r\n"
        If responseStr.Contains("state manual") Then
            retryTimer.Stop()
            retryCount = 0
            LogMessage("成功进入手动控制模式")
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
    ''' 发送电机控制指令
    ''' 指令格式: AA BB [电机序号] [档位值] CC
    ''' </summary>
    ''' <param name="motorIndex">电机序号 (0x01~0x06)</param>
    ''' <param name="value">档位值 (手指1~30, 手肘1~20)</param>
    Private Async Sub SendMotorCommand(motorIndex As Byte, value As Integer)
        If Not isConnected Then Return

        ' 检查值是否变化
        If lastValues.ContainsKey(motorIndex) AndAlso lastValues(motorIndex) = value Then
            Return ' 值未变化，不发送
        End If

        ' 限制档位值范围
        Dim maxValue As Integer = If(motorIndex = MOTOR_ELBOW, 20, 30)
        Dim minValue As Integer = 1
        value = Math.Max(minValue, Math.Min(maxValue, value))

        ' 更新上次值
        lastValues(motorIndex) = value

        ' 构建指令
        Dim command As Byte() = {&HAA, &HBB, motorIndex, CByte(value), &HCC}
        Dim motorName = GetMotorName(motorIndex)
        LogMessage(motorName & " 档位" & value & ": " & NetworkUtils.BytesToHex(command))
        Await networkUtils.SendDataAsync(command)
    End Sub

    ''' <summary>
    ''' 获取电机名称
    ''' </summary>
    Private Function GetMotorName(motorIndex As Byte) As String
        Select Case motorIndex
            Case MOTOR_THUMB : Return "拇指"
            Case MOTOR_INDEX : Return "食指"
            Case MOTOR_MIDDLE : Return "中指"
            Case MOTOR_RING : Return "无名指"
            Case MOTOR_PINKY : Return "小指"
            Case MOTOR_ELBOW : Return "手肘"
            Case Else : Return "未知"
        End Select
    End Function

    ' 滑块松开事件（仅在松开时发送）
    Private Sub Slider_MouseUp(sender As Object, e As MouseEventArgs)
        Dim slider = DirectCast(sender, TrackBar)
        Dim motorIndex As Byte = GetMotorIndexFromSlider(slider)
        If motorIndex > 0 Then
            SendMotorCommand(motorIndex, slider.Value)
        End If
    End Sub

    ''' <summary>
    ''' 根据滑块获取电机序号
    ''' </summary>
    Private Function GetMotorIndexFromSlider(slider As TrackBar) As Byte
        If slider Is sliderThumb Then Return MOTOR_THUMB
        If slider Is sliderIndex Then Return MOTOR_INDEX
        If slider Is sliderMiddle Then Return MOTOR_MIDDLE
        If slider Is sliderRing Then Return MOTOR_RING
        If slider Is sliderPinky Then Return MOTOR_PINKY
        If slider Is sliderElbow Then Return MOTOR_ELBOW
        Return 0
    End Function

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
    ''' 窗体关闭时发送退出指令并清理资源
    ''' 指令格式: AA BB FF CC
    ''' </summary>
    Private Async Sub HandControlForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        isClosing = True
        If retryTimer IsNot Nothing Then retryTimer.Stop()
        If isConnected Then
            Dim exitCommand As Byte() = {&HAA, &HBB, &HFF, &HCC}
            LogMessage("发送退出手动控制模式指令: " & NetworkUtils.BytesToHex(exitCommand))
            Await networkUtils.SendDataAsync(exitCommand)
        End If
        If networkUtils IsNot Nothing Then networkUtils.Close()
    End Sub
End Class

