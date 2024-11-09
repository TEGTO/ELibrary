using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using Moq;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.CartController
{
    internal class GetCartCartControllerTests : BaseCartControllerTest
    {
        [Test]
        public async Task GetCart_ValidRequest_ReturnsOkWithResponse()
        {
            // Arrange
            var accessToken = GetRandomAccessTokenData();
            using var request = new HttpRequestMessage(HttpMethod.Get, "/cart");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            await AddBooksToCartAsync(accessToken);
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var cartResponse = JsonSerializer.Deserialize<CartResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(cartResponse);
            Assert.NotNull(cartResponse.Books);
            Assert.That(cartResponse.Books.Count, Is.EqualTo(1));
            mockLibraryService.Verify(x => x.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.AtLeast(1));
        }
        [Test]
        public async Task GetCart_ValidRequest_ReturnsOkWithCachedResponse()
        {
            // Arrange
            var accessToken = GetRandomAccessTokenData();
            using var request = new HttpRequestMessage(HttpMethod.Get, "/cart");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            using var request2 = new HttpRequestMessage(HttpMethod.Get, "/cart");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // Act
            var response = await httpClient.SendAsync(request);
            await AddBooksToCartAsync(accessToken);
            var response2 = await httpClient.SendAsync(request2);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var cartResponse = JsonSerializer.Deserialize<CartResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(cartResponse);
            Assert.NotNull(cartResponse.Books);
            Assert.That(cartResponse.Books.Count, Is.EqualTo(0));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content2 = await response2.Content.ReadAsStringAsync();
            var cartResponse2 = JsonSerializer.Deserialize<CartResponse>(content2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(cartResponse2);
            Assert.NotNull(cartResponse2.Books);
            Assert.That(cartResponse2.Books.Count, Is.EqualTo(0));
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public async Task GetCart_UnauthorizedRequest_ReturnsUnauthorized()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Get, "/cart");
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
