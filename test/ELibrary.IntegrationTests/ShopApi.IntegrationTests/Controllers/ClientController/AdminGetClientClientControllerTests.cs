using LibraryShopEntities.Domain.Dtos.Shop;
using ShopApi.Features.ClientFeature.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.ClientController
{
    internal class AdminAdminGetClientClientControllerTests : BaseClientControllerTest
    {
        protected ClientResponse Client;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            Client = await TestHelper.CreateClientAsync(AccessToken, httpClient);
        }

        [Test]
        public async Task AdminGetClient_ClientExists_ReturnsOkWithClient()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/client/admin/{Client.UserId}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetClientResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(response);
            Assert.NotNull(response.Client);
            Assert.That(response.Client.Name, Is.EqualTo("name"));
        }
        [Test]
        public async Task AdminGetClient_ClientDoesntExist_ReturnsOkWithoutClient()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/client/admin/100");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetClientResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(response);
            Assert.Null(response.Client);
        }
        [Test]
        public async Task AdminGetClient_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/client/admin/{Client.UserId}");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task AdminGetClient_NotAdmin_ReturnsForbidden()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/client/admin/{Client.UserId}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
