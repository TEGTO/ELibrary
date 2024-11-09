using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Features.OrderFeature.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.OrderController
{
    internal class UpdateOrderRequestControllerTests : BaseOrderControllerTest
    {
        [Test]
        public async Task UpdateOrder_ValidRequest_ReturnsUpdatedWithOrder()
        {
            // Arrange
            var request = new ClientUpdateOrderRequest
            {
                Id = 1,
                DeliveryAddress = "NewDeliveryAddress",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                PaymentMethod = PaymentMethod.Cash,
                DeliveryMethod = DeliveryMethod.AddressDelivery,
            };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, "/order");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<OrderResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(response);
            Assert.That(response.DeliveryAddress, Is.EqualTo(request.DeliveryAddress));
        }
        [Test]
        public async Task UpdateOrder_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new ClientUpdateOrderRequest();
            using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, "/order");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task UpdateOrder_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var request = new ClientUpdateOrderRequest
            {
                Id = 1,
                DeliveryAddress = "NewDeliveryAddress",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                PaymentMethod = PaymentMethod.Cash,
                DeliveryMethod = DeliveryMethod.AddressDelivery,
            };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, "/order");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
