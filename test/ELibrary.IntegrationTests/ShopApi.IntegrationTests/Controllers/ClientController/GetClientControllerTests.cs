using LibraryShopEntities.Domain.Dtos.Shop;
using ShopApi.Features.ClientFeature.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.ClientController
{
    internal class GetClientControllerTests : BaseClientControllerTest
    {
        protected ClientResponse Client;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            Client = await TestHelper.CreateClientAsync(AccessToken, httpClient);
        }

        [Test]
        public async Task GetClient_ClientExists_ReturnsOkWithClient()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/client");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
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
        public async Task GetClient_ClientDoesntExist_ReturnsOkWithoutClient()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/client");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessTokenWithoutClientData());
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
        public async Task GetClient_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/client");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task GetClient_ClientExists_ReturnsOkWithCachedClient()
        {
            // Arrange
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/client");
            using var httpRequest2 = new HttpRequestMessage(HttpMethod.Get, $"/client");
            var accessToken = GetAccessTokenWithoutClientData();
            await TestHelper.CreateClientAsync(accessToken, httpClient);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpRequest2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            await ChangeClientAsync(accessToken);
            var httpResponse2 = await httpClient.SendAsync(httpRequest2);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetClientResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var content2 = await httpResponse2.Content.ReadAsStringAsync();
            var response2 = JsonSerializer.Deserialize<GetClientResponse>(content2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.That(response.Client.Name, Is.EqualTo("name"));
            Assert.That(response2.Client.Name, Is.EqualTo(response.Client.Name));
        }
    }
}
