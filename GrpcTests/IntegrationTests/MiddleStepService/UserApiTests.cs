using GrpcTests.IntegrationTests.Helpers;
using Xunit;

namespace GrpcTests.IntegrationTests.MiddleStepService
{
    public class UserApiTest : IClassFixture<TestBaseClass>
    {
        private readonly HttpClient _sut;

        public UserApiTest(TestBaseClass testBase)
        {
            _sut = AuthenticationHelper.GetClient(Constants.MiddleStepApiAddress).GetAwaiter().GetResult();
        }

        #region GetUser

        [Fact]
        public async Task GetUser_ShouldReturnUser_WhenUserExists()
        {
            // arrange
            int userID = 1;

            // act
            var response = await _sut.GetAsync($"api/user/{userID}");

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response.Content);
        }

        [Fact]
        public async Task GetUser_ShouldReturnError_WhenUserDoesNotExists()
        {
            // arrange
            int userID = 100;

            // act
            var response = await _sut.GetAsync($"api/user/{userID}");

            // assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion
    }
}
