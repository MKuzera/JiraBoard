using Grpc.Core;
using JiraBoardgRPC.FakeDataBase;

namespace JiraBoardgRPC.Services
{
    public class JiraBoardService : JiraBoard.JiraBoardBase
    {
        private readonly ILogger<JiraBoardService> _logger;

        private readonly IDataBaseService _dataBaseService;

        public JiraBoardService(IDataBaseService dataBaseService, ILogger<JiraBoardService> logger)
        {
            _dataBaseService = dataBaseService;
            _logger = logger;
        }

        public override Task<Response> CreateTask(NewJiraTask jiraTaskRequest, ServerCallContext context)
        {
            int newId = _dataBaseService.GetLastId() + 1;
            JiraTask jiraTask = new JiraTask()
            {
                Id = newId,
                Summary = jiraTaskRequest.Summary,
                Description = jiraTaskRequest.Description,
                ReporterID = jiraTaskRequest.ReporterID,
                AssigneeID = jiraTaskRequest.AssigneeID,
                Priority = jiraTaskRequest.Priority,
                TaskStatus = jiraTaskRequest.TaskStatus,
                Type = jiraTaskRequest.Type,
                ClosingMessage = jiraTaskRequest.ClosingMessage,
                SubTasksIds = { jiraTaskRequest.SubTasksIds.ToArray() },
                Comments = { jiraTaskRequest.Comments.ToArray() }
            };

            _dataBaseService.AddTask(jiraTask);

            var response = new Response
            {
                Success = true,
                Message = $"{newId}"
            };

            return Task.FromResult(response);
        }

        public override Task<Response> DeleteTask(TaskID taskIdRequest, ServerCallContext context)
        {
            bool isDeleted = _dataBaseService.DeleteTask(taskIdRequest.TaskId);

            var response = new Response
            {
                Success = isDeleted,
                Message = isDeleted ? "Task deleted successfully" : "Task was not deleted",
            };

            return Task.FromResult(response);
        }

        public override async Task GetAllTasks(EmptyRequest emptyRequest, IServerStreamWriter<JiraTask> responseStream, ServerCallContext context)
        {
            foreach (JiraTask jiraTask in _dataBaseService.GetAllTasks())
            {
                await responseStream.WriteAsync(jiraTask);
            }
        }

        public override Task<JiraTask> GetTask(TaskID request, ServerCallContext context)
        {
            JiraTask? jiraTask = _dataBaseService.GetTaskById(request.TaskId);

            if (jiraTask == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Task with ID {request.TaskId} not found"));
            }

            return Task.FromResult(jiraTask);
        }

        public override Task<Response> UpdateTask(JiraTask request, ServerCallContext context)
        {
            bool isUpdated = _dataBaseService.UpdateTask(request);

            var response = new Response
            {
                Success = isUpdated,
                Message = isUpdated ? "Task updated successfully" : "Task was not updated",
            };

            return Task.FromResult(response);
        }
    }
}
