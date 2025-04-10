﻿using JiraBoardgRPC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiddleStepService.Models;
using MiddleStepService.Serializers;

namespace MiddleStepService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class JiraBoardController : ControllerBase
    {
        [HttpGet("task/{taskID}")]
        public ActionResult<DeserializedJiraTask> GetTask(int taskID)
        {
            DeserializedJiraTask task = GrpcClient.GrpcJiraBoardClient.Instance.GetTaskAsync(taskID).GetAwaiter().GetResult();
            return new JsonResult(task);
        }

        [HttpPost("task/{taskID}/status/{statusID}")]
        public ActionResult UpdateTaskStatus(int taskID, int statusID)
        {
            DeserializedJiraTask task = GrpcClient.GrpcJiraBoardClient.Instance.GetTaskAsync(taskID).GetAwaiter().GetResult();
            task.TaskStatus = (Models.JiraTaskStatus)statusID;
            GrpcClient.GrpcJiraBoardClient.Instance.UpdateTaskAsync(task).GetAwaiter().GetResult();
            return Ok();
        }

        [HttpPost("task")]
        public IActionResult CreateTask([FromBody] DeserializedNewJiraTask task)
        {
            NewJiraTask newJiraTask = GRPCSerializer.SerializeNewJiraTask(task);
            var grpcResponse = GrpcClient.GrpcJiraBoardClient.Instance.CreateTaskAsync(newJiraTask).GetAwaiter().GetResult();
            return Created($"task/{grpcResponse}", grpcResponse);
        }

        [HttpPost("task/comment")]
        public IActionResult AddComment([FromBody] AddCommentRequest request)
        {
            var grpcResponse = GrpcClient.GrpcJiraBoardClient.Instance.AddCommentToTaskAsync(request.TaskID, request.Comment, int.Parse(request.UserID)).GetAwaiter().GetResult();
            return Created($"task/{grpcResponse}", grpcResponse);
        }

        [HttpGet("tasks")]
        public ActionResult<DeserializedJiraTask> GetAllTasks()
        {
            var tasks = GrpcClient.GrpcJiraBoardClient.Instance.GetAllTaskAsync().GetAwaiter().GetResult();
            return new JsonResult(tasks);
        }

        [HttpGet("tasksByIds")]
        public ActionResult<IEnumerable<DeserializedJiraTask>> GetTasksByIds([FromQuery] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest("No IDs provided.");
            }

            var allTasks = GrpcClient.GrpcJiraBoardClient.Instance.GetAllTaskAsync().GetAwaiter().GetResult();
            var filteredTasks = allTasks.Where(task => ids.Contains(task.Id)).ToList();

            return Ok(filteredTasks);
        }

        [HttpPut("task")]
        public ActionResult<string> UpdateTask([FromBody] DeserializedJiraTask task)
        {
            var grpcResponse = GrpcClient.GrpcJiraBoardClient.Instance.UpdateTaskAsync(task).GetAwaiter().GetResult();
            return Ok(grpcResponse);
        }

        [HttpDelete("task/{taskID}")]
        public ActionResult<string> DeleteTask(int taskID)
        {
            var grpcResponse = GrpcClient.GrpcJiraBoardClient.Instance.DeleteTaskAsync(taskID).GetAwaiter().GetResult();
            return Ok(grpcResponse);
        }

        public class AddCommentRequest
        {
            public int TaskID { get; set; }
            public string Comment { get; set; }
            public string UserID { get; set; }
        }
    }
}
