using LibraryShopEntities.Domain.Entities.Library;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController
{
    internal abstract class CreateBaseLibraryEntityControllerTests<
        TEntity,
        TCreateRequest,
        TEntityResponse
        > : BaseLibraryEntityControllerTest<TEntity, TCreateRequest, TEntityResponse> where TEntity : BaseLibraryEntity
    {
        protected abstract ValueTask<TCreateRequest> GetCreateRequestAsync();
        protected abstract ValueTask<TCreateRequest> GetInvalidCreateRequestAsync();

        [Test]
        public async Task CreateEntity_ValidEntity_ReturnsOKWithItem()
        {
            // Arrange
            var createRequest = await GetCreateRequestAsync();

            using var request = new HttpRequestMessage(HttpMethod.Post, ControllerEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(createRequest),
                Encoding.UTF8,
                "application/json"
            );

            // Act 
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var content = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonSerializer.Deserialize<TEntityResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(responseEntity);
            Assert.That(mapper.Map<TEntity>(responseEntity).Name, Is.EqualTo(mapper.Map<TEntity>(createRequest).Name));
        }

        [Test]
        public async Task CreateEntity_NotEnoughtRights_ReturnsEAccessForbidden()
        {
            // Arrange
            var createRequest = GetCreateRequestAsync();

            using var request = new HttpRequestMessage(HttpMethod.Post, ControllerEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(createRequest),
                Encoding.UTF8,
                "application/json"
            );

            // Act 
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task CreateEntity_UnauthorizedRequest_ReturnsUnauthorized()
        {
            // Arrange
            var createRequest = GetCreateRequestAsync();

            using var request = new HttpRequestMessage(HttpMethod.Post, ControllerEndpoint);
            request.Content = new StringContent(
                JsonSerializer.Serialize(createRequest),
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
            var createRequest = await GetInvalidCreateRequestAsync();

            using var request = new HttpRequestMessage(HttpMethod.Post, ControllerEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(createRequest),
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
