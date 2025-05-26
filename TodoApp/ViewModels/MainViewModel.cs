using CommunityToolkit.Mvvm.ComponentModel;   // ObservableObject
using CommunityToolkit.Mvvm.Input;           // RelayCommand
using System;
using System.Collections.ObjectModel;
using TodoApp.Models;                        // Todo 모델
using TodoApp.Services;                      // ITodoService

namespace TodoApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ItdoService _service;

        public MainViewModel(ItdoService service)
        {
            _service = service;
            LoadTodosCommand.Execute(null);
        }

        // 입력 컨트롤 바인딩
        private string _content = "";
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);

        }

        private DateTime? _deadline;
        public DateTime? Deadline
        {
            get => _deadline;
            set => SetProperty(ref _deadline, value);
        }

        private bool _showCompleted;
        public bool ShowCompleted
        {
            get => _showCompleted;
            set
            {
                if (SetProperty(ref _showCompleted, value)) { LoadTodosCommand.Execute(null);
                }
            }

        }
        //목록 바인딩
        public ObservableCollection<Todo> Todos { get; } = new();
        // 전체 조회
        [RelayCommand]
        private void LoadTodos()
        {
            Todos.Clear();
            var items = _service.GetTodos(ShowCompleted);
            foreach (var t in items)
                Todos.Add(t);
        }

        // 추가
        [RelayCommand(CanExecute = nameof(CanAdd))]
        private void AddTodo()
        {
            _service.AddTodo(new Todo
            {
                Content = Content,
                Deadline = Deadline
            });
            Content = "";     // 입력 초기화
            Deadline = null;
            LoadTodosCommand.Execute(null);
        }
        private bool CanAdd() =>
            !string.IsNullOrWhiteSpace(Content) && Deadline.HasValue;

        // 삭제
        [RelayCommand]
        private void DeleteTodo(Todo todo)
        {
            _service.DeleteTodo(todo.Id);
            LoadTodosCommand.Execute(null);
        }

        // 완료 토글
        [RelayCommand]
        private void ToggleDone(Todo todo)
        {
            _service.ToggleDone(todo.Id, !todo.IsDone);
            LoadTodosCommand.Execute(null);
        }
    }
}