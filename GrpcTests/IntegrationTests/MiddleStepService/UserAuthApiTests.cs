using GrpcTests.IntegrationTests.Helpers;
using MiddleStepService.Models;
using System.Net.Http.Json;
using Xunit;

namespace GrpcTests.IntegrationTests.MiddleStepService
{
    public class UserAuthApiTest : IClassFixture<TestBaseClass>
    {
        private readonly HttpClient _sut;

        public UserAuthApiTest(TestBaseClass testBase)
        {
            _sut = AuthenticationHelper.GetClient(Constants.MiddleStepApiAddress).GetAwaiter().GetResult();
        }
        #region Register

        [Fact]
        public async Task Register_ShouldRegisterUser()
        {
            // arrange 
            DeserializedRegisterRequest registerRequest = new DeserializedRegisterRequest
            {
                FirstName = "Test",
                LastName = "TestLastName",
                Email = "newemail.com",
                Password = "password",
            };

            // act
            var response = await _sut.PostAsJsonAsync("/api/register", registerRequest);

            // assert
            var responseContent = await response.Content.ReadFromJsonAsync<DeserializedRegisterResponse>();

            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(responseContent);
            Assert.NotEqual(0, responseContent.UserId);
            Assert.True(responseContent.Success);
        }

        [Fact]
        public async Task Register_ShouldReturnError_WhenUserExistsWithGivenEmail()
        {
            // arrange 
            DeserializedRegisterRequest registerRequest = new DeserializedRegisterRequest
            {
                FirstName = "Test",
                LastName = "TestLastName",
                Email = "marek.zielinski@example.com",
                Password = "password",
            };

            // act
            var response = await _sut.PostAsJsonAsync("/api/register", registerRequest);

            // assert
            Assert.NotNull(response);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(response.StatusCode, System.Net.HttpStatusCode.Conflict);
        }

        #endregion

        #region Login

        [Fact]
        public async Task Login_ShouldReturnUser_WhenExists()
        {
            // arrange 
            DeserializedLoginRequest loginRequest = new DeserializedLoginRequest
            {
                Email = "jan.kowalski@example.com",
                Password = "123",
            };

            // act
            var response = await _sut.PostAsJsonAsync("api/login", loginRequest);

            // assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);

            LoginResponseWithToken reponseContent = await response.Content.ReadFromJsonAsync<LoginResponseWithToken>();
            Assert.NotNull(reponseContent); 
            Assert.True(reponseContent.Success);
            Assert.Equal(1, reponseContent.UserId);
            Assert.NotEmpty(reponseContent.Token);
        }

        [Fact]
        public async Task Login_ShouldReturnError_WhenUserDoesNotExists()
        {
            // arrange 
            DeserializedLoginRequest loginRequest = new DeserializedLoginRequest
            {
                Email = "X.kowalski@example.com",
                Password = "123",
            };

            // act
            var response = await _sut.PostAsJsonAsync("api/login", loginRequest);

            // assert
            Assert.NotNull(response);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion
    }
}
