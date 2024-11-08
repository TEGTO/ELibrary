using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Filters;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.OrderController
{
    internal class ManagerGetPaginatedOrdersOrderControllerTests : BaseOrderControllerTest
    {
        [Test]
        public async Task ManagerGetPaginatedOrders_ValidRequest_ReturnsOkWithOrderList()
        {
            // Arrange
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/manager/pagination");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
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
        public async Task ManagerGetPaginatedOrders_InvalidPagination_ReturnsBadRequest()
        {
            // Arrange
            var request = new GetOrdersFilter { PageNumber = -1, PageSize = 0 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/manager/pagination");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ManagerGetPaginatedOrders_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/manager/pagination");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task ManagerGetPaginatedOrders_Forbidden_ReturnsForbidden()
        {
            // Arrange
            var clientAccessToken = await GenerateNewClientAccessTokenAsync();
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/manager/pagination");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", clientAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
