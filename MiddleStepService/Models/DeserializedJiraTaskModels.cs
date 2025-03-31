namespace MiddleStepService.Models
{
    public class TaskID
    {
        public int TaskId { get; set; }
    }

    public class EmptyRequest { }

    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class DeserializedNewJiraTask
    {
        public JiraTaskType Type { get; set; }
        public int ReporterID { get; set; }
        public int AssigneeID { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public JiraTaskPriority Priority { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public JiraTaskStatus TaskStatus { get; set; }
        public string ClosingMessage { get; set; }
        public List<int> SubTasksIds { get; set; } = new();
    }

    public class DeserializedJiraTask
    {
        public int Id { get; set; }
        public JiraTaskType Type { get; set; }
        public int ReporterID { get; set; }
        public int AssigneeID { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public JiraTaskPriority Priority { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public JiraTaskStatus TaskStatus { get; set; }
        public string ClosingMessage { get; set; }
        public List<int> SubTasksIds { get; set; } = new();
    }

    public class Comment
    {
        public string Value { get; set; }
        public int AuthorId { get; set; }
        public string CommentDate { get; set; }
    }

    public enum JiraTaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public enum JiraTaskStatus
    {
        New = 0,
        InDevelopment = 1,
        InTest = 2,
        InReview = 3,
        Resolved = 4,
        Completed = 5
    }

    public enum JiraTaskType
    {
        Undefined = 0,
        Issue = 1,
        Epic = 2,
        Story = 3,
        Spike = 4,
        Bug = 5,
        Task = 6,
        SubTask = 7
    }
}