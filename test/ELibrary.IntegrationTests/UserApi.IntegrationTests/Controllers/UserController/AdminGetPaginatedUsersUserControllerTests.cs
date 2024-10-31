using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.IntegrationTests.Controllers.UserController
{
    internal class AdminGetPaginatedUsersUserControllerTests : BaseUserControllerTest
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "testuser@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
            await RegisterSampleUser(new UserRegistrationRequest
            {
                Email = "testuser2@example.com",
                Password = "123456;QWERTY",
                ConfirmPassword = "123456;QWERTY"
            });
        }

        [Test]
        public async Task AdminGetUserByInfo_ValidRequest_ReturnsOkWithTwoItems()
        {
            // Arrange
            var filter = new AdminGetUserFilter
            {
                PageNumber = 1,
                PageSize = 10,
                ContainsInfo = ""
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, $"/user/admin/users");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var paginatedUsersResponse = JsonSerializer.Deserialize<List<AdminUserResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(paginatedUsersResponse);
            Assert.That(paginatedUsersResponse.Count, Is.EqualTo(2));
            Assert.That(paginatedUsersResponse[0].Email, Is.EqualTo("testuser2@example.com"));
        }
        [Test]
        public async Task AdminGetUserByInfo_ValidRequestWithFilter_ReturnsOkWithOneItem()
        {
            // Arrange
            var filter = new AdminGetUserFilter
            {
                PageNumber = 2,
                PageSize = 1,
                ContainsInfo = ""
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, $"/user/admin/users");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var paginatedUsersResponse = JsonSerializer.Deserialize<List<AdminUserResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(paginatedUsersResponse);
            Assert.That(paginatedUsersResponse.Count, Is.EqualTo(1));
            Assert.That(paginatedUsersResponse[0].Email, Is.EqualTo("testuser@example.com"));
        }
        [Test]
        public async Task AdminGetUserByInfo_ValidRequest_ReturnsOkWithZeroItems()
        {
            // Arrange
            var filter = new AdminGetUserFilter
            {
                PageNumber = 100,
                PageSize = 10,
                ContainsInfo = ""
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, $"/user/admin/users");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var paginatedUsersResponse = JsonSerializer.Deserialize<List<AdminUserResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(paginatedUsersResponse);
            Assert.That(paginatedUsersResponse.Count, Is.EqualTo(0));
        }
        [Test]
        public async Task AdminGetUserByInfo_NotEnoughRights_ReturnsForbidden()
        {
            // Arrange
            var filter = new AdminGetUserFilter
            {
                PageNumber = 1,
                PageSize = 10,
                ContainsInfo = ""
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, $"/user/admin/users");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
