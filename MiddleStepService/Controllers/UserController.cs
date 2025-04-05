using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiddleStepService.Models;

namespace MiddleStepService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        [HttpGet("user/{userID}")]
        public ActionResult<DeserializedUserResponse> GetUser(int userID)
        {
           DeserializedUserResponse response = GrpcClient.GrpcUserClient.Instance.GetUserAsync(userID).GetAwaiter().GetResult();
            if (response == null) 
            {
                return NotFound();
            }
           return new JsonResult(response);
        }
    }
}
