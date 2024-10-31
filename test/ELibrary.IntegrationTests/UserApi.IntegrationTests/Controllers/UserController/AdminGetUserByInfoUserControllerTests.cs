using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.IntegrationTests.Controllers.UserController
{
    internal class AdminGetUserByInfoUserControllerTests : BaseUserControllerTest
    {
        [Test]
        public async Task AdminGetUserByInfo_ValidRequest_ReturnsOk()
        {
            // Arrange
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "testuser@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            string info = "testuser@example.com";
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/user/admin/users/{info}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var adminUserResponse = JsonSerializer.Deserialize<AdminUserResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(adminUserResponse);
            Assert.That(adminUserResponse.Email, Is.EqualTo("testuser@example.com"));
        }
        [Test]
        public async Task AdminGetUserByInfo_InvalidRequest_ReturnsNotFound()
        {
            // Arrange
            string info = "testuser1@example.com";
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/user/admin/users/{info}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task AdminGetUserByInfo_NotEnoughtRights_ReturnsForbidden()
        {
            // Arrange
            string info = "testuser2@example.com";
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/user/admin/users/{info}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}