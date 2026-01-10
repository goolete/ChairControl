Imports System.Threading
Imports System.Threading.Tasks
Imports System.IO

''' <summary>
''' 设置模式窗体 (SettingActivity)
''' </summary>
Public Class SettingForm
    Private Const MAX_RETRIES As Integer = 1
    Private Const RETRY_TIMEOUT As Integer = 800 ' 毫秒

    Private networkUtils As NetworkUtils
    Private retryTimer As System.Windows.Forms.Timer
    Private retryCount As Integer = 0
    Private isConnected As Boolean = False
    Private isClosing As Boolean = False

    ' 电机序号映射
    Private Const MOTOR_THUMB As Byte = &H1
    Private Const MOTOR_INDEX As Byte = &H2
    Private Const MOTOR_MIDDLE As Byte = &H3
    Private Const MOTOR_RING As Byte = &H4
    Private Const MOTOR_PINKY As Byte = &H5
    Private Const MOTOR_ELBOW As Byte = &H6

    ' 设置类型
    Private Const SETTING_LOWER As Byte = &H1 ' 下限值
    Private Const SETTING_UPPER As Byte = &H2 ' 上限值

    ' 上次的值（用于避免重复发送）
    Private lastLowerValues As New Dictionary(Of Byte, Integer)
    Private lastUpperValues As New Dictionary(Of Byte, Integer)

    Private Sub SettingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        ' 加载保存的设置值
        LoadSavedValues()

        ' 连接并发送进入设置模式指令
        ConnectAndSendEnterCommand()
    End Sub

    ''' <summary>
    ''' 初始化上次值
    ''' </summary>
    Private Sub InitializeLastValues()
        For Each motorIndex As Byte In New Byte() {MOTOR_THUMB, MOTOR_INDEX, MOTOR_MIDDLE, MOTOR_RING, MOTOR_PINKY, MOTOR_ELBOW}
            lastLowerValues(motorIndex) = 0
            lastUpperValues(motorIndex) = 0
        Next
    End Sub

    ''' <summary>
    ''' 从设置文件加载保存的值
    ''' </summary>
    Private Sub LoadSavedValues()
        ' 这里简化处理，实际应该从SharedPreferences加载
        ' VB.NET可以使用My.Settings或配置文件
        Try
            ' 示例：从My.Settings加载（需要先定义设置项）
            ' 这里暂时使用默认值
        Catch
            ' 忽略错误
        End Try
    End Sub

    ''' <summary>
    ''' 保存设置值到配置文件
    ''' </summary>
    Private Sub SaveValue(motorIndex As Byte, settingType As Byte, value As Integer)
        Dim suffix As String = If(settingType = SETTING_LOWER, "lowerValue", "upperValue")
        Dim key As String = HandSelect.SelectedHand.ToString() & "_" & motorIndex.ToString() & "_" & suffix
        ' 这里简化处理，实际应该保存到My.Settings或配置文件
        ' My.Settings(key) = value
        ' My.Settings.Save()
    End Sub

    ''' <summary>
    ''' 连接并发送进入设置模式指令
    ''' 指令格式: AA BB 04 CC
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
    ''' 发送进入设置模式指令
    ''' </summary>
    Private Async Function SendEnterCommand() As Threading.Tasks.Task
        Dim command As Byte() = {&HAA, &HBB, &H4, &HCC}
        LogMessage("发送进入设置模式指令: " & NetworkUtils.BytesToHex(command))
        retryTimer.Start()
        Await networkUtils.SendDataAsync(command)
    End Function

    ''' <summary>
    ''' 成功响应处理
    ''' </summary>
    Private Sub OnSuccess(response As Byte())
        Dim responseStr = NetworkUtils.BytesToString(response)
        LogMessage("收到响应: " & responseStr)

        ' 验证状态: "state set\r\n"
        If responseStr.Contains("state set") Then
            retryTimer.Stop()
            retryCount = 0
            LogMessage("成功进入设置模式")
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
    ''' 发送电机范围设置指令
    ''' 指令格式: AA BB [电机序号] [设置类型] [设置值] CC
    ''' </summary>
    ''' <param name="motorIndex">电机序号</param>
    ''' <param name="settingType">设置类型 (0x01=下限, 0x02=上限)</param>
    ''' <param name="value">设置值</param>
    Private Async Sub SendRangeSettingCommand(motorIndex As Byte, settingType As Byte, value As Integer)
        If Not isConnected Then Return

        ' 检查值是否变化
        Dim lastValues = If(settingType = SETTING_LOWER, lastLowerValues, lastUpperValues)
        If lastValues.ContainsKey(motorIndex) AndAlso lastValues(motorIndex) = value Then
            Return ' 值未变化，不发送
        End If

        ' 限制值范围
        Dim maxValue As Integer = If(motorIndex = MOTOR_ELBOW, 20, 30)
        Dim minValue As Integer = 1
        value = Math.Max(minValue, Math.Min(maxValue, value))

        ' 更新上次值
        lastValues(motorIndex) = value

        ' 保存到配置文件
        SaveValue(motorIndex, settingType, value)

        ' 构建指令
        Dim command As Byte() = {&HAA, &HBB, motorIndex, settingType, CByte(value), &HCC}
        Dim motorName = GetMotorName(motorIndex)
        Dim settingName = If(settingType = SETTING_LOWER, "下限", "上限")
        LogMessage(motorName & " " & settingName & value & ": " & NetworkUtils.BytesToHex(command))
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

    ''' <summary>
    ''' 范围滑块值改变事件（下限）
    ''' </summary>
    Private Sub RangeSlider_LowerValueChanged(sender As Object, e As EventArgs)
        Dim slider = DirectCast(sender, TrackBar)
        Dim motorIndex = GetMotorIndexFromSlider(slider)
        If motorIndex > 0 Then
            SendRangeSettingCommand(motorIndex, SETTING_LOWER, slider.Value)
        End If
    End Sub

    ''' <summary>
    ''' 范围滑块值改变事件（上限）
    ''' </summary>
    Private Sub RangeSlider_UpperValueChanged(sender As Object, e As EventArgs)
        Dim slider = DirectCast(sender, TrackBar)
        Dim motorIndex = GetMotorIndexFromSlider(slider)
        If motorIndex > 0 Then
            SendRangeSettingCommand(motorIndex, SETTING_UPPER, slider.Value)
        End If
    End Sub

    ''' <summary>
    ''' 根据滑块获取电机序号
    ''' </summary>
    Private Function GetMotorIndexFromSlider(slider As TrackBar) As Byte
        ' 这里简化处理，实际应该根据滑块名称或Tag属性判断
        ' 由于WinForms没有RangeSlider，使用两个TrackBar分别表示上下限
        ' 这里需要根据实际UI设计调整
        Return 0 ' 占位符
    End Function

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
    Private Async Sub SettingForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        isClosing = True
        If retryTimer IsNot Nothing Then retryTimer.Stop()
        If isConnected Then
            Dim exitCommand As Byte() = {&HAA, &HBB, &HFF, &HCC}
            LogMessage("发送退出设置模式指令: " & NetworkUtils.BytesToHex(exitCommand))
            Await networkUtils.SendDataAsync(exitCommand)
        End If
        If networkUtils IsNot Nothing Then networkUtils.Close()
    End Sub
End Class

