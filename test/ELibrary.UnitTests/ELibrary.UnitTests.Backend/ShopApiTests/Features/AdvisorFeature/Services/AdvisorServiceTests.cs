namespace ShopApi.Features.AdvisorFeature.Services.Tests
{
    [TestFixture]
    internal class AdvisorServiceTests
    {
        //private Mock<ISearchClientFactory> mockSearchClientFactory;
        //private Mock<IChatService> mockChatService;
        //private Mock<SearchClient> mockSearchClient;
        //private AdvisorService advisorService;

        //[SetUp]
        //public void SetUp()
        //{
        //    var configuration = new ConfigurationBuilder()
        //       .AddInMemoryCollection(new Dictionary<string, string?>
        //       {
        //            {$"{Configuration.CHAT_CONFIGURATION_SECTION}:SearchApiKey", "test"},
        //            {$"{Configuration.CHAT_CONFIGURATION_SECTION}:SearchServiceEndpoint", "test"},
        //            {$"{Configuration.CHAT_CONFIGURATION_SECTION}:SearchIndexName", "test"},
        //            {$"{Configuration.CHAT_CONFIGURATION_SECTION}:ChatModel", "test"},
        //            {$"{Configuration.CHAT_CONFIGURATION_SECTION}:OpenAiApiKey", "test"},
        //            {$"{Configuration.CHAT_CONFIGURATION_SECTION}:GroundedPrompt", "test"},
        //       })
        //       .Build();

        //    mockSearchClientFactory = new Mock<ISearchClientFactory>();
        //    mockSearchClient = new Mock<SearchClient>();
        //    mockChatService = new Mock<IChatService>();

        //    var mockPipelineProvider = new Mock<ResiliencePipelineProvider<string>>();
        //    mockPipelineProvider.Setup(x => x.GetPipeline(It.IsAny<string>())).Returns(ResiliencePipeline.Empty);

        //    mockSearchClientFactory.Setup(x => x.CreateSearchClient()).Returns(mockSearchClient.Object);

        //    advisorService = new AdvisorService(configuration, mockSearchClientFactory.Object, mockChatService.Object, mockPipelineProvider.Object);
        //}

        //[Test]
        //public async Task SendQueryAsyncc_ReturnsChatResponse()
        //{
        //    // Arrange
        //    var query = "Find best hotels";
        //    var searchDocument = new SearchDocument(new Dictionary<string, object>
        //    {
        //        { "HotelName", "Hotel Paris" },
        //        { "Description", "Luxury hotel" },
        //        { "Tags", "Luxury, Paris" }
        //    });
        //    var mockResponse = new Mock<Response>();
        //    var mockResults = SearchModelFactory.SearchResults<SearchDocument>(new[]
        //    {
        //      SearchModelFactory.SearchResult<SearchDocument>(searchDocument , 1.0, null),
        //    }, 1, null, 1, rawResponse: mockResponse.Object);
        //    mockSearchClient.Setup(m => m.SearchAsync<SearchDocument>(It.IsAny<string>(), It.IsAny<SearchOptions>(), It.IsAny<CancellationToken>()))
        //      .ReturnsAsync(Response.FromValue(mockResults, mockResponse.Object));
        //    var chatResponse = "The best hotel based on your preferences is Hotel1.";
        //    mockChatService.Setup(s => s.GetChatCompletionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
        //                  .ReturnsAsync(chatResponse);
        //    // Act
        //    var result = await advisorService.SendQueryAsync(query, CancellationToken.None);
        //    // Assert
        //    Assert.That(result, Is.EqualTo(chatResponse));
        //}
        //[Test]
        //public async Task GetSourcesAsync_ShouldFormatSearchResultsCorrectly()
        //{
        //    // Arrange
        //    var query = "Best hotels in Paris";
        //    var searchDocument = new SearchDocument(new Dictionary<string, object>
        //    {
        //        { "HotelName", "Hotel Paris" },
        //        { "Description", "Luxury hotel" },
        //        { "Tags", "Luxury, Paris" }
        //    });
        //    var mockResponse = new Mock<Response>();
        //    var mockResults = SearchModelFactory.SearchResults<SearchDocument>(new[]
        //    {
        //      SearchModelFactory.SearchResult<SearchDocument>(searchDocument , 1.0, null),
        //    }, 1, null, 1, rawResponse: mockResponse.Object);
        //    mockSearchClient.Setup(m => m.SearchAsync<SearchDocument>(It.IsAny<string>(), It.IsAny<SearchOptions>(), It.IsAny<CancellationToken>()))
        //      .ReturnsAsync(Response.FromValue(mockResults, mockResponse.Object));
        //    // Act
        //    MethodInfo methodInfo = advisorService.GetType().GetMethod("GetSourcesAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        //    var result = await (Task<StringBuilder>)methodInfo.Invoke(advisorService, new object[] { query, CancellationToken.None });
        //    // Assert
        //    var expectedOutput = "Hotel Paris:Luxury hotel:Luxury, Paris\r\n";
        //    Assert.That(result.ToString(), Is.EqualTo(expectedOutput));
        //}
        //[Test]
        //public async Task GetChatResponseAsync_ShouldReturnResponse_FromChatService()
        //{
        //    // Arrange
        //    var query = "Best hotels in Paris";
        //    var sourcesFormatted = new StringBuilder("Hotel Paris:Luxury hotel:Luxury, Paris");
        //    var chatResponseText = "The best hotel in Paris is Hotel Paris.";
        //    mockChatService.Setup(s => s.GetChatCompletionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
        //                   .ReturnsAsync(chatResponseText);
        //    // Act
        //    MethodInfo methodInfo = advisorService.GetType().GetMethod("GetChatResponseAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        //    var result = await (Task<string>)methodInfo.Invoke(advisorService, new object[] { query, sourcesFormatted, CancellationToken.None });
        //    // Assert
        //    Assert.That(result, Is.EqualTo(chatResponseText));
        //}
    }
}