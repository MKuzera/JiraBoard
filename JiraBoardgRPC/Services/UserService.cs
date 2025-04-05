using Grpc.Core;
using JiraBoardgRPC.FakeDataBase;

namespace JiraBoardgRPC.Services
{
    public class UserService : User.UserBase
    {
        private readonly ILogger<UserService> _logger;

        private readonly IDataBaseService _dataBaseService;

        public UserService(IDataBaseService dataBaseService, ILogger<UserService> logger)
        {
            _dataBaseService = dataBaseService;
            _logger = logger;
        }

        public override Task<UserResponse> GetUser(UserIdRequest request, ServerCallContext context)
        {
            var response = _dataBaseService.GetUser(request.Id);
            UserResponse userResponse = new UserResponse {Email = response.Email, Id = response.Id, FirstName = response.FirstName, LastName = response.LastName };
            return Task.FromResult(userResponse);
        }
    }
}
