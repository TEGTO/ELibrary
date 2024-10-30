using LibraryShopEntities.Domain.Entities.Library;
using System.Net;
using System.Net.Http.Headers;

namespace LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController
{
    internal abstract class DeleteLibraryEntityControllerTests<
        TEntity,
        TCreateRequest,
        TEntityResponse
        > : BaseLibraryEntityControllerTest<TEntity, TCreateRequest, TEntityResponse> where TEntity : BaseLibraryEntity
    {
        [Test]
        public async Task DeleteEntity_ValidId_ReturnsOk()
        {
            // Arrange
            var list = await CreateSamplesAsync();
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ControllerEndpoint}/{list[0].Id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            using var getRequest = new HttpRequestMessage(HttpMethod.Get, $"{ControllerEndpoint}/{list[0].Id}");
            var getResponse = await client.SendAsync(getRequest);
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task DeleteEntity_NotEnoughRights_ReturnsForbidden()
        {
            // Arrange
            var list = await CreateSamplesAsync();
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ControllerEndpoint}/{list[0].Id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
        [Test]
        public async Task DeleteEntity_UnauthorizedRequest_ReturnsUnauthorized()
        {
            // Arrange
            var list = await CreateSamplesAsync();
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ControllerEndpoint}/{list[0].Id}");
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task DeleteEntity_InvalidId_ReturnsOk()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"{ControllerEndpoint}/1000");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ManagerAccessToken);
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}