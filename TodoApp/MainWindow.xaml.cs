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
            string deadline = selectedDate.Value.ToString("yyyy-mm-dd");

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
        {
            lstTodos.Items.Clear();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT id, content, is_done, deadline FROM todos WHERE is_done = 0 ORDER BY id DESC";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string content = reader["content"].ToString();
                        string deadline = reader["deadline"].ToString();
                        lstTodos.Items.Add($"{content} {(string.IsNullOrEmpty(deadline) ? "" : $"(~{deadline})")}");
                    }
                }
            }
        }

        private void DeleteTodo_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = lstTodos.SelectedIndex;
            if (selectedIndex == -1) return;
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM todos WHERE rowid = (SELECT id FROM todos ORDER BY id DESC LIMIT 1 OFFSET @offset)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@offset", selectedIndex);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadTodos();
        }
    }
}
