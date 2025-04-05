using Grpc.Core;
using Grpc.Net.Client;
using GrpcTests.IntegrationTests.Helpers;
using JiraBoardgRPC;
using Xunit;

namespace GrpcTests.IntegrationTests.GrpcService
{
    public class UserAuthTests : IClassFixture<TestBaseClass>
    {
        private UserAuth.UserAuthClient _sut;

        private readonly GrpcChannel _channel;

        public UserAuthTests(TestBaseClass fixture)
        {
            _channel = GrpcChannel.ForAddress(Constants.GrpcServerUrlAdress);
            _sut = new UserAuth.UserAuthClient(_channel);
        }

        [Fact]
        public async Task Register_ShouldRegisterUser()
        {
            // arrange 
            RegisterRequest registerRequest = new RegisterRequest
            {
                FirstName = "Test",
                LastName = "TestLastName",
                Email = "email.com",
                Password = "password",
            };
      
            // act
            var response = await _sut.RegisterAsync(registerRequest);

            // assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.NotEqual(0, response.UserId);
        }

        [Fact]
        public async Task Register_ShouldReturnError_WhenUserExistsWithGivenEmail()
        {
            // arrange 
            RegisterRequest registerRequest = new RegisterRequest
            {
                FirstName = "Test",
                LastName = "TestLastName",
                Email = "jan.kowalski@example.com",
                Password = "password",
            };

            // act & assert
            var exception = await Assert.ThrowsAsync<RpcException>(async () => await _sut.RegisterAsync(registerRequest));

            Assert.Equal(StatusCode.AlreadyExists, exception.StatusCode);
            Assert.Equal("User already exists", exception.Status.Detail);
        }

        [Fact]
        public async Task Login_ShouldReturnUserIfExists()
        {
            // arrange 
            LoginRequest loginRequest = new LoginRequest
            {
                Email = "jan.kowalski@example.com",
                Password = "123",
            };

            // act
            var response = await _sut.LoginAsync(loginRequest);

            // assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            Assert.Equal(1, response.UserId);
        }

        [Fact]
        public async Task Login_ShouldThrowNotFoundException_WhenUserDoesNotExists()
        {
            // arrange 
            LoginRequest loginRequest = new LoginRequest
            {
                Email = "not.exitsts@example.com",
                Password = "123",
            };

            // act & assert
            var exception = await Assert.ThrowsAsync<RpcException>(async () => await _sut.LoginAsync(loginRequest));

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.Equal("User not found", exception.Status.Detail);
        }

        [Fact]
        public async Task Login_ShouldThrowPermissionDeniedException_WhenUserMissmatchPassword()
        {
            // arrange 
            LoginRequest loginRequest = new LoginRequest
            {
                Email = "jan.kowalski@example.com",
                Password = "badpassword",
            };

            // act & assert
            var exception = await Assert.ThrowsAsync<RpcException>(async () => await _sut.LoginAsync(loginRequest));

            Assert.Equal(StatusCode.PermissionDenied, exception.StatusCode);
            Assert.Equal("Invalid password", exception.Status.Detail);
        }
    }
}
