using System.Net;
using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.IntegrationTests.Controllers.AuthController
{
    internal class RefreshTokenAuthControllerTests : BaseAuthControllerTest
    {
        [Test]
        public async Task RefreshToken_ValidRequest_ReturnsOk()
        {
            // Arrange
            var refreshRequest = await GetValidAuthToken();
            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/refresh");
            request.Content = new StringContent(JsonSerializer.Serialize(refreshRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task RefreshToken_InvalidRequest_ReturnsUnauthorized()
        {
            // Arrange
            var refreshRequest = new AuthToken
            {
                AccessToken = "invalidAccessToken",
                RefreshToken = "validRefreshToken"
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/refresh");
            request.Content = new StringContent(JsonSerializer.Serialize(refreshRequest), Encoding.UTF8, "application/json");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }

        private async Task<AuthToken?> GetValidAuthToken()
        {
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "testuser@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });

            var loginRequest = new UserAuthenticationRequest
            {
                Login = "testuser@example.com",
                Password = "123456;QWERTY"
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login");
            request.Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<UserAuthenticationResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return authResponse?.AuthToken;
        }
    }
}
