using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using Moq;
using ShopApi.Features.CartFeature.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.CartController
{
    internal class DeleteBooksFromCartCartControllerTests : BaseCartControllerTest
    {
        [Test]
        public async Task DeleteBooksFromCart_ValidRequest_ReturnsOkWithResponse()
        {
            // Arrange
            var accessToken = GetRandomAccessTokenData();
            await AddBooksToCartAsync(accessToken);
            var deleteBookFromCartRequests = new[] { new DeleteCartBookFromCartRequest() { Id = 1 } };
            using var request = new HttpRequestMessage(HttpMethod.Put, "/cart");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(deleteBookFromCartRequests), Encoding.UTF8, "application/json");
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var cartBookResponse = JsonSerializer.Deserialize<CartResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(cartBookResponse);
            Assert.That(cartBookResponse.Books.Count, Is.EqualTo(0));
            mockLibraryService.Verify(x => x.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.AtLeast(1));
        }
        [Test]
        public async Task DeleteBooksFromCart_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var updateCartBookRequest = new UpdateCartBookRequest();
            using var request = new HttpRequestMessage(HttpMethod.Put, "/cart");
            request.Content = new StringContent(JsonSerializer.Serialize(updateCartBookRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
