Imports System.IO

Public Class MainWindow
    Private Sub BtnSelectFolder_Click(sender As Object, e As RoutedEventArgs)
        Dim windowHandle = New Interop.WindowInteropHelper(Me).Handle
        Dim selectedPath As String = FolderPickerHelper.SelectFolder(windowHandle, "Please select a folder", allowNewFolderButton:=False, initialFolder:=Environment.SpecialFolder.MyComputer)
        If Not String.IsNullOrEmpty(selectedPath) Then
            TxtFolder.Text = selectedPath
            ProcessFiles(selectedPath)
        End If
    End Sub

    Private Sub ProcessFiles(SelectedFolder As String)
        LstResult.Items.Clear()

        If Not Directory.Exists(SelectedFolder) Then
            MessageBox.Show($"Folder  {SelectedFolder}  does not exist!")
            Return
        End If

        ' دریافت همه فایل‌ها
        Dim files = Directory.GetFiles(SelectedFolder)

        ' فقط اسم فایل‌ها بدون مسیر
        Dim fileNames = files.Select(Function(f) Path.GetFileName(f)).ToList()

        For Each fileName In fileNames

            ' نام فایل بدون پسوند
            Dim FileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName)
            Dim ext = Path.GetExtension(fileName)

            If FileNameWithoutExt.EndsWith("-Path") Then Continue For

            Dim IsExist As Boolean = False

            For Each fileNamePath In fileNames
                Dim FileNamePathWithoutExt = Path.GetFileNameWithoutExtension(fileNamePath)
                If Not FileNamePathWithoutExt.EndsWith("-Path") Then Continue For
                If $"{FileNameWithoutExt}-Path" = FileNamePathWithoutExt Then
                    IsExist = True
                    Exit For
                End If
            Next fileNamePath

            If Not IsExist Then LstResult.Items.Add(fileName)

        Next fileName

        If LstResult.Items.Count = 0 Then LstResult.Items.Add("✅ All files in this folder end with '-Path'.")

    End Sub

End Class
