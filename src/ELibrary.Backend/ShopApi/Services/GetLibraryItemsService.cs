using Polly;
using Polly.Registry;
using Shared.Helpers;
using ShopApi.Domain.Dtos;
using System.Text.Json;

namespace ShopApi.Services
{
    public class GetLibraryItemsService : IGetLibraryItemsService
    {
        private readonly ResiliencePipeline resiliencePipeline;
        private readonly IHttpHelper httpHelper;

        public GetLibraryItemsService(ResiliencePipelineProvider<string> resiliencePipelineProvider, IHttpHelper httpHelper, IConfiguration configuration)
        {
            resiliencePipeline = resiliencePipelineProvider.GetPipeline(Configuration.DEFAULT_RESILIENCE_PIPELINE);
            this.httpHelper = httpHelper;
        }

        public async Task<IEnumerable<T>> GetByIdsAsync<T>(List<int> ids, string endpoint, CancellationToken cancellationToken)
        {
            var request = new GetByIdsRequest() { Ids = ids };
            return await resiliencePipeline.ExecuteAsync(async (ct) =>
            {
                return (await httpHelper.SendPostRequestAsync<IEnumerable<T>>(Configuration.LIBRARY_API_URL + endpoint, JsonSerializer.Serialize(request), cancellationToken: cancellationToken))!;
            }, cancellationToken);
        }
    }
}
