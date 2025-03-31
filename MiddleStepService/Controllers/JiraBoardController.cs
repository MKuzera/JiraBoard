using JiraBoardgRPC;
using Microsoft.AspNetCore.Mvc;
using MiddleStepService.Models;

namespace MiddleStepService.Controllers
{
    [ApiController]
    [Route("api")]
    public class JiraBoardController : ControllerBase
    {
        [HttpGet("task/{taskID}")]
        public ActionResult<DeserializedJiraTask> GetTask(int taskID)
        {
            DeserializedJiraTask task = GrpcClient.GrpcClient.Instance.GetTaskAsync(taskID).GetAwaiter().GetResult();
            return new JsonResult(task);
        }

        [HttpPost("task")]
        public IActionResult CreateTask([FromBody] DeserializedNewJiraTask task)
        {
            NewJiraTask newJiraTask = GRPCSerializer.SerializeNewJiraTask(task);
            var grpcResponse = GrpcClient.GrpcClient.Instance.CreateTaskAsync(newJiraTask).GetAwaiter().GetResult();
            return Created($"task/{grpcResponse}" ,  grpcResponse);
        }

        [HttpGet("tasks")]
        public ActionResult<DeserializedJiraTask> GetAllTasks()
        {
            var tasks = GrpcClient.GrpcClient.Instance.GetAllTaskAsync().GetAwaiter().GetResult();
            return new JsonResult(tasks);
        }

        [HttpPut("task")]
        public ActionResult<string> UpdateTask([FromBody] DeserializedJiraTask task)
        {
            var grpcResponse = GrpcClient.GrpcClient.Instance.UpdateTaskAsync(task).GetAwaiter().GetResult();
            return Ok(grpcResponse);
        }

        [HttpDelete("task/{taskID}")]
        public ActionResult<string> DeleteTask(int taskID)
        {
            var grpcResponse = GrpcClient.GrpcClient.Instance.DeleteTaskAsync(taskID).GetAwaiter().GetResult();
            return Ok(grpcResponse);
        }
    }
}
