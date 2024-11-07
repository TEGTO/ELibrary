using LibraryShopEntities.Domain.Dtos.Shop;
using ShopApi.Features.ClientFeature.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.ClientController
{
    internal class AdminAdminUpdateClientControllerTests : BaseClientControllerTest
    {
        protected ClientResponse Client;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            Client = await TestHelper.CreateClientAsync(AccessToken, httpClient);
        }

        [Test]
        public async Task AdminUpdateClient_ValidRequest_ReturnsCreateWithClient()
        {
            // Arrange
            var request = new UpdateClientRequest() { Address = "New address", DateOfBirth = new DateTime(1962, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), Email = "newsome@gmail.com", LastName = "newlastname", Name = "newname", MiddleName = "newmiddlename", Phone = "05353" };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"/client/admin/{Client.UserId}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ClientResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(response);
            Assert.That(response.Name, Is.EqualTo("newname"));
        }
        [Test]
        public async Task AdminUpdateClient_ClientDoesntExist_ReturnsConflict()
        {
            // Arrange
            var request = new UpdateClientRequest() { Address = "New address", DateOfBirth = new DateTime(1962, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), Email = "newsome@gmail.com", LastName = "newlastname", Name = "newname", MiddleName = "newmiddlename", Phone = "05353" };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"/client/admin/100");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }
        [Test]
        public async Task AdminUpdateClient_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new UpdateClientRequest();
            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"/client/admin/{Client.UserId}");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AdminAccessToken);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task AdminUpdateClient_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var request = new UpdateClientRequest();
            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"/client/admin/{Client.UserId}");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
