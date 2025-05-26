using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Services
{
    public interface ItdoService
    {
        IEnumerable<Todo> GetTodos(bool includeDone);
        void AddTodo(Todo todo);
        void DeleteTodo(int id);
        void ToggleDone(int id, bool done);
    }
}
