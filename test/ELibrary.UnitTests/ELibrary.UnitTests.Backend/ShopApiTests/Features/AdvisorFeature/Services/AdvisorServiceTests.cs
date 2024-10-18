using Microsoft.Extensions.Configuration;

namespace ShopApi.Features.AdvisorFeature.Services.Tests
{
    [TestFixture]
    internal class AdvisorServiceTests
    {
        private AdvisorService advisorService;

        [SetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder()
               .AddInMemoryCollection(new Dictionary<string, string?>
               {
                    {$"{Configuration.CHAT_CONFIGURATION_SECTION}:SearchApiKey", "test"},
                    {$"{Configuration.CHAT_CONFIGURATION_SECTION}:SearchServiceEndpoint", "test"},
                    {$"{Configuration.CHAT_CONFIGURATION_SECTION}:SearchIndexName", "test"},
                    {$"{Configuration.CHAT_CONFIGURATION_SECTION}:ChatModel", "test"},
                    {$"{Configuration.CHAT_CONFIGURATION_SECTION}:OpenAiApiKey", "test"},
                    {$"{Configuration.CHAT_CONFIGURATION_SECTION}:GroundedPrompt", "test"},
               })
               .Build();

            advisorService = new AdvisorService(configuration);
        }

    }
}