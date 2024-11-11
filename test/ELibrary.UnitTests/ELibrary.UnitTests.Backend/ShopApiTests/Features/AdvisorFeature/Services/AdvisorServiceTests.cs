using Microsoft.Extensions.Configuration;
using Moq;
using Polly;
using Polly.Registry;
using Shared.Helpers;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;

namespace ShopApi.Features.AdvisorFeature.Services.Tests
{
    [TestFixture]
    internal class AdvisorServiceTests
    {

        private Mock<IHttpHelper> mockHttpHelper;
        private Mock<ResiliencePipelineProvider<string>> mockPipelineProvider;
        private AdvisorService advisorService;
        private ResiliencePipeline mockPipeline;
        private IConfiguration configuration;

        private const string ChatEndpoint = "/chat";
        private const string BotUrl = "https://fake-bot-url.com";

        [SetUp]
        public void SetUp()
        {
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                { $"{Configuration.CHAT_CONFIGURATION_SECTION}:BotUrl", BotUrl }
                })
                .Build();

            mockHttpHelper = new Mock<IHttpHelper>();

            mockPipeline = ResiliencePipeline.Empty;
            mockPipelineProvider = new Mock<ResiliencePipelineProvider<string>>();
            mockPipelineProvider.Setup(p => p.GetPipeline(It.IsAny<string>())).Returns(mockPipeline);

            advisorService = new AdvisorService(mockPipelineProvider.Object, mockHttpHelper.Object, configuration);
        }

        [Test]
        public async Task SendQueryAsync_ReturnsChatAdvisorResponse()
        {
            // Arrange
            var queryRequest = new ChatAdvisorQueryRequest
            {
                Query = "What are the latest trends in AI?"
            };
            var expectedResponse = new ChatAdvisorResponse
            {
                Message = "The latest trends in AI include advancements in deep learning and natural language processing."
            };
            mockHttpHelper.Setup(h => h.SendPostRequestAsync<ChatAdvisorResponse>(
                    $"{BotUrl}{ChatEndpoint}",
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(expectedResponse);
            // Act
            var result = await advisorService.SendQueryAsync(queryRequest, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Message, Is.EqualTo(expectedResponse.Message));
            mockHttpHelper.Verify(h => h.SendPostRequestAsync<ChatAdvisorResponse>(
                $"{BotUrl}{ChatEndpoint}",
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void SendQueryAsync_HandlesErrorGracefully()
        {
            // Arrange
            var queryRequest = new ChatAdvisorQueryRequest
            {
                Query = "What is the best way to learn programming?"
            };
            mockHttpHelper.Setup(h => h.SendPostRequestAsync<ChatAdvisorResponse>(
                    $"{BotUrl}{ChatEndpoint}",
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()
                ))
                .ThrowsAsync(new HttpRequestException("Network Error"));
            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(async () => await advisorService.SendQueryAsync(queryRequest, CancellationToken.None));
            mockHttpHelper.Verify(h => h.SendPostRequestAsync<ChatAdvisorResponse>(
                $"{BotUrl}{ChatEndpoint}",
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}