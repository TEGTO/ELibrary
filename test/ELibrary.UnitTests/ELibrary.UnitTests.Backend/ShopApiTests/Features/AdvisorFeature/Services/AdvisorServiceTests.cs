using LangChain.DocumentLoaders;
using Microsoft.Extensions.Configuration;
using Moq;
using Polly;
using Polly.Registry;
using System.Text;

namespace ShopApi.Features.AdvisorFeature.Services.Tests
{
    [TestFixture]
    internal class AdvisorServiceTests
    {
        private Mock<IChatService> mockChatService;
        private Mock<ResiliencePipelineProvider<string>> mockPipelineProvider;
        private AdvisorService advisorService;
        private ResiliencePipeline mockPipeline;
        private List<Document> mockDocuments;
        private IConfiguration configuration;

        [SetUp]
        public void SetUp()
        {
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { $"{Configuration.CHAT_CONFIGURATION_SECTION}:GetDocumentsCacheTimeInMinutes", "10" }
                })
                .Build();

            mockChatService = new Mock<IChatService>();

            mockPipeline = ResiliencePipeline.Empty;
            mockPipelineProvider = new Mock<ResiliencePipelineProvider<string>>();
            mockPipelineProvider.Setup(p => p.GetPipeline(It.IsAny<string>())).Returns(mockPipeline);

            advisorService = new AdvisorService(mockChatService.Object, mockPipelineProvider.Object, configuration);

            mockDocuments = new List<Document>
            {
                new Document("Sample content 1", new Dictionary<string, object> { { "bookId", 1 } }),
                new Document("Sample content 2", new Dictionary<string, object> { { "bookId", 2 } })
            };
        }

        [Test]
        public async Task SendQueryAsync_ReturnsChatResponse()
        {
            // Arrange
            string query = "Tell me about book 1.";
            string chatResponse = "Book 1 is great!";
            mockChatService.Setup(s => s.AskQuestionAsync(query, mockDocuments, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new StringBuilder(chatResponse));
            mockChatService.Setup(s => s.GetDocumentsAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(mockDocuments);
            // Act
            var result = await advisorService.SendQueryAsync(query, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(chatResponse));
        }
        [Test]
        public async Task GetCachedDocumentsAsync_UpdatesCache_WhenExpired()
        {
            // Arrange
            await Task.Delay(1);

            mockChatService.Setup(s => s.GetDocumentsAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(mockDocuments);
            mockChatService.Setup(s => s.AskQuestionAsync(It.IsAny<string>(), It.IsAny<List<Document>>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new StringBuilder());
            // Act
            await advisorService.SendQueryAsync("Test query", CancellationToken.None);
            // Assert
            mockChatService.Verify(s => s.GetDocumentsAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task GetCachedDocumentsAsync_UsesCache_IfNotExpired()
        {
            // Arrange
            mockChatService.Setup(s => s.GetDocumentsAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(mockDocuments);
            mockChatService.Setup(s => s.AskQuestionAsync(It.IsAny<string>(), It.IsAny<List<Document>>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new StringBuilder());
            await advisorService.SendQueryAsync("Prime cache", CancellationToken.None);
            // Act
            await advisorService.SendQueryAsync("Another query", CancellationToken.None);
            // Assert
            mockChatService.Verify(s => s.GetDocumentsAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}