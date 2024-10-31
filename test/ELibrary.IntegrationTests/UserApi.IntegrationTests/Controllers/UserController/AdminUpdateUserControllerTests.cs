using Authentication.Identity;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.IntegrationTests.Controllers.UserController
{
    internal class AdminAdminUpdateControllerTests : BaseUserControllerTest
    {
        [Test]
        public async Task AdminUpdate_ValidRequest_ReturnsOk()
        {
            // Arrange
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "olduser1@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            var updateRequest = new AdminUserUpdateDataRequest
            {
                CurrentLogin = "olduser1@example.com",
                Email = "updateduser1@example.com",
                Password = "NEW123456;QWERTY",
                ConfirmPassword = "NEW123456;QWERTY",
                Roles = [Roles.MANAGER]
            };
            using var request = new HttpRequestMessage(HttpMethod.Put, "/user/admin/update");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var adminUserResponse = JsonSerializer.Deserialize<AdminUserResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(adminUserResponse);
            Assert.That(adminUserResponse.Email, Is.EqualTo("updateduser1@example.com"));
        }
        [Test]
        public async Task AdminUpdate_NotEnoughRights_ReturnsForbidden()
        {
            // Arrange
            var updateRequest = new AdminUserUpdateDataRequest
            {
                CurrentLogin = "olduser2@example.com",
                Email = "updateduser2@example.com",
                Password = "NEW123456;QWERTY",
                ConfirmPassword = "NEW123456;QWERTY",
                Roles = [Roles.MANAGER]
            };
            using var request = new HttpRequestMessage(HttpMethod.Put, "/user/admin/update");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
        [Test]
        public async Task AdminUpdate_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var updateRequest = new AdminUserUpdateDataRequest
            {
                CurrentLogin = "",
                Email = "",
                Password = "NEW123456;QWERTY",
                ConfirmPassword = "NEW123456;QWERTY",
                Roles = [Roles.MANAGER]
            };
            using var request = new HttpRequestMessage(HttpMethod.Put, "/user/admin/update");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task AdminUpdate_ConflictingEmail_ReturnsConflictError()
        {
            // Arrange
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "conflict@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "conflict2@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            var updateRequest = new AdminUserUpdateDataRequest
            {
                CurrentLogin = "conflict2@example.com",
                Email = "conflict@example.com",
                Password = "NEW123456;QWERTY",
                ConfirmPassword = "NEW123456;QWERTY",
                Roles = [Roles.CLIENT],
            };
            using var request = new HttpRequestMessage(HttpMethod.Put, "/user/admin/update");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }
    }
}
