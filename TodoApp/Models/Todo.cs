namespace TodoApp.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsDone { get; set; }
        public DateTime? Deadline { get; set; }

        //public string DisplayText => $"{Content} {(Deadline.HasValue ? $"(~{Deadline:yyyy-MM-dd})" : "")}";
        public string DeadlineDisplay => Deadline?.ToString("yyyy-MM-dd") ?? "";
    }
}
