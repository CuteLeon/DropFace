Public Class Form1
    Private Const FacesImageCount = 390
    Dim DefaultIntervalTime As Integer = 10     '时间间隔
    Dim Random As New Random                        '随机数
    Dim FaceBitmap As Bitmap                         '位图
    Dim FaceGraphics As Graphics                    '画笔
    Dim FaceSize As Size = New Size(56, 56)
    Dim DesktopBitmap As Bitmap = New Bitmap(My.Computer.Screen.Bounds.Width, My.Computer.Screen.Bounds.Height)

    Private Structure Face
        Dim X As Integer
        Dim Y As Integer
        Dim Gravity As Integer
        Dim YVelocity As Integer
        Dim FaceImage As Bitmap
    End Structure
    Dim Faces As New ArrayList      '记录

    '更新
    Private Sub UpdateFace(ByRef Face As Face, ByVal IntervalTime As Integer)
        With Face
            .Y += (.YVelocity * IntervalTime / 1000.0F)
            .YVelocity += .Gravity * IntervalTime
        End With
    End Sub

    '发射
    Private Sub EmitSingleFace(ByVal X As Integer, ByVal FaceIndex As Integer, ByVal Gravity As Integer)
        Dim FaceDemo As Face
        With FaceDemo
            .X = X
            .Y = -FaceSize.Height
            .Gravity = Gravity
            .YVelocity = 0
            .FaceImage = My.Resources.FaceResource.ResourceManager.GetObject("Face_" & FaceIndex.ToString("000"))
        End With
        Faces.Add(FaceDemo)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.SetBounds(0, 0, My.Computer.Screen.Bounds.Width, My.Computer.Screen.Bounds.Height)
        Me.DoubleBuffered = True
        Timer1.Interval = DefaultIntervalTime
        Me.BackgroundImageLayout = ImageLayout.Stretch

        FaceGraphics = Graphics.FromImage(DesktopBitmap)
        FaceGraphics.CopyFromScreen(0, 0, 0, 0, My.Computer.Screen.Bounds.Size)
        FaceGraphics.Dispose()
        Me.BackgroundImage = DesktopBitmap
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        EmitSingleFace(Random.Next(My.Computer.Screen.Bounds.Width - FaceSize.Width + 1), Random.Next(FacesImageCount), Random.Next(10) + 1)

        If Faces.Count = 0 Then Exit Sub
        FaceBitmap = Nothing
        FaceBitmap = New Bitmap(DesktopBitmap, Me.Width, Me.Height)
        FaceGraphics = Graphics.FromImage(FaceBitmap)
        Dim Index As Integer = 0, FacesCount As Integer = Faces.Count
        Dim FaceInstance As Face
        Do Until Index = FacesCount
            UpdateFace(Faces.Item(Index), DefaultIntervalTime)
            FaceInstance = CType(Faces.Item(Index), Face)
            If FaceInstance.Y >= My.Computer.Screen.Bounds.Height Then
                FaceInstance = Nothing
                Faces.RemoveAt(Index)
                FacesCount -= 1
                Continue Do
            End If
            FaceGraphics.DrawImage(FaceInstance.FaceImage, FaceInstance.X, FaceInstance.Y)
            Index += 1
        Loop
        FaceGraphics.DrawString(FacesCount, Me.Font, Brushes.Red, 10, 10)
        Me.BackgroundImage = FaceBitmap
        FaceGraphics.Dispose()
    End Sub
End Class
