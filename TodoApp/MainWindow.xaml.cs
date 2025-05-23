using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp
{
    public partial class MainWindow : Window
    {
        private List<Todo> todos = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadTodos();
        }

        private void AddTodo_Click(object sender, RoutedEventArgs e)
        {
            string content = txtContent.Text.Trim();
            var selectedDate = dpDeadline.SelectedDate;

            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("할 일을 입력해주세요!", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedDate == null)
            {
                MessageBox.Show("날짜를 선택해주세요!", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string deadline = selectedDate.Value.ToString("yyyy-MM-dd");

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO todos (content, is_done, deadline) VALUES (@content, 0, @deadline)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.Parameters.AddWithValue("@deadline", deadline);
                    cmd.ExecuteNonQuery();
                }
            }

            txtContent.Text = "";
            dpDeadline.SelectedDate = null;
            LoadTodos();
        }

        private void LoadTodos()
        {   //db에서 할일 목록 가져오기
            todos.Clear();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT id, content, is_done, deadline FROM todos ORDER BY id DESC";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        todos.Add(new Todo
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Content = reader["content"].ToString()!,
                            IsDone = Convert.ToInt32(reader["is_done"]) == 1,
                            Deadline = DateTime.TryParse(reader["deadline"].ToString(), out var d) ? d : null
                        });
                    }
                }
            }

            lstTodos.ItemsSource = null;
            lstTodos.ItemsSource = todos;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Todo todo)
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string sql = "DELETE FROM todos WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", todo.Id);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadTodos();
            }
        }

        private void Todo_Checked(object sender, RoutedEventArgs e) => ToggleDone(sender, true);
        private void Todo_Unchecked(object sender, RoutedEventArgs e) => ToggleDone(sender, false);

        private void ToggleDone(object sender, bool done)
        {
            if (sender is CheckBox chk && chk.DataContext is Todo todo)
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string sql = "UPDATE todos SET is_done = @done WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@done", done ? 1 : 0);
                        cmd.Parameters.AddWithValue("@id", todo.Id);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadTodos();
            }
        }
    }
}


