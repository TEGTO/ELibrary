using Azure.Search.Documents;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public interface ISearchClientFactory
    {
        public SearchClient CreateSearchClient();
    }
}