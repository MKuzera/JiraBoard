using System.Net.Http.Json;
using MiddleStepService.Models;

namespace GrpcTests.IntegrationTests.Helpers
{
    public class AuthenticationHelper
    {
        public async static Task<HttpClient> GetClient(string endpoint)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(endpoint) };

            var loginRequest = new
            {
                Email = "test@test.com",
                Password = "test"
            };

            var loginResponse = await httpClient.PostAsJsonAsync("api/login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponseWithToken>();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);

            return httpClient;
        }
    }
}
