﻿using LibraryShopEntities.Domain.Dtos.Shop;
using ShopApi.Features.ClientFeature.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.ClientController
{
    internal class CreateClientClientControllerTests : BaseClientControllerTest
    {
        [Test]
        public async Task CreateClient_ValidRequest_ReturnsCreateWithClient()
        {
            // Arrange
            var request = new CreateClientRequest() { Address = "Some address", DateOfBirth = new DateTime(1962, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), Email = "some@gmail.com", LastName = "lastname", Name = "name", MiddleName = "middlename", Phone = "05353" };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/client");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ClientResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(response);
            Assert.That(response.Name, Is.EqualTo("name"));
        }
        [Test]
        public async Task CreateClient_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateClientRequest();
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/client");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task CreateClient_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var request = new CreateClientRequest();
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/client");
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            // Act
            var httpResponse = await httpClient.SendAsync(httpRequest);
            // Assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}