using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.StockBookOrderFeature.Dtos;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.StockBookOrderController
{
    internal class BaseStockBookOrderControllerTest : BaseControllerTest
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
        }

        protected async Task<StockBookOrderResponse> CreateStockBookOrderAsync(string accessToken)
        {
            var client = await TestHelper.CreateClientAsync(ManagerAccessToken, httpClient);
            var request = new CreateStockBookOrderRequest()
            {
                Type = StockBookOrderType.ManagerOrderCancel,
                ClientId = client.Id,
                StockBookChanges = new List<StockBookChangeRequest> { new StockBookChangeRequest { BookId = 1, ChangeAmount = 10 } }
            };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/stockbook");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var httpResponse = await httpClient.SendAsync(httpRequest);
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<StockBookOrderResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return response;
        }
    }
}
