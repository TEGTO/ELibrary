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
            var idsRequest = new GetByIdsRequest() { Ids = new List<int> { 1, 2 } };

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/ids");
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
            Assert.That(list.Find(x => x?.Name == mapper.Map<TEntity>(responseEntities[0]).Name) != null, Is.True);
        }

        [Test]
        public async Task GetByIds_ReturnsOkWithCachedItem()
        {
            // Arrange
            var idsRequest = new GetByIdsRequest() { Ids = new List<int> { 1 } };

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/ids");
            request.Content = new StringContent(JsonSerializer.Serialize(idsRequest), Encoding.UTF8, "application/json");

            using var request2 = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/ids");
            request2.Content = new StringContent(JsonSerializer.Serialize(idsRequest), Encoding.UTF8, "application/json");

            // Act 
            var response = await client.SendAsync(request);
            var list = await CreateSamplesAsync();
            var response2 = await client.SendAsync(request2);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

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

            Assert.NotNull(responseEntities);
            Assert.NotNull(responseEntities2);
            Assert.That(responseEntities.Count, Is.Not.EqualTo(list.Count));
            Assert.That(responseEntities2.Count, Is.Not.EqualTo(list.Count));
            Assert.That(responseEntities.Count, Is.EqualTo(responseEntities2.Count));
        }

        [Test]
        public async Task GetByIds_ReturnsOkWithEmptyList()
        {
            // Arrange
            var idsRequest = new GetByIdsRequest() { Ids = new List<int> { 10, 20, 30 } };

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/ids");
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
            var idsRequest = new GetByIdsRequest() { Ids = null! };

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/ids");
            request.Content = new StringContent(JsonSerializer.Serialize(idsRequest), Encoding.UTF8, "application/json");

            // Act 
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
