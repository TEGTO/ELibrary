using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.OrderController
{
    internal class ManagerCancelOrderOrderController : BaseOrderControllerTest
    {
        [Test]
        public async Task ManagerCancelOrder_ValidId_CancelsOrder()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/order/manager/1");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var httpGetResponse = await GetOrderByIdAsync(1);
            var content = await httpGetResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<OrderResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(response.OrderStatus, Is.EqualTo(OrderStatus.Canceled));
        }
        [Test]
        public async Task ManagerCancelOrder_InvalidId_ReturnsConflict()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/order/manager/100");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            var httpGetResponse = await GetOrderByIdAsync(100);
            Assert.That(httpGetResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task ManagerCancelOrder_CancelsCanceledOrder_ReturnsConflict()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/order/manager/2");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            var httpGetResponse = await GetOrderByIdAsync(2);
            var content = await httpGetResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<OrderResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(response.OrderStatus, Is.EqualTo(OrderStatus.Canceled));
        }
        [Test]
        public async Task ManagerCancelOrder_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/order/manager/1");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task ManagerCancelOrder_NotManager_ReturnsForbidden()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/order/manager/1");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        private async Task<HttpResponseMessage> GetOrderByIdAsync(int id)
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/order/manager/{id}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            var httpResponse = await httpClient.SendAsync(httpRequest);
            return httpResponse;
        }
    }
}
