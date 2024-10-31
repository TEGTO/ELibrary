using System.Net;
using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.IntegrationTests.Controllers.AuthController
{
    internal class RegisterAuthControllerTests : BaseAuthControllerTest
    {
        [Test]
        public async Task RegisterUser_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var registrationRequest = new UserRegistrationRequest
            {
                Email = "testuser@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register");
            request.Content = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
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
            var registrationRequest = new UserRegistrationRequest
            {
                Email = "conflict@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register");
            request.Content = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }
        [Test]
        public async Task RegisterUser_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var registrationRequest = new UserRegistrationRequest
            {
                Email = "", // Invalid
                Password = "Test@123" // Invalid
                //No confirmation
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/auth/register");
            request.Content = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}