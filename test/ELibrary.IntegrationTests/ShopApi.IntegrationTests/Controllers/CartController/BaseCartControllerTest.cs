using Authentication.Identity;
using Authentication.Token;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using Microsoft.AspNetCore.Identity;
using Moq;
using ShopApi.Features.CartFeature.Dtos;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.CartController
{
    internal class BaseCartControllerTest : BaseControllerTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mockLibraryService.Setup(x => x.GetByIdsAsync<BookResponse>(
              It.Is<List<int>>(ids => ids.Contains(100)),
              It.IsAny<string>(),
              It.IsAny<CancellationToken>()
            )).ThrowsAsync(new Exception("Invalid ID: 100"));

            mockLibraryService.Setup(x => x.GetByIdsAsync<BookResponse>(
                It.Is<List<int>>(ids => !ids.Contains(100)),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<BookResponse> { new BookResponse { Id = 1, Name = "Test" } });

            mockLibraryService.Setup(x => x.GetByIdsAsync<BookResponse>(
             It.Is<List<int>>(ids => ids.Contains(50)),
             It.IsAny<string>(),
             It.IsAny<CancellationToken>()
         )).ReturnsAsync(new List<BookResponse>());
        }

        protected async Task<CartBookResponse> AddBooksToCartAsync(string accessToken)
        {
            var addRequest = new AddBookToCartRequest() { BookId = 1, BookAmount = 1 };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/cart/cartbook");
            request.Content = new StringContent(JsonSerializer.Serialize(addRequest), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var cartBookResponse = JsonSerializer.Deserialize<CartBookResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return cartBookResponse;
        }
        protected string GetRandomAccessTokenData()
        {
            var random = new Random();
            var jwtHandler = new JwtHandler(settings);
            IdentityUser identity = new IdentityUser()
            {
                Id = random.Next(int.MaxValue).ToString(),
                UserName = "testuser",
                Email = "test@example.com"
            };
            return jwtHandler.CreateToken(identity, [Roles.CLIENT]).AccessToken;
        }
    }
}
