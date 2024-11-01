using Polly;
using Polly.Registry;
using Shared.Helpers;
using ShopApi.Domain.Dtos;
using System.Text.Json;

namespace ShopApi.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ResiliencePipeline resiliencePipeline;
        private readonly IHttpHelper httpHelper;
        private readonly string libraryApi;

        public LibraryService(ResiliencePipelineProvider<string> resiliencePipelineProvider, IHttpHelper httpHelper, IConfiguration configuration)
        {
            resiliencePipeline = resiliencePipelineProvider.GetPipeline(Configuration.DEFAULT_RESILIENCE_PIPELINE);
            this.httpHelper = httpHelper;
            libraryApi = configuration[Configuration.LIBRARY_API_URL]!;
        }

        public async Task<IEnumerable<T>> GetByIdsAsync<T>(List<int> ids, string endpoint, CancellationToken cancellationToken)
        {
            var request = new GetByIdsRequest() { Ids = ids };
            return await resiliencePipeline.ExecuteAsync(async (ct) =>
            {
                return (await httpHelper.SendPostRequestAsync<IEnumerable<T>>(libraryApi + endpoint, JsonSerializer.Serialize(request), cancellationToken: cancellationToken))!;
            }, cancellationToken);
        }
        public async Task RaiseBookPopularityByIdsAsyn(List<int> ids, CancellationToken cancellationToken)
        {
            var request = new GetByIdsRequest() { Ids = ids };
            await resiliencePipeline.ExecuteAsync(async (ct) =>
            {
                return (await httpHelper.SendPostRequestAsync<string>(
                    $"{libraryApi}/{Configuration.LIBRARY_API_RAISE_BOOK_POPULARITY_ENDPOINT}",
                    JsonSerializer.Serialize(request), cancellationToken: cancellationToken))!;
            }, cancellationToken);
        }
    }
}
