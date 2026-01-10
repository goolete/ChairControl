''' <summary>
''' 左右手选择管理类
''' 注意：避免与 VB 关键字 Select 冲突，因此类名为 HandSelect。
''' </summary>
Public Class HandSelect
    Private Shared _selectedHand As Boolean = False ' False=左手, True=右手

    ''' <summary>
    ''' 获取或设置当前选择的手（False=左手, True=右手）
    ''' </summary>
    Public Shared Property SelectedHand As Boolean
        Get
            Return _selectedHand
        End Get
        Set(value As Boolean)
            _selectedHand = value
            ' 保存到设置（用户级设置）
            My.Settings.select_hand = value
            My.Settings.Save()
        End Set
    End Property

    ''' <summary>
    ''' 初始化，从设置中加载
    ''' </summary>
    Public Shared Sub Initialize()
        Try
            _selectedHand = My.Settings.select_hand
        Catch
            _selectedHand = False ' 默认左手
        End Try
    End Sub
End Class

