using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.CartController
{
    internal class GetInCartAmountCartControllerTests : BaseCartControllerTest
    {
        [Test]
        public async Task GetInCartAmount_ValidRequest_ReturnsOkWithResponse()
        {
            // Arrange
            var accessToken = GetRandomAccessTokenData();
            using var request = new HttpRequestMessage(HttpMethod.Get, "/cart/amount");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            await AddBooksToCartAsync(accessToken);
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var amountResponse = JsonSerializer.Deserialize<int>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(amountResponse, Is.EqualTo(1));
        }
        [Test]
        public async Task GetInCartAmount_ValidRequest_ReturnsOkWithCachedResponse()
        {
            // Arrange
            var accessToken = GetRandomAccessTokenData();
            using var request = new HttpRequestMessage(HttpMethod.Get, "/cart/amount");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            using var request2 = new HttpRequestMessage(HttpMethod.Get, "/cart/amount");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // Act
            var response = await httpClient.SendAsync(request);
            await AddBooksToCartAsync(accessToken);
            var response2 = await httpClient.SendAsync(request2);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var amountResponse = JsonSerializer.Deserialize<int>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(amountResponse, Is.EqualTo(0));

            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content2 = await response2.Content.ReadAsStringAsync();
            var amountResponse2 = JsonSerializer.Deserialize<int>(content2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.That(amountResponse2, Is.EqualTo(0));
        }
        [Test]
        public async Task GetInCartAmount_UnauthorizedRequest_ReturnsUnauthorized()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Get, "/cart/amount");
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
