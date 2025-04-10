﻿using GrpcTests.IntegrationTests.Helpers;
using MiddleStepService.Models;
using System.Net.Http.Json;
using Xunit;

namespace GrpcTests.IntegrationTests.MiddleStepService
{
    public class JiraBoardApiTests : IClassFixture<TestBaseClass>
    {
        private readonly HttpClient _sut;

        public JiraBoardApiTests(TestBaseClass testBase)
        {
            _sut = AuthenticationHelper.GetClient(Constants.MiddleStepApiAddress).GetAwaiter().GetResult();
        }

        #region GetTask

        [Fact]
        public async Task GetTask_ShouldReturnTask_WhenTaskExists()
        {
            // arrange
            int taskId = 1;

            // act
            var response = await _sut.GetAsync($"api/task/{taskId}");

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
        }

        #endregion

        #region GetAllTasks

        [Fact]
        public async Task GetAllTasks_ShouldReturnTasks()
        {
            // act
            var response = await _sut.GetAsync($"api/tasks");

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
        }

        #endregion

        #region GetAllTasks

        [Fact]
        public async Task CreateTask_ShouldCreateTask()
        {
            // act
            var response = await _sut.PostAsJsonAsync($"api/task", GetNewJiraTask());

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);

            var taskID = response.Content.ReadAsStringAsync().Result;

            var responseGet = await _sut.GetAsync($"api/task/{taskID}");
            Assert.True(responseGet.IsSuccessStatusCode);
            Assert.NotNull(responseGet.Content);
        }

        #endregion

        #region DeleteTask

        [Fact]
        public async Task DeleteTask_ShouldDeleteTask_WhenTaskExists()
        {
            // arrange
            var reponseCreate = await _sut.PostAsJsonAsync($"api/task", GetNewJiraTask());
            var taskID = reponseCreate.Content.ReadAsStringAsync().Result;

            // act
            var response = await _sut.DeleteAsync($"api/task/{taskID}");

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
        }

        #endregion

        #region UpdateTask

        [Fact]
        public async Task UpdateTask_ShouldUpdateTask()
        { 
            // arrange
            var response = await _sut.PostAsJsonAsync($"api/task", GetNewJiraTask());
            var taskID = response.Content.ReadAsStringAsync().Result;
            var jiraTask = GetJiraTask();
            jiraTask.Id = int.Parse(taskID);
            jiraTask.Description = "description edited";

            // act
            var responseUpdate = await _sut.PutAsJsonAsync($"api/task", jiraTask);

            // assert
            Assert.True(responseUpdate.IsSuccessStatusCode);
            Assert.NotNull(responseUpdate.Content);

            var responseGet = await _sut.GetAsync($"api/task/{taskID}");
            Assert.True(responseGet.IsSuccessStatusCode);
            var content = responseGet.Content.ReadAsStringAsync().Result;
            Assert.NotNull(content);
            Assert.Contains("description edited", content);
        }

        #endregion

        #region UpdateTaskStatus

        [Fact]
        public async Task UpdateTaskStatus_ShouldUpdateTaskStatus()
        {
            // arrange
            var taskData = GetNewJiraTask();
            var response = await _sut.PostAsJsonAsync($"api/task", taskData);
            var taskID = response.Content.ReadAsStringAsync().Result;
            var jiraTask = GetJiraTask();
            jiraTask.Id = int.Parse(taskID);
            var statusID = 2;

            // act
            var responseUpdate = await _sut.PostAsync($"api/task/{jiraTask.Id}/status/{statusID}", null);

            // assert
            Assert.True(responseUpdate.IsSuccessStatusCode);
            var responseGet = await _sut.GetAsync($"api/task/{taskID}");
            Assert.True(responseGet.IsSuccessStatusCode);
            var content = await responseGet.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("\"taskStatus\":2", content);
        }

        #endregion

        private DeserializedNewJiraTask GetNewJiraTask()
        {
            return new DeserializedNewJiraTask
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

        private DeserializedJiraTask GetJiraTask()
        {
            return new DeserializedJiraTask
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
