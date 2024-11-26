using LibraryShopEntities.Domain.Entities.Library;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController
{
    internal abstract class BaseLibraryEntityControllerTest<
        TEntity,
        TCreateRequest,
        TEntityResponse
        > : BaseControllerTest
         where TEntity : BaseLibraryEntity
    {
        protected abstract string ControllerEndpoint { get; }

        protected abstract Task<List<TEntity?>> CreateSamplesAsync();

        protected async Task<TEntity?> CreateSampleEntityAsync(TCreateRequest createRequest)
        {
            return await CreateSampleEntityAsync<TEntity, TCreateRequest, TEntityResponse>(createRequest, ControllerEndpoint);
        }
        protected async Task<T?> CreateSampleEntityAsync<T, TC, TR>(TC createRequest, string endpoint)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(createRequest),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var responseEntity = JsonSerializer.Deserialize<TR>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return mapper.Map<T>(responseEntity);
        }
    }
}
