using Authentication.Identity;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.IntegrationTests.Controllers.AuthController
{
    internal class AdminRegisterAuthControllerTests : BaseAuthControllerTest
    {
        [Test]
        public async Task RegisterUser_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var registrationRequest = new AdminUserRegistrationRequest
            {
                Email = "testuser@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY",
                Roles = [Roles.CLIENT]
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/admin/register");
            request.Content = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var content = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonSerializer.Deserialize<AdminUserResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(responseEntity);
            Assert.That(responseEntity.Roles[0], Is.EqualTo(Roles.CLIENT));
        }

        [Test]
        public async Task RegisterUser_ConflictingEmail_ReturnsUnauthorized()
        {
            // Arrange
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "conflict@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });

            var registrationRequest = new AdminUserRegistrationRequest
            {
                Email = "conflict@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY",
                Roles = [Roles.CLIENT]
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/admin/register");
            request.Content = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }

        [Test]
        public async Task RegisterUser_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var registrationRequest = new AdminUserRegistrationRequest
            {
                Email = "", // Invalid
                Password = "Test@123" // Invalid
                //No confirmation
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/admin/register");
            request.Content = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task RegisterUser_NotEnoughRigts_ReturnsForbiddent()
        {
            // Arrange
            var registrationRequest = new AdminUserRegistrationRequest
            {
                Email = "testuser@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY",
                Roles = [Roles.CLIENT]
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/admin/register");
            request.Content = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}