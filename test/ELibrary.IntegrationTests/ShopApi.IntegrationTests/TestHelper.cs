using LibraryShopEntities.Domain.Dtos.Shop;
using ShopApi.Features.ClientFeature.Dtos;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests
{
    internal static class TestHelper
    {
        public static async Task<ClientResponse> CreateClientAsync(string managerAccessToken, HttpClient client)
        {
            var createClientRequest = new CreateClientRequest() { Address = "Some address", DateOfBirth = new DateTime(1962, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), Email = "some@gmail.com", LastName = "lastname", Name = "name", MiddleName = "middlename", Phone = "0123456789" };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/client");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", managerAccessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(createClientRequest), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            var clientContent = await response.Content.ReadAsStringAsync();
            var clientResponse = JsonSerializer.Deserialize<ClientResponse>(clientContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return clientResponse;
        }
    }
}
