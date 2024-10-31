using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.IntegrationTests.Controllers.UserController
{
    internal class DeleteUserControllerTests : BaseUserControllerTest
    {
        [Test]
        public async Task DeleteUser_ValidRequest_ReturnsOk()
        {
            // Arrange
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "user1@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            using var request = new HttpRequestMessage(HttpMethod.Delete, "/user");
            var accessKey = await GetAccessKeyForUserAsync(new UserAuthenticationRequest
            {
                Login = "user1@example.com",
                Password = "123456;QWERTY",
            });
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessKey);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var loginRequest = new UserAuthenticationRequest
            {
                Login = "user1@example.com",
                Password = "123456;QWERTY"
            };
            using var request2 = new HttpRequestMessage(HttpMethod.Post, "/auth/login");
            request2.Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            var response2 = await client.SendAsync(request2);
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task DeleteUser_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "user3@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            using var request = new HttpRequestMessage(HttpMethod.Delete, "/user");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            var loginRequest = new UserAuthenticationRequest
            {
                Login = "user3@example.com",
                Password = "123456;QWERTY"
            };
            using var request2 = new HttpRequestMessage(HttpMethod.Post, "/auth/login");
            request2.Content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
            var response2 = await client.SendAsync(request2);
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
