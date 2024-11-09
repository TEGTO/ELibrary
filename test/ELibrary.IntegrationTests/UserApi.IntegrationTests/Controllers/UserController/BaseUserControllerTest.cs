using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.IntegrationTests.Controllers.UserController
{
    internal class BaseUserControllerTest : BaseControllerTest
    {
        protected async Task RegisterSampleUser(UserRegistrationRequest registerRequest)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register");

            request.Content = new StringContent(
                JsonSerializer.Serialize(registerRequest),
                Encoding.UTF8,
                "application/json"
            );
            var response = await client.SendAsync(request);
        }
        protected async Task<string> GetAccessKeyForUserAsync(UserAuthenticationRequest loginRequest)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login");
            request.Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<UserAuthenticationResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return authResponse.AuthToken.AccessToken;
        }
    }
}
