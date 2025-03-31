namespace MiddleStepService.Models
{
    using JiraBoardgRPC;

    public class Deserializer
    {
        public static DeserializedJiraTask DeserializeJiraTask(JiraTask grpcTask)
        {
            return new DeserializedJiraTask
            {
                Id = grpcTask.Id,
                Type = (JiraTaskType)grpcTask.Type,
                ReporterID = grpcTask.ReporterID,
                AssigneeID = grpcTask.AssigneeID,
                Summary = grpcTask.Summary,
                Description = grpcTask.Description,
                Priority = (JiraTaskPriority)grpcTask.Priority,
                Comments = grpcTask.Comments.Select(c => new Comment
                {
                    Value = c.Value,
                    AuthorId = c.AuthorId,
                    CommentDate = c.CommentDate
                }).ToList(),
                TaskStatus = (JiraTaskStatus)grpcTask.TaskStatus,
                ClosingMessage = grpcTask.ClosingMessage,
                SubTasksIds = grpcTask.SubTasksIds.ToList()
            };
        }

        public static DeserializedNewJiraTask DeserializeNewJiraTask(NewJiraTask grpcNewTask)
        {
            return new DeserializedNewJiraTask
            {
                Type = (JiraTaskType)grpcNewTask.Type,
                ReporterID = grpcNewTask.ReporterID,
                AssigneeID = grpcNewTask.AssigneeID,
                Summary = grpcNewTask.Summary,
                Description = grpcNewTask.Description,
                Priority = (JiraTaskPriority)grpcNewTask.Priority,
                Comments = grpcNewTask.Comments.Select(c => new Comment
                {
                    Value = c.Value,
                    AuthorId = c.AuthorId,
                    CommentDate = c.CommentDate
                }).ToList(),
                TaskStatus = (JiraTaskStatus)grpcNewTask.TaskStatus,
                ClosingMessage = grpcNewTask.ClosingMessage,
                SubTasksIds = grpcNewTask.SubTasksIds.ToList()
            };
        }
    }
}
