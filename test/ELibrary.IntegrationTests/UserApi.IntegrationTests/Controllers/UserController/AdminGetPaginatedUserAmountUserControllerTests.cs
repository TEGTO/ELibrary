using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.IntegrationTests.Controllers.UserController
{
    internal class AdminGetPaginatedUserAmountUserControllerTests : BaseUserControllerTest
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
        public async Task AdminGetPaginatedUserAmount_ValidRequest_ReturnsOkWithTwo()
        {
            // Arrange
            var filter = new AdminGetUserFilter
            {
                ContainsInfo = ""
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, $"/user/admin/users/amount");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            Assert.That(int.Parse(content), Is.EqualTo(2));
        }
        [Test]
        public async Task AdminGetPaginatedUserAmount_ValidRequestWithFilter_ReturnsOkWithOne()
        {
            // Arrange
            var filter = new AdminGetUserFilter
            {
                PageNumber = 2,
                PageSize = 1,
                ContainsInfo = "testuser2@example.com"
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, $"/user/admin/users/amount");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            Assert.That(int.Parse(content), Is.EqualTo(1));
        }
        [Test]
        public async Task AdminGetPaginatedUserAmount_ValidRequest_ReturnsOkWithZero()
        {
            // Arrange
            var filter = new AdminGetUserFilter
            {
                PageNumber = 100,
                PageSize = 10,
                ContainsInfo = "someinfo"
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, $"/user/admin/users/amount");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            Assert.That(int.Parse(content), Is.EqualTo(0));
        }
        [Test]
        public async Task AdminGetPaginatedUserAmount_NotEnoughRights_ReturnsForbidden()
        {
            // Arrange
            var filter = new AdminGetUserFilter
            {
                PageNumber = 1,
                PageSize = 10,
                ContainsInfo = ""
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, $"/user/admin/users/amount");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
