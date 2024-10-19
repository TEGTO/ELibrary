using Azure;
using Azure.Search.Documents;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public class SearchClientFactory : ISearchClientFactory
    {
        private readonly ChatConfiguration chatConfig;

        public SearchClientFactory(IConfiguration configuration)
        {
            chatConfig = configuration.GetSection(Configuration.CHAT_CONFIGURATION_SECTION)
                                          .Get<ChatConfiguration>()!;
        }

        public SearchClient CreateSearchClient()
        {
            return new SearchClient(new Uri(chatConfig.SearchServiceEndpoint), chatConfig.SearchIndexName, new AzureKeyCredential(chatConfig.SearchApiKey));
        }
    }
}
