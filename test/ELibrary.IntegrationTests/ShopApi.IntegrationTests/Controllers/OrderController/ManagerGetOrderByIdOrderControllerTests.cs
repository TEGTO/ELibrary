using LibraryShopEntities.Domain.Dtos.Shop;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.OrderController
{
    internal class ManagerGetOrderByIdOrderControllerTests : BaseOrderControllerTest
    {
        [Test]
        public async Task ManagerGetOrderById_ValidRequest_ReturnsOkWithOrder()
        {
            // Arrange
            int existingOrderId = 1;
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/order/manager/{existingOrderId}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<OrderResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(response);
            Assert.That(response.Id, Is.EqualTo(existingOrderId));
        }
        [Test]
        public async Task ManagerGetOrderById_OrderDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            int nonExistingOrderId = 9999;
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/order/manager/{nonExistingOrderId}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task ManagerGetOrderById_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            int existingOrderId = 1;
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/order/manager/{existingOrderId}");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task ManagerGetOrderById_Forbidden_ReturnsForbidden()
        {
            // Arrange
            var clientAccessToken = await GenerateNewClientAccessTokenAsync();
            int existingOrderId = 1;
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/order/manager/{existingOrderId}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", clientAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
