using Grpc.Core;
using Grpc.Net.Client;
using GrpcTests.IntegrationTests.Helpers;
using JiraBoardgRPC;
using Xunit;

namespace GrpcTests.IntegrationTests.GrpcService
{
    public class JiraBoardTests : IClassFixture<TestBaseClass>
    {
        private JiraBoard.JiraBoardClient _sut;

        private readonly GrpcChannel _channel;

        public JiraBoardTests(TestBaseClass fixture)
        {
            _channel = GrpcChannel.ForAddress(Constants.GrpcServerUrlAdress);
            _sut = new JiraBoard.JiraBoardClient(_channel);
        }

        [Fact]
        public async Task GetTask_ShouldReturnTask_WhenTaskExists()
        {
            // arrange 
            var taskId = new TaskID { TaskId = 1 };

            // act
            var response = await _sut.GetTaskAsync(taskId);

            // assert
            Assert.NotNull(response);
            Assert.Equal(1, response.Id);
        }

        [Fact]
        public async Task GetTask_ShouldThrowRpcException_WhenTaskDoesNotExist()
        {
            // arrange 
            var taskId = new TaskID { TaskId = 999 };

            // act & assert
            var exception = await Assert.ThrowsAsync<RpcException>(async () => await _sut.GetTaskAsync(taskId));

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Equal("Task with ID 999 not found", exception.Status.Detail);
        }


        [Fact]
        public async Task GetALlTasks_ShoudlReturnAllTasks()
        {
            // arrange
            List<JiraTask> tasks = new List<JiraTask>();

            // act 
            using (var call = _sut.GetAllTasks(new EmptyRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentTask = call.ResponseStream.Current;
                    tasks.Add(currentTask);
                }
            }

            // assert
            Assert.NotEmpty(tasks);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnSuccess_WhenTaskIsCreated()
        {
            // arrange
            var newTask = GetNewJiraTask();

            // act
            var response = await _sut.CreateTaskAsync(newTask);

            // assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.NotEqual("0", response.Message);

            // clean up
            await _sut.DeleteTaskAsync(new TaskID { TaskId = int.Parse(response.Message) });
        }

        [Fact]
        public async Task UpdateTask_ShouldReturnSuccess_WhenTaskIsUpdated()
        {
            // arrange
            var updateTask = GetJiraTask();

            // act
            var response = await _sut.UpdateTaskAsync(updateTask);

            // assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("Task updated successfully", response.Message);
        }


        [Fact]
        public async Task DeleteTask_ShouldReturnSuccess_WhenTaskIsDeleted()
        {
            // arrange
            var newTask = GetNewJiraTask();
            var createTaskResponse = await _sut.CreateTaskAsync(newTask);

            // act
            Response response = await _sut.DeleteTaskAsync(new TaskID { TaskId = int.Parse(createTaskResponse.Message) });

            // assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal("Task deleted successfully", response.Message);
        }

        private NewJiraTask GetNewJiraTask()
        {
            return new NewJiraTask
            {
                Type = JiraTaskType.Issue,
                ReporterID = 1,
                AssigneeID = 2,
                Summary = "Task Summary",
                Description = "Task Description",
                Priority = JiraTaskPriority.High,
                TaskStatus = JiraTaskStatus.New,
                ClosingMessage = "Closing message",
                SubTasksIds = { 101, 102 },
                Comments = {
                    new Comment
                    {
                        Value = "This is a comment",
                        AuthorId = 3,
                        CommentDate = DateTime.Now.ToString("yyyy-MM-dd")
                    }
                }
            };
        }

        private JiraTask GetJiraTask()
        {
            return new JiraTask
            {
                Id = 1,
                Type = JiraTaskType.Issue,
                ReporterID = 1,
                AssigneeID = 2,
                Summary = "Task Summary",
                Description = "Task Description",
                Priority = JiraTaskPriority.High,
                TaskStatus = JiraTaskStatus.New,
                ClosingMessage = "Closing message",
                SubTasksIds = { 101, 102 },
                Comments = {
                    new Comment
                    {
                        Value = "This is a comment",
                        AuthorId = 3,
                        CommentDate = DateTime.Now.ToString("yyyy-MM-dd")
                    }
                }
            };
        }
    }
}
