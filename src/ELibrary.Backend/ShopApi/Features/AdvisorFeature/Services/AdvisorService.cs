using Polly;
using Polly.Registry;
using Shared.Helpers;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;
using System.Text.Json;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public class AdvisorService : IAdvisorService
    {
        private const string CHAT_ADVISOR_ENDPOINT = "/advisor";

        private readonly ResiliencePipeline resiliencePipeline;
        private readonly IHttpHelper httpHelper;
        private readonly ChatConfiguration chatConfig;

        public AdvisorService(ResiliencePipelineProvider<string> resiliencePipelineProvider, IHttpHelper httpHelper, IConfiguration configuration)
        {
            resiliencePipeline = resiliencePipelineProvider.GetPipeline(Configuration.DEFAULT_RESILIENCE_PIPELINE);

            chatConfig = configuration.GetSection(Configuration.CHAT_CONFIGURATION_SECTION)
                                    .Get<ChatConfiguration>()!;

            this.httpHelper = httpHelper;
        }

        public async Task<AdvisorResponse?> SendQueryAsync(AdvisorQueryRequest req, CancellationToken cancellationToken)
        {
            return await AskChatAsync(req, cancellationToken);
        }

        private async Task<AdvisorResponse?> AskChatAsync(AdvisorQueryRequest query, CancellationToken cancellationToken)
        {
            return await resiliencePipeline.ExecuteAsync(async (ct) =>
            {
                return (await httpHelper.SendPostRequestAsync<AdvisorResponse>(chatConfig.BotUrl + CHAT_ADVISOR_ENDPOINT, JsonSerializer.Serialize(query), cancellationToken: cancellationToken));
            }, cancellationToken);
        }
    }
}

