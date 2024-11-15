using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Features.OrderFeature.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.OrderController
{
    internal class CreateOrderControllerTests : BaseOrderControllerTest
    {
        [Test]
        public async Task CreateOrder_ValidRequest_ReturnsCreatedWithOrder()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                ContactClientName = "Client Name",
                ContactPhone = "0123456789",
                DeliveryAddress = "123 Test Street",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                PaymentMethod = PaymentMethod.Cash,
                DeliveryMethod = DeliveryMethod.AddressDelivery,
                OrderBooks = new List<OrderBookRequest>
                {
                    new OrderBookRequest { BookAmount = 1, BookId = 1 },
                    new OrderBookRequest { BookAmount = 2, BookId = 2 }
                }
            };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<OrderResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(response);
            Assert.That(response.DeliveryAddress, Is.EqualTo(request.DeliveryAddress));
            Assert.That(response.OrderBooks.Count, Is.EqualTo(request.OrderBooks.Count));
        }
        [Test]
        public async Task CreateOrder_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                PaymentMethod = PaymentMethod.Cash,
                DeliveryMethod = DeliveryMethod.AddressDelivery,
                OrderBooks = null
            };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task CreateOrder_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "123 Test Street",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                PaymentMethod = PaymentMethod.Cash,
                DeliveryMethod = DeliveryMethod.AddressDelivery,
                OrderBooks = new List<OrderBookRequest>
                {
                    new OrderBookRequest { BookAmount = 1, BookId = 1 },
                    new OrderBookRequest { BookAmount = 2, BookId = 2 }
                }
            };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
