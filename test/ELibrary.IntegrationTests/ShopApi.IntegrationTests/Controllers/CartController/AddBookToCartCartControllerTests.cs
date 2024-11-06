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
    internal class AddBookToCartCartControllerTests : BaseCartControllerTest
    {
        [Test]
        public async Task AddBookToCart_ValidRequest_ReturnsOkWithResponse()
        {
            // Arrange
            var accessToken = GetRandomAccessTokenData();
            var addBookToCartRequest = new AddBookToCartRequest() { BookId = 1, BookAmount = 10 };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/cart/cartbook");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(addBookToCartRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var cartBookResponse = JsonSerializer.Deserialize<CartBookResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(cartBookResponse);
            Assert.That(cartBookResponse.BookId, Is.EqualTo(1));
            Assert.That(cartBookResponse.BookAmount, Is.EqualTo(10));
            mockLibraryService.Verify(x => x.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.AtLeast(1));
        }
        [Test]
        public async Task AddBookToCart_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var accessToken = GetRandomAccessTokenData();
            var addBookToCartRequest = new AddBookToCartRequest() { BookId = -1, BookAmount = 10 };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/cart/cartbook");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(addBookToCartRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task AddBookToCart_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var addBookToCartRequest = new AddBookToCartRequest() { BookId = 1, BookAmount = 10 };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/cart/cartbook");
            request.Content = new StringContent(JsonSerializer.Serialize(addBookToCartRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task AddBookToCart_BookDoesntExist_ReturnsInternalServerError()
        {
            // Arrange
            var accessToken = GetRandomAccessTokenData();
            var addBookToCartRequest = new AddBookToCartRequest() { BookId = 100, BookAmount = 10 };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/cart/cartbook");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(addBookToCartRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }
    }
}
