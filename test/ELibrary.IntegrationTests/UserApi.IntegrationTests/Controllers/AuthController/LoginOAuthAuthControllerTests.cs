using Authentication.OAuth;
using System.Net;
using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.IntegrationTests.Controllers.AuthController
{
    internal class LoginOAuthAuthControllerTests : BaseAuthControllerTest
    {
        [Test]
        public async Task LoginOAuthRequest_ValidRequest_ReturnsOk()
        {
            //Arrange
            var loginRequest = new LoginOAuthRequest
            {
                Code = "somecode",
                CodeVerifier = "someverifier",
                RedirectUrl = "someurl",
                OAuthLoginProvider = OAuthLoginProvider.Google,
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/oauth");
            request.Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<UserAuthenticationResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(authResponse);
            Assert.That(authResponse.Email, Is.EqualTo("someemail@gmail.com"));
        }
        [Test]
        public async Task LoginOAuthRequest_InvalidRequestData_ReturnsBadRequest()
        {
            //Arrange
            var loginRequest = new LoginOAuthRequest
            {
                Code = "someinvalidcode",
                CodeVerifier = "someinvalidverifier",
                RedirectUrl = "someinvalidurl",
                OAuthLoginProvider = OAuthLoginProvider.Google,
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/oauth");
            request.Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task LoginOAuthRequest_InvalidRequest_ReturnsBadRequest()
        {
            //Arrange
            var loginRequest = new LoginOAuthRequest
            {
                Code = "",
                CodeVerifier = "",
                RedirectUrl = "",
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/oauth");
            request.Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
