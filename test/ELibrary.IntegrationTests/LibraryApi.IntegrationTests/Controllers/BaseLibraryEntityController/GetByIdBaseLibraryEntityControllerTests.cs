using LibraryShopEntities.Domain.Entities.Library;
using System.Net;
using System.Text.Json;

namespace LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController
{
    internal abstract class GetByIdBaseLibraryEntityControllerTests<
        TEntity,
        TCreateRequest,
        TEntityResponse
        > : BaseLibraryEntityControllerTest<TEntity, TCreateRequest, TEntityResponse> where TEntity : BaseLibraryEntity
    {
        [Test]
        public async Task GetEntityById_ValidId_ReturnsOKWithItem()
        {
            // Arrange
            var list = await CreateSamplesAsync();

            using var request = new HttpRequestMessage(HttpMethod.Get, $"{ControllerEndpoint}/{list[0]?.Id}");

            // Act 
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var actualServerSlot = JsonSerializer.Deserialize<TEntityResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(actualServerSlot);
            Assert.That(mapper.Map<TEntity>(actualServerSlot).Name, Is.EqualTo(list[0]?.Name));
        }

        [Test]
        public async Task GetEntityById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{ControllerEndpoint}/1");

            // Act 
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
