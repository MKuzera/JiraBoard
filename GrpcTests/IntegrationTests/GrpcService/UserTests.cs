using Grpc.Core;
using Grpc.Net.Client;
using GrpcTests.IntegrationTests.Helpers;
using JiraBoardgRPC;
using Xunit;

namespace GrpcTests.IntegrationTests.GrpcService
{
    public class UserTests : IClassFixture<TestBaseClass>
    {
        private User.UserClient _sut;

        private readonly GrpcChannel _channel;

        public UserTests(TestBaseClass fixture)
        {
            _channel = GrpcChannel.ForAddress(Constants.GrpcServerUrlAdress);
            _sut = new User.UserClient(_channel);
        }

        [Fact]
        public async Task GetUser_ShouldReturnUser_WhenExists()
        {
            // arrange 
            var userID = new UserIdRequest { Id = 1 };

            // act
            var response = await _sut.GetUserAsync(userID);

            // assert
            Assert.NotNull(response);
            Assert.Equal(1, response.Id);
        }

        [Fact]
        public async Task GetUser_ShouldReturnNotFoundException_WhenUserDoesNotExists()
        {
            // arrange 
            var userID = new UserIdRequest { Id = 100 };

            // act & assert
            var exception = await Assert.ThrowsAsync<RpcException>(async () => await _sut.GetUserAsync(userID));

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Equal("User not found", exception.Status.Detail);
        }
    }
}
