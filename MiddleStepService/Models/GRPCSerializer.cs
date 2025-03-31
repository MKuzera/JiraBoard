using JiraBoardgRPC;

namespace MiddleStepService.Models
{
    public class GRPCSerializer
    {
        public static JiraTask SerializeJiraTask(DeserializedJiraTask task)
        {
            var grpcTask = new JiraTask
            {
                Id = task.Id,
                Type = (JiraBoardgRPC.JiraTaskType)task.Type,
                ReporterID = task.ReporterID,
                AssigneeID = task.AssigneeID,
                Summary = task.Summary,
                Description = task.Description,
                Priority = (JiraBoardgRPC.JiraTaskPriority)task.Priority,
                TaskStatus = (JiraBoardgRPC.JiraTaskStatus)task.TaskStatus,
                ClosingMessage = task.ClosingMessage,
                SubTasksIds = { task.SubTasksIds }
            };

            grpcTask.Comments.AddRange(
                    task.Comments.Select(c => new JiraBoardgRPC.Comment
                    {
                        Value = c.Value,
                        AuthorId = c.AuthorId,
                        CommentDate = c.CommentDate
                    }).ToList());

            return grpcTask;
        }
        public static NewJiraTask SerializeNewJiraTask(DeserializedNewJiraTask task)
        {
            var grpcTask = new NewJiraTask
            {
                Type = (JiraBoardgRPC.JiraTaskType)task.Type,
                ReporterID = task.ReporterID,
                AssigneeID = task.AssigneeID,
                Summary = task.Summary,
                Description = task.Description,
                Priority = (JiraBoardgRPC.JiraTaskPriority)task.Priority,
                TaskStatus = (JiraBoardgRPC.JiraTaskStatus)task.TaskStatus,
                ClosingMessage = task.ClosingMessage,
                SubTasksIds = { task.SubTasksIds }
            };

            grpcTask.Comments.AddRange(
                task.Comments.Select(c => new JiraBoardgRPC.Comment
                {
                    Value = c.Value,
                    AuthorId = c.AuthorId,
                    CommentDate = c.CommentDate
                }).ToList());

            return grpcTask;
        }
    }
}
