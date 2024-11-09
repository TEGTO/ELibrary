using LibraryShopEntities.Domain.Dtos.SharedRequests;
using LibraryShopEntities.Domain.Entities.Library;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController
{
    internal abstract class GetByIdsBaseLibraryEnttityControllerTests<
        TEntity,
        TCreateRequest,
        TEntityResponse
        > : BaseLibraryEntityControllerTest<TEntity, TCreateRequest, TEntityResponse> where TEntity : BaseLibraryEntity
    {
        [Test]
        public async Task GetByIds_ReturnsOkWithTwoItems()
        {
            // Arrange
            var list = await CreateSamplesAsync();
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/ids");
            var idsRequest = new GetByIdsRequest() { Ids = new List<int> { 1, 2, 3 } };
            request.Content = new StringContent(JsonSerializer.Serialize(idsRequest), Encoding.UTF8, "application/json");
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
            Assert.That(list.Find(x => x.Name == mapper.Map<TEntity>(responseEntities[0]).Name) != null, Is.True);
        }
        [Test]
        public async Task GetByIds_ReturnsOkWithEmptyList()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/ids");
            var idsRequest = new GetByIdsRequest() { Ids = new List<int> { 10, 20, 30 } };
            request.Content = new StringContent(JsonSerializer.Serialize(idsRequest), Encoding.UTF8, "application/json");
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
        [Test]
        public async Task GetByIds_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/ids");
            var idsRequest = new GetByIdsRequest() { Ids = null };
            request.Content = new StringContent(JsonSerializer.Serialize(idsRequest), Encoding.UTF8, "application/json");
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
