using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using OpenAI.Chat;
using System.Text;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public class AdvisorService : IAdvisorService
    {
        private readonly string searchServiceEndpoint;
        private readonly string searchIndexName;
        private readonly string searchApiKey;
        private readonly string openAiEndpoint;
        private readonly string openAiApiKey;
        private readonly string chatModel;
        private string groundedPrompt;

        public AdvisorService(IConfiguration configuration)
        {
            searchServiceEndpoint = configuration[Configuration.CHAT_SERACH_SERVICE_ENDPOINT]!;
            searchIndexName = configuration[Configuration.CHAT_SEARCH_INDEX_NAME]!;
            searchApiKey = configuration[Configuration.CHAT_SEARCH_API_KEY]!;
            openAiEndpoint = configuration[Configuration.CHAT_OPEN_AI_ENDPOINT]!;
            chatModel = configuration[Configuration.CHAT_OPENAI_CHAT_MODEL]!;
            groundedPrompt = configuration[Configuration.CHAT_OPENAI_GROUNDED_PROMPT]!;
            openAiApiKey = configuration[Configuration.CHAT_OPENAI_API_KEY]!;
        }

        public async Task<string> AskQueryAsync(string query, CancellationToken cancellationToken)
        {
            var sourcesFormatted = await GetSourcesAsync(query, cancellationToken);
            return await GetChatResponseAsync(query, sourcesFormatted, cancellationToken);
        }
        private async Task<StringBuilder> GetSourcesAsync(string query, CancellationToken cancellationToken)
        {
            AzureKeyCredential searchCredential = new AzureKeyCredential(searchApiKey);
            var searchClient = new SearchClient(new Uri(searchServiceEndpoint), searchIndexName, searchCredential);

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
        }
        private async Task<string> GetChatResponseAsync(string query, StringBuilder sourcesFormatted, CancellationToken cancellationToken)
        {
            try
            {
                var chatClient = new ChatClient(chatModel, apiKey: openAiApiKey);

                groundedPrompt = groundedPrompt.Replace("{sourcesFormatted}", sourcesFormatted.ToString());

                var chatCompletion = await chatClient.CompleteChatAsync(
                    new SystemChatMessage(groundedPrompt),
                    new UserChatMessage(query)
                );

                return chatCompletion.Value.Content[0].Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}

