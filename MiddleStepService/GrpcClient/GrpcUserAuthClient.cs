using Grpc.Net.Client;
using JiraBoardgRPC;
using MiddleStepService.Models;

namespace MiddleStepService.GrpcClient
{
    public class GrpcUserAuthClient
    {
        public static GrpcUserAuthClient Instance => _instance.Value;

        private static readonly Lazy<GrpcUserAuthClient> _instance = new Lazy<GrpcUserAuthClient>(() => new GrpcUserAuthClient());

        private readonly UserAuth.UserAuthClient _client;

        private readonly GrpcChannel _channel;

        private const string GrpcServerUrlAdress = "http://localhost:5107";

        private GrpcUserAuthClient()
        {
            _channel = GrpcChannel.ForAddress(GrpcServerUrlAdress);
            _client = new UserAuth.UserAuthClient(_channel);
        }

        public async Task<DeserializedLoginResponse> LoginAsync(DeserializedLoginRequest deserializedLoginRequest)
        {
            LoginRequest loginRequest = new LoginRequest
            {
                Password = deserializedLoginRequest.Password,
                Email = deserializedLoginRequest.Email,
            };
  
            LoginResponse loginResponse = await _client.LoginAsync(loginRequest);

            DeserializedLoginResponse deserializedLoginResponse = new DeserializedLoginResponse
            {
                Message = loginResponse.Message,
                UserId = loginResponse.UserId,
                Success = loginResponse.Success,
            };

            return deserializedLoginResponse;
        }

        public async Task<DeserializedRegisterResponse> RegisterAsync(DeserializedRegisterRequest deserializedRegisterRequest)
        {
            RegisterRequest registerRequest = new RegisterRequest
            {
                Password = deserializedRegisterRequest.Password,
                Email = deserializedRegisterRequest.Email,
                FirstName = deserializedRegisterRequest.FirstName,
                LastName = deserializedRegisterRequest.LastName,
            };

            RegisterResponse registerResponse = await _client.RegisterAsync(registerRequest);

            DeserializedRegisterResponse deserializedRegisterResponse = new DeserializedRegisterResponse
            {
                Message= registerResponse.Message,
                Success = registerResponse.Success,
                UserId= registerResponse.UserId,
            };

            return deserializedRegisterResponse;
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
