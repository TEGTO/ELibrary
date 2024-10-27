using LangChain.DocumentLoaders;
using Polly;
using Polly.Registry;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public class AdvisorService : IAdvisorService
    {
        private readonly IChatService chatService;
        private readonly ResiliencePipeline resiliencePipeline;
        private List<Document> cachedDocuments = new List<Document>();
        private DateTime lastUpdate = DateTime.MinValue;
        private readonly TimeSpan cacheDuration;

        public AdvisorService(IChatService chatService, ResiliencePipelineProvider<string> resiliencePipelineProvider, IConfiguration configuration)
        {
            this.chatService = chatService;
            resiliencePipeline = resiliencePipelineProvider.GetPipeline(Configuration.DEFAULT_RESILIENCE_PIPELINE);

            var chatConfig = configuration.GetSection(Configuration.CHAT_CONFIGURATION_SECTION)
                                    .Get<ChatConfiguration>()!;

            cacheDuration = TimeSpan.FromMinutes(chatConfig.GetDocumentsCacheTimeInMinutes);
        }

        public async Task<string> SendQueryAsync(string query, CancellationToken cancellationToken)
        {
            return await GetChatResponseAsync(query, cancellationToken);
        }

        private async Task<string> GetChatResponseAsync(string query, CancellationToken cancellationToken)
        {
            return await resiliencePipeline.ExecuteAsync(async (ct) =>
            {
                var documents = await GetCachedDocumentsAsync(cancellationToken);
                return (await chatService.AskQuestionAsync(query, documents, cancellationToken)).ToString();
            }, cancellationToken);
        }
        private async ValueTask<List<Document>> GetCachedDocumentsAsync(CancellationToken cancellationToken)
        {
            if ((DateTime.UtcNow - lastUpdate) > cacheDuration)
            {
                cachedDocuments = await chatService.GetDocumentsAsync(cancellationToken);
                lastUpdate = DateTime.UtcNow;
            }

            return cachedDocuments;
        }
    }
}

