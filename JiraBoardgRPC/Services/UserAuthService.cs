using Grpc.Core;
using JiraBoardgRPC.FakeDataBase;

namespace JiraBoardgRPC.Services
{
    public class UserAuthService : UserAuth.UserAuthBase
    {
        private readonly ILogger<UserAuthService> _logger;

        private readonly IDataBaseService _dataBaseService;

        public UserAuthService(IDataBaseService dataBaseService, ILogger<UserAuthService> logger)
        {
            _dataBaseService = dataBaseService;
            _logger = logger;
        }

        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var user = _dataBaseService.GetUser(request.Email);

            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }

            if(user.Password != request.Password) 
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, "Invalid password"));
            }

            LoginResponse loginResponse = new LoginResponse
            {
                Message = "User found",
                UserId = user.Id,
                Success = true,
            };

            return Task.FromResult(loginResponse);
    }

        public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var user = _dataBaseService.GetUser(request.Email);

            if (user != null)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, "User already exists"));
            }

            var id = _dataBaseService.AddUser(request.Email, request.Password, request.FirstName, request.LastName);
            RegisterResponse response = new RegisterResponse { Success = true, Message = $"User {id} created", UserId = id };
            return Task.FromResult(response);
        }
    }
}
