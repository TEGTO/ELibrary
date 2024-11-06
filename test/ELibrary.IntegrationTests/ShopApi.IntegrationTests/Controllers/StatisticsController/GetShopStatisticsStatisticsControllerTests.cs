using LibraryShopEntities.Domain.Dtos.Library;
using Microsoft.AspNetCore.Http;
using Moq;
using ShopApi.Features.CartFeature.Dtos;
using ShopApi.Features.StatisticsFeature.Domain.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.StatisticsController
{
    internal class GetShopStatisticsStatisticsControllerTests : BaseControllerTest
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await AddSamplesAsync();
        }

        [SetUp]
        public void SetUp()
        {
            mockCachingHelper.Setup(x => x.GetCacheKey(It.IsAny<string>(), It.IsAny<HttpContext>()))
                .Returns(Guid.NewGuid().ToString());
        }

        [TestCaseSource(nameof(AddOrderDataInvalidOrderDetails))]
        public async Task GetShopStatistics_Valid_ReturnsExpectedResults(GetShopStatisticsRequest request, int expectedCanceledCopies, int expectedInCartCopies, int expectedInOrderCopies, decimal expectedEarnedMoney, decimal expectedAveragePrice, int expectedCanceledOrderAmount, int expectedSoldCopies)
        {
            // Act
            var response = await SendStatisticsRequestAsync(request);
            // Assert
            AssertValidStatisticsResponse(response, expectedCanceledCopies, expectedInCartCopies, expectedInOrderCopies, expectedEarnedMoney, expectedAveragePrice, expectedCanceledOrderAmount, expectedSoldCopies);
        }
        [TestCase]
        public async Task GetShopStatistics_Invalid_ReturnsBadRequest()
        {
            //Arrange
            var request = new GetShopStatisticsRequest();
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/statistics");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            //Act
            var response = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [TestCase]
        public async Task GetShopStatistics_Unauthorized_ReturnsUnauthorized()
        {
            //Arrange
            var request = new GetShopStatisticsRequest();
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/statistics");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            //Act
            var response = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [TestCase]
        public async Task GetShopStatistics_ValidRequest_ReturnsCachedResponse()
        {
            //Arrange
            var request = new GetShopStatisticsRequest() { IncludeBooks = Array.Empty<StatisticsBookRequest>() };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/statistics");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            using var httpRequest2 = new HttpRequestMessage(HttpMethod.Post, "/statistics");
            httpRequest2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            httpRequest2.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            //Act
            var response = await httpClient.SendAsync(httpRequest);
            var response2 = await httpClient.SendAsync(httpRequest2);
            // Assert
            var content = await response.Content.ReadAsStringAsync();
            var statistics = JsonSerializer.Deserialize<ShopStatisticsResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var content2 = await response2.Content.ReadAsStringAsync();
            var statistics2 = JsonSerializer.Deserialize<ShopStatisticsResponse>(content2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.That(statistics.InCartCopies, Is.EqualTo(statistics2.InCartCopies));
        }

        private static IEnumerable<TestCaseData> AddOrderDataInvalidOrderDetails()
        {
            yield return new TestCaseData(
               new GetShopStatisticsRequest
               {
                   IncludeBooks = Array.Empty<StatisticsBookRequest>()
               },
               1, 10, 2, 500m, 500m, 1, 1
           );

            yield return new TestCaseData(
                new GetShopStatisticsRequest
                {
                    FromUTC = new DateTime(2024, 10, 1),
                    ToUTC = new DateTime(2024, 10, 31),
                    IncludeBooks = new[] { new StatisticsBookRequest { Id = 1 } }
                },
                0, 10, 0, 0m, 0m, 0, 0
            );

            yield return new TestCaseData(
                new GetShopStatisticsRequest
                {
                    FromUTC = new DateTime(2024, 11, 1),
                    ToUTC = new DateTime(2024, 11, 30),
                    IncludeBooks = Array.Empty<StatisticsBookRequest>()
                },
                0, 10, 1, 500m, 500m, 0, 1
            );
        }
        private async Task<ShopStatisticsResponse> SendStatisticsRequestAsync(GetShopStatisticsRequest request)
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/statistics");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ShopStatisticsResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        private void AssertValidStatisticsResponse(ShopStatisticsResponse statistics, int canceledCopies, int inCartCopies, int inOrderCopies, decimal earnedMoney, decimal averagePrice, int canceledOrderAmount, int soldCopies)
        {
            Assert.NotNull(statistics);
            Assert.That(statistics.CanceledCopies, Is.EqualTo(canceledCopies));
            Assert.That(statistics.InCartCopies, Is.EqualTo(inCartCopies));
            Assert.That(statistics.InOrderCopies, Is.EqualTo(inOrderCopies));
            Assert.That(statistics.EarnedMoney, Is.EqualTo(earnedMoney));
            Assert.That(statistics.AveragePrice, Is.EqualTo(averagePrice));
            Assert.That(statistics.CanceledOrderAmount, Is.EqualTo(canceledOrderAmount));
            Assert.That(statistics.SoldCopies, Is.EqualTo(soldCopies));
        }
        private async Task AddSamplesAsync()
        {
            await AddBookToCartAsync();

            var client = await TestHelper.CreateClientAsync(ManagerAccessToken, httpClient);

            await GetPostgreSqlContainer().ExecScriptAsync(
                $"INSERT INTO public.orders (id, created_at, updated_at, order_amount, total_price, delivery_address, delivery_time, order_status, payment_method, delivery_method, client_id) VALUES(1, '2024-11-02 14:20:58.870', '2024-11-02 14:20:58.871', 1, 500.00, '2', '2024-11-03 06:30:00.000', 1, 0, 0, '{client.Id}');" +
                $"INSERT INTO public.orders (id, created_at, updated_at, order_amount, total_price, delivery_address, delivery_time, order_status, payment_method, delivery_method, client_id) VALUES(2, '2024-10-02 14:20:58.870', '2024-10-02 14:20:58.871', 1, 500.00, '2', '2024-10-03 06:30:00.000', -1, 0, 0, '{client.Id}');" +
                $"INSERT INTO public.order_books (id, book_amount, book_id, book_price, order_id) VALUES('0b0b8823-99dd-4cb2-809c-757b4d36ce0d', 1, 1, 500.00, 1);");

        }
        private async Task AddBookToCartAsync()
        {
            mockLibraryService.Setup(x => x.GetByIdsAsync<BookResponse>(
              It.IsAny<List<int>>(),
              It.IsAny<string>(),
              It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<BookResponse> { new BookResponse { Id = 1, Name = "Test" } });

            var addBookToCartRequest = new AddBookToCartRequest() { BookId = 1, BookAmount = 10 };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/cart/cartbook");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(addBookToCartRequest), Encoding.UTF8, "application/json");
            await httpClient.SendAsync(request);
        }
    }
}
