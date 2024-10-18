using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using OpenAI.Chat;
using System.Text;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public class AdvisorService : IAdvisorService
    {
        private readonly ChatConfiguration chatConfig;

        public AdvisorService(IConfiguration configuration)
        {
            chatConfig = configuration.GetSection(Configuration.CHAT_CONFIGURATION_SECTION)
                                          .Get<ChatConfiguration>()!;
        }
        public async Task<string> SendQueryAsync(string query, CancellationToken cancellationToken)
        {
            var sourcesFormatted = await GetSourcesAsync(query, cancellationToken);
            return await GetChatResponseAsync(query, sourcesFormatted, cancellationToken);
        }
        private async Task<StringBuilder> GetSourcesAsync(string query, CancellationToken cancellationToken)
        {
            AzureKeyCredential searchCredential = new AzureKeyCredential(chatConfig.SearchApiKey);
            var searchClient = new SearchClient(new Uri(chatConfig.SearchServiceEndpoint), chatConfig.SearchIndexName, searchCredential);

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
                var chatClient = new ChatClient(chatConfig.ChatModel, apiKey: chatConfig.OpenAiApiKey);

                var promt = chatConfig.GroundedPrompt.Replace("{sourcesFormatted}", sourcesFormatted.ToString());

                var chatCompletion = await chatClient.CompleteChatAsync(
                    new SystemChatMessage(promt),
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

