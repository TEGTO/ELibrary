using LibraryShopEntities.Domain.Entities.Library;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController
{
    internal abstract class UpdateBaseLibraryEntityControllerTests<
        TEntity,
        TCreateRequest,
        TEntityResponse,
        TUpdateRequest
        > : BaseLibraryEntityControllerTest<TEntity, TCreateRequest, TEntityResponse> where TEntity : BaseLibraryEntity
    {
        protected abstract TUpdateRequest GetUpdateRequest();
        protected abstract TUpdateRequest GetInvalidUpdateRequest();

        [Test]
        public async Task UpdateEntity_ValidEntity_ReturnsOKWithItem()
        {
            // Arrange
            await CreateSamplesAsync();
            using var request = new HttpRequestMessage(HttpMethod.Put, ControllerEndpoint);
            var updateRequest = GetUpdateRequest();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(updateRequest),
                Encoding.UTF8,
                "application/json"
            );
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonSerializer.Deserialize<TEntityResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(responseEntity);
            Assert.That(mapper.Map<TEntity>(responseEntity).Name, Is.EqualTo(mapper.Map<TEntity>(updateRequest).Name));
        }
        [Test]
        public async Task UpdateEntity_NotEnoughtRights_ReturnsEAccessForbidden()
        {
            // Arrange
            await CreateSamplesAsync();
            using var request = new HttpRequestMessage(HttpMethod.Put, ControllerEndpoint);
            var updateRequest = GetUpdateRequest();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(updateRequest),
                Encoding.UTF8,
                "application/json"
            );
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
        [Test]
        public async Task UpdateEntity_UnauthorizedRequest_ReturnsUnauthorized()
        {
            // Arrange
            await CreateSamplesAsync();
            using var request = new HttpRequestMessage(HttpMethod.Put, ControllerEndpoint);
            var updateRequest = GetUpdateRequest();
            request.Content = new StringContent(
                JsonSerializer.Serialize(updateRequest),
                Encoding.UTF8,
                "application/json"
            );
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task CreateEntity_InvalidEntity_BadRequest()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Post, ControllerEndpoint);
            var updateRequest = GetInvalidUpdateRequest();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(updateRequest),
                Encoding.UTF8,
                "application/json"
            );
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}