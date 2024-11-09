using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.IntegrationTests.Controllers.UserController
{
    internal class AdminAdminDeleteUserControllerTests : BaseUserControllerTest
    {
        [Test]
        public async Task AdminDeleteUser_ValidRequest_ReturnsOk()
        {
            // Arrange
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "user1@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            string info = "user1@example.com";
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"/user/admin/delete/{info}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
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
        public async Task AdminDeleteUser_NotEnoughtRights_ReturnsForbidden()
        {
            // Arrange
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "user2@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            string info = "user2@example.com";
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"/user/admin/delete/{info}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
