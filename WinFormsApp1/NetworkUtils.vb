Imports System.Net.Sockets
Imports System.Text
Imports System.Threading.Tasks

''' <summary>
''' 网络通信工具类，用于与机械手进行TCP通信
''' </summary>
Public Class NetworkUtils
    Private Const SERVER_IP As String = "192.168.4.1"
    Private Const LEFT_HAND_PORT As Integer = 3001
    Private Const RIGHT_HAND_PORT As Integer = 4001
    Private Const SOCKET_TIMEOUT As Integer = 600 ' 毫秒

    Private client As TcpClient
    Private stream As NetworkStream
    Private isConnected As Boolean = False
    Private isLeftHand As Boolean

    ' 事件：发送成功 / 失败
    Public Event OnSuccess(response As Byte())
    Public Event OnFailure(errorMessage As String)

    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="selectHand">false=左手(3001), true=右手(4001)</param>
    Public Sub New(selectHand As Boolean)
        ' selectHand=false 表示左手 → 使用端口3001
        isLeftHand = (selectHand = False)
    End Sub

    ''' <summary>
    ''' 异步建立连接
    ''' </summary>
    Public Async Function ConnectAsync() As Task(Of Boolean)
        Try
            Dim port As Integer = If(isLeftHand, LEFT_HAND_PORT, RIGHT_HAND_PORT)
            client = New TcpClient()
            client.ReceiveTimeout = SOCKET_TIMEOUT
            client.SendTimeout = SOCKET_TIMEOUT

            Await client.ConnectAsync(SERVER_IP, port)
            stream = client.GetStream()
            isConnected = True
            Return True
        Catch ex As Exception
            isConnected = False
            RaiseEvent OnFailure($"连接失败: {ex.Message}")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 发送数据并接收响应
    ''' </summary>
    ''' <param name="data">要发送的字节数组</param>
    Public Async Function SendDataAsync(data As Byte()) As Task(Of Boolean)
        If Not isConnected OrElse stream Is Nothing Then
            RaiseEvent OnFailure("未连接到服务器")
            Return False
        End If

        Try
            ' 发送数据
            Await stream.WriteAsync(data, 0, data.Length)
            Await stream.FlushAsync()

            ' 读取响应（最多等待600ms）
            Dim buffer(1023) As Byte
            Dim bytesRead As Integer = 0

            ' 设置读取超时
            Dim readTask = stream.ReadAsync(buffer, 0, buffer.Length)
            Dim timeoutTask = Task.Delay(SOCKET_TIMEOUT)

            Dim completedTask = Await Task.WhenAny(readTask, timeoutTask)
            If completedTask Is readTask Then
                bytesRead = Await readTask
            Else
                RaiseEvent OnFailure("读取响应超时")
                Return False
            End If

            If bytesRead > 0 Then
                Dim response(bytesRead - 1) As Byte
                Array.Copy(buffer, response, bytesRead)
                RaiseEvent OnSuccess(response)
                Return True
            Else
                RaiseEvent OnFailure("未收到响应")
                Return False
            End If

        Catch ex As Exception
            RaiseEvent OnFailure("发送数据失败: " & ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 关闭连接
    ''' </summary>
    Public Sub Close()
        Try
            If stream IsNot Nothing Then
                stream.Close()
                stream = Nothing
            End If
            If client IsNot Nothing Then
                client.Close()
                client = Nothing
            End If
            isConnected = False
        Catch ex As Exception
            ' 忽略关闭时的错误
        End Try
    End Sub

    ''' <summary>
    ''' 检查连接状态
    ''' </summary>
    Public ReadOnly Property IsConnectedStatus As Boolean
        Get
            Return isConnected AndAlso client IsNot Nothing AndAlso client.Connected
        End Get
    End Property

    ''' <summary>
    ''' 将字节数组转换为十六进制字符串（用于日志）
    ''' </summary>
    Public Shared Function BytesToHex(bytes As Byte()) As String
        Return String.Join(" ", bytes.Select(Function(b) b.ToString("X2")))
    End Function

    ''' <summary>
    ''' 将字节数组转换为UTF-8字符串
    ''' </summary>
    Public Shared Function BytesToString(bytes As Byte()) As String
        Return Encoding.UTF8.GetString(bytes)
    End Function
End Class

