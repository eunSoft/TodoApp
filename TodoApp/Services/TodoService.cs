using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class TodoService : ItdoService
    {
        public IEnumerable<Todo> GetTodos(bool includeDone)
        {
            var list = new List<Todo>();
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            string sql = includeDone
                ? "SELECT id, content, is_done, deadline FROM todos ORDER BY id DESC"
                : "SELECT id, content, is_done, deadline FROM todos WHERE is_done = 0 ORDER BY id DESC";

            using var cmd = new SQLiteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Todo
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Content = reader["content"].ToString()!,
                    IsDone = Convert.ToInt32(reader["is_done"]) == 1,
                    Deadline = DateTime.TryParse(reader["deadline"].ToString(), out var d) ? d : null
                });
            }

            return list;
        }

        public void AddTodo(Todo todo)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            const string sql = "INSERT INTO todos (content, is_done, deadline) VALUES (@content, 0, @deadline)";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@content", todo.Content);
            cmd.Parameters.AddWithValue("@deadline", todo.Deadline?.ToString("yyyy-MM-dd") ?? "");
            cmd.ExecuteNonQuery();
        }

        public void DeleteTodo(int id)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            const string sql = "DELETE FROM todos WHERE id = @id";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public void ToggleDone(int id, bool done)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            const string sql = "UPDATE todos SET is_done = @done WHERE id = @id";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@done", done ? 1 : 0);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
