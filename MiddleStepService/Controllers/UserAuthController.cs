using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using MiddleStepService.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MiddleStepService.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserAuthController : ControllerBase
    {
        private readonly string _secretKey = "your-256-bit-secret-it-should-be-hidden"; 
        private readonly string _issuer = "your-app"; 
        private readonly string _audience = "your-audience";

        [HttpPost("login")]
        public ActionResult<LoginResponseWithToken> LoginUser([FromBody] DeserializedLoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Email and password are required.");
            }
            try
            {
                DeserializedLoginResponse deserializedLoginResponse = GrpcClient.GrpcUserAuthClient.Instance.LoginAsync(loginRequest).GetAwaiter().GetResult();

                if (deserializedLoginResponse.Success)
                {
                    var token = GenerateJwtToken(deserializedLoginResponse.UserId, deserializedLoginResponse.Message);
                    LoginResponseWithToken loginResponseWithtoken = new LoginResponseWithToken
                    {
                        Token = token,
                        Success = true,
                        Message = "Login Successful",
                        UserId = deserializedLoginResponse.UserId,
                    };
                    return loginResponseWithtoken;
                }

                return Unauthorized("Invalid email or password.");

            }
            catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound || ex.StatusCode == Grpc.Core.StatusCode.PermissionDenied)
            {
                return NotFound("User with that email and password was not found");
            }      
        }

        [HttpPost("register")]
        public ActionResult<DeserializedRegisterResponse> RegisterUser([FromBody] DeserializedRegisterRequest registerRequest)
        {
            if (registerRequest == null || string.IsNullOrEmpty(registerRequest.Email) || string.IsNullOrEmpty(registerRequest.Password) || string.IsNullOrEmpty(registerRequest.LastName) || string.IsNullOrEmpty(registerRequest.FirstName))
            {
                return BadRequest("All fields are required");
            }
            try
            {
                DeserializedRegisterResponse deserializedRegisterResponse = GrpcClient.GrpcUserAuthClient.Instance.RegisterAsync(registerRequest).GetAwaiter().GetResult();
                return deserializedRegisterResponse;

            }
            catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.AlreadyExists)
            {
                return Conflict("User with that email already exists.");
            }
        }

        private string GenerateJwtToken(int userId, string message)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, message),  
            new Claim(ClaimTypes.Role, "User")  
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
