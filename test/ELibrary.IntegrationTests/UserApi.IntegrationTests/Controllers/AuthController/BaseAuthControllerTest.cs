using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.IntegrationTests.Controllers.AuthController
{
    internal class BaseAuthControllerTest : BaseControllerTest
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
    }
}
