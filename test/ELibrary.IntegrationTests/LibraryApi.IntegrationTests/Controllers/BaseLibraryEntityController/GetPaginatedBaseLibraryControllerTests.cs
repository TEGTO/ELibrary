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
        public async Task GetPaginatedEntities_ReturnsOkWithTwoItems()
        {
            // Arrange
            var list = await CreateSamplesAsync();
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/pagination");
            var filter = GetFilter();
            filter.PageNumber = 1;
            filter.PageSize = 10;
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var responseEntities = JsonSerializer.Deserialize<List<TEntityResponse>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(responseEntities);
            Assert.That(responseEntities.Count, Is.EqualTo(list.Count));
            Assert.That(mapper.Map<TEntity>(responseEntities[0]).Name, Is.EqualTo(list[1].Name)); //Sorted by desc
        }
        [Test]
        public async Task GetPaginatedEntities_ReturnsOkWithEmptyList()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/pagination");
            var filter = GetFilter();
            filter.PageNumber = 1;
            filter.PageSize = 10;
            filter.ContainsName = "ZEROMATCHES";
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
