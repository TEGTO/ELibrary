using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Filters;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.OrderController
{
    internal class GetOrdersOrderControllerTests : BaseOrderControllerTest
    {
        [Test]
        public async Task GetOrders_ClientWithOrders_ReturnsOkWithOrderList()
        {
            // Arrange
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/pagination");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<IEnumerable<OrderResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(response);
            Assert.IsNotEmpty(response);
        }
        [Test]
        public async Task GetOrders_ClientWithoutOrders_ReturnsOkWithEmptyList()
        {
            // Arrange
            var acccessToken = await GenerateNewClientAccessTokenAsync();
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/pagination");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", acccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<IEnumerable<OrderResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(response);
            Assert.IsEmpty(response);
        }
        [Test]
        public async Task GetOrders_Caching_ReturnsCachedResponseOnSecondRequest()
        {
            // Arrange
            var acccessToken = await GenerateNewClientAccessTokenAsync();
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/pagination");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", acccessToken);

            using var httpRequest2 = new HttpRequestMessage(HttpMethod.Post, "/order/pagination");
            httpRequest2.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", acccessToken);
            // Act 
            var httpResponse = await httpClient.SendAsync(httpRequest);
            await AddOrdersToClientAsync(acccessToken);
            var httpResponse2 = await httpClient.SendAsync(httpRequest2);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(httpResponse2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var firstContent = await httpResponse.Content.ReadAsStringAsync();
            var firstResponse = JsonSerializer.Deserialize<IEnumerable<OrderResponse>>(firstContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var secondContent = await httpResponse2.Content.ReadAsStringAsync();
            var secondResponse = JsonSerializer.Deserialize<IEnumerable<OrderResponse>>(secondContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(firstResponse, Is.EqualTo(secondResponse));
        }
        [Test]
        public async Task GetOrders_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/pagination");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
