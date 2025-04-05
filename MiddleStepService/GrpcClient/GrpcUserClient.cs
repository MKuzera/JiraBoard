using Grpc.Net.Client;
using JiraBoardgRPC;
using MiddleStepService.Models;

namespace MiddleStepService.GrpcClient
{
    public class GrpcUserClient
    {
        public static GrpcUserClient Instance => _instance.Value;

        private static readonly Lazy<GrpcUserClient> _instance = new Lazy<GrpcUserClient>(() => new GrpcUserClient());

        private readonly User.UserClient _client;

        private readonly GrpcChannel _channel;

        private const string GrpcServerUrlAdress = "http://localhost:5107";

        private GrpcUserClient()
        {
            _channel = GrpcChannel.ForAddress(GrpcServerUrlAdress);
            _client = new User.UserClient(_channel);
        }

        public async Task<DeserializedUserResponse> GetUserAsync(int userID)
        {
            UserIdRequest userIdRequest = new UserIdRequest { Id = userID };
            var response = await _client.GetUserAsync(userIdRequest);
            DeserializedUserResponse deserializedUserResponse = Deserializer.DeserializeUserResponse(response);
            return deserializedUserResponse;
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
