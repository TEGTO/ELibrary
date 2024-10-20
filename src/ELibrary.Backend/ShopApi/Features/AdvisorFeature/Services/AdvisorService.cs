using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Polly;
using Polly.Registry;
using System.Text;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public class AdvisorService : IAdvisorService
    {
        private readonly ChatConfiguration chatConfig;
        private readonly ISearchClientFactory searchClientFactory;
        private readonly IChatService chatService;
        private readonly ResiliencePipeline resiliencePipeline;

        public AdvisorService(IConfiguration configuration, ISearchClientFactory searchClientFactory, IChatService chatService, ResiliencePipelineProvider<string> resiliencePipelineProvider)
        {
            chatConfig = configuration.GetSection(Configuration.CHAT_CONFIGURATION_SECTION)
                                          .Get<ChatConfiguration>()!;
            this.searchClientFactory = searchClientFactory;
            this.chatService = chatService;
            resiliencePipeline = resiliencePipelineProvider.GetPipeline(Configuration.DEFAULT_RESILIENCE_PIPELINE);
        }
        public async Task<string> SendQueryAsync(string query, CancellationToken cancellationToken)
        {
            var sourcesFormatted = await GetSourcesAsync(query, cancellationToken);
            return await GetChatResponseAsync(query, sourcesFormatted, cancellationToken);
        }
        private async Task<StringBuilder> GetSourcesAsync(string query, CancellationToken cancellationToken)
        {
            return await resiliencePipeline.ExecuteAsync(async (ct) =>
            {
                var searchClient = searchClientFactory.CreateSearchClient();

                var searchOptions = new SearchOptions
                {
                    Size = 5,
                    Select = { "Description", "HotelName", "Tags" }
                };

                var searchResults = await searchClient.SearchAsync<SearchDocument>(query, searchOptions, cancellationToken);

                var sourcesFormatted = new StringBuilder();
                foreach (var result in searchResults.Value.GetResults())
                {
                    string hotelName = result.Document["HotelName"]?.ToString();
                    string description = result.Document["Description"]?.ToString();
                    string tags = result.Document["Tags"]?.ToString();

                    sourcesFormatted.AppendLine($"{hotelName}:{description}:{tags}");
                }
                return sourcesFormatted;

            }, cancellationToken);
        }
        private async Task<string> GetChatResponseAsync(string query, StringBuilder sourcesFormatted, CancellationToken cancellationToken)
        {
            return await resiliencePipeline.ExecuteAsync(async (ct) =>
            {
                var prompt = chatConfig.GroundedPrompt.Replace("{sourcesFormatted}", sourcesFormatted.ToString());
                return await chatService.GetChatCompletionAsync(prompt, query, cancellationToken);
            }, cancellationToken);
        }
    }
}

