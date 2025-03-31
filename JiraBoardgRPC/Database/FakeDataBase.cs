namespace JiraBoardgRPC.FakeDataBase
{
    /// <summary>
    /// This is a FakeDataBase. It will be used as a test data. Later proper MongoDB database connection should be established. List of Tasks is AI generated.
    /// </summary>
    public class FakeDatabase : IDataBaseService
    {
        public static List<JiraTask> JiraTasks { get; } = new List<JiraTask>
        {
            new JiraTask {
                Id = 1, Type = JiraTaskType.Task, ReporterID = 101, AssigneeID = 201,
                Summary = "Implement login feature", Description = "Create authentication module",
                Priority = JiraTaskPriority.High, TaskStatus = JiraTaskStatus.InDevelopment,
                ClosingMessage = "", SubTasksIds = { 2, 3 },
                Comments = { new Comment { Value = "Initial setup done", AuthorId = 101, CommentDate = DateTime.UtcNow.ToString() } }
            },
            new JiraTask {
                Id = 2, Type = JiraTaskType.SubTask, ReporterID = 101, AssigneeID = 202,
                Summary = "Database schema for auth", Description = "Design tables for user authentication",
                Priority = JiraTaskPriority.Medium, TaskStatus = JiraTaskStatus.Completed,
                ClosingMessage = "Schema approved", SubTasksIds = {},
                Comments = { new Comment { Value = "Schema created", AuthorId = 102, CommentDate = DateTime.UtcNow.ToString() } }
            },
            new JiraTask {
                Id = 3, Type = JiraTaskType.SubTask, ReporterID = 101, AssigneeID = 203,
                Summary = "JWT implementation", Description = "Setup JWT authentication flow",
                Priority = JiraTaskPriority.High, TaskStatus = JiraTaskStatus.Resolved,
                ClosingMessage = "JWT auth working", SubTasksIds = {},
                Comments = { new Comment { Value = "JWT added", AuthorId = 103, CommentDate = DateTime.UtcNow.ToString() } }
            },
            new JiraTask {
                Id = 4, Type = JiraTaskType.Bug, ReporterID = 104, AssigneeID = 204,
                Summary = "Fix login redirect", Description = "Redirection issue after login",
                Priority = JiraTaskPriority.High, TaskStatus = JiraTaskStatus.InTest,
                ClosingMessage = "", SubTasksIds = {},
                Comments = { new Comment { Value = "Issue reproduced", AuthorId = 104, CommentDate = DateTime.UtcNow.ToString() } }
            },
            new JiraTask {
                Id = 5, Type = JiraTaskType.Story, ReporterID = 105, AssigneeID = 205,
                Summary = "User profile feature", Description = "Allow users to update profile info",
                Priority = JiraTaskPriority.Medium, TaskStatus = JiraTaskStatus.New,
                ClosingMessage = "", SubTasksIds = {},
                Comments = { new Comment { Value = "Feature planned", AuthorId = 105, CommentDate = DateTime.UtcNow.ToString() } }
            }
        };

        public void AddTask(JiraTask task)
        {
            JiraTasks.Add(task);
        }

        public bool DeleteTask(int id)
        {
            var task = JiraTasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                JiraTasks.Remove(task);
                return true;
            }
            return false;
        }

        public bool UpdateTask(JiraTask updatedTask)
        {
            var task = JiraTasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (task != null)
            {
                task.Type = updatedTask.Type;
                task.ReporterID = updatedTask.ReporterID;
                task.AssigneeID = updatedTask.AssigneeID;
                task.Summary = updatedTask.Summary;
                task.Description = updatedTask.Description;
                task.Priority = updatedTask.Priority;
                task.TaskStatus = updatedTask.TaskStatus;
                task.ClosingMessage = updatedTask.ClosingMessage;
                task.SubTasksIds.Clear();
                task.SubTasksIds.AddRange(updatedTask.SubTasksIds);
                task.Comments.Clear();
                task.Comments.AddRange(updatedTask.Comments);

                return true;
            }
            return false;
        }

        public JiraTask? GetTaskById(int id)
        {
            return JiraTasks.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<JiraTask> GetAllTasks()
        {
            return JiraTasks;
        }

        public int GetLastId()
        {
            return JiraTasks.Last().Id;
        }
    }
}
