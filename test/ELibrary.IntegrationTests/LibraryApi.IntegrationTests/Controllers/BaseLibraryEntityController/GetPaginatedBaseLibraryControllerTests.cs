using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController
{
    internal abstract class GetPaginatedBaseLibraryControllerTests<
        TEntity,
        TCreateRequest,
        TEntityResponse,
        TFilter
        > : BaseLibraryEntityControllerTest<TEntity, TCreateRequest, TEntityResponse> where TEntity : BaseLibraryEntity where TFilter : LibraryFilterRequest
    {
        protected abstract TFilter GetFilter();

        [Test]
        public async Task GetPaginatedEntities_ReturnsOkWithCachedItems()
        {
            // Arrange
            var filter = GetFilter();
            filter.PageNumber = 1;
            filter.PageSize = 5;

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/pagination");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");

            using var request2 = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/pagination");
            request2.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");

            filter.PageNumber = 1;
            filter.PageSize = 10;

            using var request3 = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/pagination");
            request3.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");

            // Act 
            var response = await client.SendAsync(request);
            var list = await CreateSamplesAsync();
            var response2 = await client.SendAsync(request2);
            var response3 = await client.SendAsync(request3);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response3.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var responseEntities = JsonSerializer.Deserialize<List<TEntityResponse>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var content2 = await response2.Content.ReadAsStringAsync();
            var responseEntities2 = JsonSerializer.Deserialize<List<TEntityResponse>>(content2, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var content3 = await response3.Content.ReadAsStringAsync();
            var responseEntities3 = JsonSerializer.Deserialize<List<TEntityResponse>>(content3, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(responseEntities);
            Assert.NotNull(responseEntities2);
            Assert.NotNull(responseEntities3);
            Assert.That(responseEntities.Count, Is.Not.EqualTo(list.Count));
            Assert.That(responseEntities2.Count, Is.Not.EqualTo(list.Count));
            Assert.That(responseEntities3.Count, Is.EqualTo(list.Count));
            Assert.That(responseEntities.Count, Is.EqualTo(responseEntities2.Count));
            Assert.That(responseEntities3.Count, Is.Not.EqualTo(responseEntities2.Count));
        }

        [Test]
        public async Task GetPaginatedEntities_ReturnsOkWithEmptyList()
        {
            // Arrange
            var filter = GetFilter();
            filter.PageNumber = 1;
            filter.PageSize = 10;
            filter.ContainsName = "ZEROMATCHES";

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/pagination");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");

            // Act 
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var responseEntities = JsonSerializer.Deserialize<List<TEntity>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(responseEntities);
            Assert.That(responseEntities.Count, Is.EqualTo(0));
        }
    }
}
