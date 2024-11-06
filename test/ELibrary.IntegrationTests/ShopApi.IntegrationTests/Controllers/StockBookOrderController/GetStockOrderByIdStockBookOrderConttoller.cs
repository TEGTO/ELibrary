using LibraryShopEntities.Domain.Dtos.Shop;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.StockBookOrderController
{
    internal class GetStockOrderByIdStockBookOrderConttoller : BaseStockBookOrderControllerTest
    {
        private StockBookOrderResponse order;

        [OneTimeSetUp]
        public async Task OneTimeSetUpGetStockOrderById()
        {
            order = await CreateStockBookOrderAsync(ManagerAccessToken);
        }

        [Test]
        public async Task GetStockOrderById_ValidRequest_ReturnsOkWithResponse()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/stockbook/{order.Id}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<StockBookOrderResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(response);
            Assert.That(response.Id, Is.EqualTo(order.Id));
            Assert.That(response.StockBookChanges[0].ChangeAmount, Is.EqualTo(10));
        }
        [Test]
        public async Task GetStockOrderById_IdDoesntExist_ReturnsNotFound()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/stockbook/100");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task GetStockOrderById_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/stockbook/{order.Id}");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
