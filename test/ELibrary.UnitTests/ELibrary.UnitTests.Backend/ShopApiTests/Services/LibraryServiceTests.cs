using AutoMapper;
using LibraryShopEntities.Domain.Dtos.SharedRequests;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.Extensions.Configuration;
using Moq;
using Polly;
using Polly.Registry;
using Shared.Helpers;

namespace ShopApi.Services.Tests
{
    [TestFixture]
    internal class LibraryServiceTests
    {
        private Mock<IHttpHelper> mockHttpHelper;
        private Mock<ResiliencePipelineProvider<string>> mockResiliencePipelineProvider;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<IMapper> mockMapper;
        private LibraryService service;

        [SetUp]
        public void SetUp()
        {
            mockHttpHelper = new Mock<IHttpHelper>();
            mockConfiguration = new Mock<IConfiguration>();
            mockResiliencePipelineProvider = new Mock<ResiliencePipelineProvider<string>>();
            mockMapper = new Mock<IMapper>();

            mockConfiguration.Setup(config => config[Configuration.LIBRARY_API_URL])
                .Returns("https://mockapi.com");

            mockResiliencePipelineProvider.Setup(provider => provider.GetPipeline(Configuration.DEFAULT_RESILIENCE_PIPELINE))
               .Returns(ResiliencePipeline.Empty);

            service = new LibraryService(mockResiliencePipelineProvider.Object, mockHttpHelper.Object, mockMapper.Object, mockConfiguration.Object);
        }

        [Test]
        public async Task GetByIdsAsync_ValidIdsAndEndpoint_ReturnsExpectedResult()
        {
            // Arrange
            var ids = new List<int> { 1, 2, 3 };
            var endpoint = "/book/ids";
            var cancellationToken = CancellationToken.None;
            var expectedResponse = new List<string> { "Item1", "Item2" };

            mockHttpHelper.Setup(http => http.SendPostRequestAsync<IEnumerable<string>>(
                It.Is<string>(url => url == "https://mockapi.com/book/ids"),
                It.Is<string>(json => json.Contains("\"Ids\":[1,2,3]")),
                null, cancellationToken))
            .ReturnsAsync(expectedResponse);
            // Act
            var result = await service.GetByIdsAsync<string>(ids, endpoint, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(expectedResponse));
            mockHttpHelper.Verify(http => http.SendPostRequestAsync<IEnumerable<string>>(
                "https://mockapi.com/book/ids", It.IsAny<string>(), null, cancellationToken), Times.Once);
        }
        [Test]
        public async Task RaiseBookPopularityByIdsAsync_ValidIds_CallsExpectedEndpoint()
        {
            // Arrange
            var ids = new List<int> { 1, 2, 3 };
            var cancellationToken = CancellationToken.None;
            mockHttpHelper.Setup(http => http.SendPostRequestAsync<string>(
                It.Is<string>(url => url == "https://mockapi.com/book/popularity"),
                It.Is<string>(json => json.Contains("\"Ids\":[1,2,3]")),
                null, cancellationToken))
            .ReturnsAsync("Success");
            // Act
            await service.RaiseBookPopularityByIdsAsync(ids, cancellationToken);
            // Assert
            mockHttpHelper.Verify(http => http.SendPostRequestAsync<string>(
                "https://mockapi.com/book/popularity", It.IsAny<string>(), null, cancellationToken), Times.Once);
        }
        [Test]
        public async Task UpdateBookStockAmount_ValidChanges_CallsExpectedEndpoint()
        {
            // Arrange
            var changes = new List<StockBookChange>
            {
                new StockBookChange { BookId = 1, ChangeAmount = 10 },
                new StockBookChange { BookId = 2, ChangeAmount = -5 }
            };
            var cancellationToken = CancellationToken.None;
            mockMapper.Setup(m => m.Map<UpdateBookStockAmountRequest>(It.IsAny<StockBookChange>()))
                .Returns((StockBookChange change) => new UpdateBookStockAmountRequest { BookId = change.BookId, ChangeAmount = change.ChangeAmount });
            mockHttpHelper.Setup(http => http.SendPostRequestAsync<string>(
                It.Is<string>(url => url == "https://mockapi.com/book/stockamount"),
                It.Is<string>(json => json.Contains("\"BookId\":1") && json.Contains("\"Amount\":10")),
                null, cancellationToken))
            .ReturnsAsync("Success");
            // Act
            await service.UpdateBookStockAmountAsync(changes, cancellationToken);
            // Assert
            mockHttpHelper.Verify(http => http.SendPostRequestAsync<string>(
                "https://mockapi.com/book/stockamount", It.IsAny<string>(), null, cancellationToken), Times.Once);
        }
    }
}