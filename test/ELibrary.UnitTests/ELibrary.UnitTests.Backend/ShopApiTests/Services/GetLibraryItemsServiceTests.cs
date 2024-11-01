using Microsoft.Extensions.Configuration;
using Moq;
using Polly;
using Polly.Registry;
using Shared.Helpers;

namespace ShopApi.Services.Tests
{
    [TestFixture]
    internal class GetLibraryItemsServiceTests
    {
        private Mock<IHttpHelper> mockHttpHelper;
        private Mock<ResiliencePipelineProvider<string>> mockResiliencePipelineProvider;
        private Mock<IConfiguration> mockConfiguration;
        private GetLibraryItemsService service;

        [SetUp]
        public void SetUp()
        {
            mockHttpHelper = new Mock<IHttpHelper>();
            mockConfiguration = new Mock<IConfiguration>();
            mockResiliencePipelineProvider = new Mock<ResiliencePipelineProvider<string>>();

            mockConfiguration.Setup(config => config[Configuration.LIBRARY_API_URL])
                .Returns("https://mockapi.com");

            mockResiliencePipelineProvider.Setup(provider => provider.GetPipeline(Configuration.DEFAULT_RESILIENCE_PIPELINE))
                .Returns(ResiliencePipeline.Empty);

            service = new GetLibraryItemsService(mockResiliencePipelineProvider.Object, mockHttpHelper.Object, mockConfiguration.Object);
        }

        [Test]
        public async Task GetByIdsAsync_ValidIdsAndEndpoint_ReturnsExpectedResult()
        {
            // Arrange
            var ids = new List<int> { 1, 2, 3 };
            var endpoint = "/items";
            var cancellationToken = CancellationToken.None;
            var expectedResponse = new List<string> { "Item1", "Item2" };
            mockHttpHelper.Setup(http => http.SendPostRequestAsync<IEnumerable<string>>(
                It.Is<string>(url => url == "https://mockapi.com/items"),
                It.Is<string>(json => json.Contains("\"Ids\":[1,2,3]")),
                null, cancellationToken))
            .ReturnsAsync(expectedResponse);
            // Act
            var result = await service.GetByIdsAsync<string>(ids, endpoint, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(expectedResponse));
            mockHttpHelper.Verify(http => http.SendPostRequestAsync<IEnumerable<string>>(
                "https://mockapi.com/items", It.IsAny<string>(), null, cancellationToken), Times.Once);
        }
    }
}