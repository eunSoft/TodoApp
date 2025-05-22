using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
namespace TodoApp.Services
{
    public static class DatabaseHelper
    {
        //const 써서 바꾸지 않을 값으로 설정해두기(파일경로,db이름,기본설정값,앱이름,버전정보)
        private const string DbFileName = "todos.db";// 실제로 파일로 저장될 sqlite db이름
        private const string ConnectionString = "Data Source=" + DbFileName + ";Version=3;";

        public static SQLiteConnection GetConnection()
        {
            if (!File.Exists(DbFileName))
            {
                SQLiteConnection.CreateFile(DbFileName);
                using (var conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string createTableSql = @"CREATE TABLE todos (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            content TEXT NOT NULL,
                                            is_done INTEGER NOT NULL DEFAULT 0,
                                            deadline TEXT
                                          )";
                    var cmd = new SQLiteCommand(createTableSql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            return new SQLiteConnection(ConnectionString);
        }
    }
}
