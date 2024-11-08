using LibraryShopEntities.Filters;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.OrderController
{
    internal class GetOrderAmountOrderControllerTests : BaseOrderControllerTest
    {
        [Test]
        public async Task GetOrderAmount_ClientWithOrders_ReturnsOkWithAmount()
        {
            // Arrange
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/amount");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<int>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(response, Is.EqualTo(2));
        }
        [Test]
        public async Task GetOrderAmount_ClientWithoutOrders_ReturnsOkWithZeroAmount()
        {
            // Arrange
            var acccessToken = await GenerateNewClientAccessTokenAsync();
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/amount");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", acccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<int>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(response, Is.EqualTo(0));
        }
        [Test]
        public async Task GetOrderAmount_Caching_ReturnsCachedResponseOnSecondRequest()
        {
            // Arrange
            var acccessToken = await GenerateNewClientAccessTokenAsync();
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/amount");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", acccessToken);

            using var httpRequest2 = new HttpRequestMessage(HttpMethod.Post, "/order/amount");
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
            var firstResponse = JsonSerializer.Deserialize<int>(firstContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var secondContent = await httpResponse2.Content.ReadAsStringAsync();
            var secondResponse = JsonSerializer.Deserialize<int>(secondContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(firstResponse, Is.EqualTo(secondResponse));
        }
        [Test]
        public async Task GetOrderAmount_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var request = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/order/amount");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
