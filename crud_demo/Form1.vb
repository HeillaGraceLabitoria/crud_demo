Imports MySql.Data.MySqlClient
Public Class Form1
    Dim conn As MySqlConnection
    Dim COMMAND As MySqlCommand
    Private Sub ButtonConnect_Click(sender As Object, e As EventArgs) Handles ButtonConnect.Click
        conn = New MySqlConnection
        conn.ConnectionString = "server=localhost; userid=root; password=root; database=crud_demo_db;"


        Try
            conn.Open()
            MessageBox.Show("Connected")

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            conn.Close()
        End Try
    End Sub

    Private Sub BtnInsert_Click(sender As Object, e As EventArgs) Handles BtnInsert.Click
        Dim query As String = "INSERT  INTo students_tbl (name, age, email) VALUES (@name, @age, @email)"
        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=crud_demo_db;")
                conn.Open()
                Using cmd As New MySqlCommand(query, conn) 'may gustong ilagay
                    cmd.Parameters.AddWithValue("@name", txtName.Text)
                    cmd.Parameters.AddWithValue("@age", CInt(txtAge.Text))
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Record insert succesfully!")

                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnRead_Click(sender As Object, e As EventArgs) Handles BtnRead.Click
        Dim query As String = "SELECT name, age, email FROM crud_demo_db.students_tbl;" 'lahat ng column *
        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=crud_demo_db;")
                Dim adapter As New MySqlDataAdapter(query, conn) 'may gustong kunin
                Dim table As New DataTable() 'table object
                adapter.Fill(table) 'from adapter to table
                DataGridView1.DataSource = table 'Display to DataGridView
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        Dim newAge As Integer

        If String.IsNullOrWhiteSpace(txtAge.Text) OrElse Not Integer.TryParse(txtAge.Text, newAge) Then
            MessageBox.Show("Please enter a valid number for the Age.", "Input Error")
            txtAge.Focus()
            Return
        End If


        Dim query As String = "UPDATE students_tbl SET name = @newName, age = @age, email = @email WHERE name = @nameToFind"

        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=crud_demo_db;")
                conn.Open()

                Using cmd As New MySqlCommand(query, conn)

                    cmd.Parameters.AddWithValue("@nameToFind", txtName.Text)
                    cmd.Parameters.AddWithValue("@newName", txtName.Text)
                    cmd.Parameters.AddWithValue("@age", newAge)
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text)

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show(rowsAffected & " record(s) updated successfully!")

                        txtName.Clear()
                        txtAge.Clear()
                        txtEmail.Clear()
                        Call BtnRead_Click(Nothing, Nothing)
                    Else
                        MessageBox.Show("Update failed: No record found with that name.", "Update Status")
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If String.IsNullOrWhiteSpace(txtName.Text) Then
            MessageBox.Show("Please enter the name of the record to delete.", "Selection Required")
            Return
        End If

        If MessageBox.Show("Are you sure you want to delete the record(s) for " & txtName.Text & "?", "Confirm Delete", MessageBoxButtons.YesNo) = DialogResult.No Then
            Return
        End If

        Dim query As String = "DELETE FROM students_tbl WHERE name = @nameToDelete"


        Try
            Using conn As New MySqlConnection("server=localhost; userid=root; password=root; database=crud_demo_db;")
                conn.Open()

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@nameToDelete", txtName.Text)

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show(rowsAffected & " record(s) deleted successfully!")

                        txtName.Clear()
                        txtAge.Clear()
                        txtEmail.Clear()

                        Call BtnRead_Click(Nothing, Nothing)
                    Else
                        MessageBox.Show("Delete failed: No record found with that name.", "Delete Status")
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Database Error: " & ex.Message)
        End Try
    End Sub
End Class

