using Authentication.Identity;
using Authentication.Token;
using Microsoft.AspNetCore.Identity;
using ShopApi.Features.ClientFeature.Dtos;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.ClientController
{
    internal class BaseClientControllerTest : BaseControllerTest
    {
        protected string GetAccessTokenWithoutClientData()
        {
            var jwtHandler = new JwtHandler(settings);
            var random = new Random();
            IdentityUser identity = new IdentityUser()
            {
                Id = $"{random.Next(int.MaxValue)}",
                UserName = $"testuser{random.Next(int.MaxValue)}",
                Email = $"test{random.Next(int.MaxValue)}@example.com"
            };
            return jwtHandler.CreateToken(identity, [Roles.CLIENT]).AccessToken;
        }
        protected async Task ChangeClientAsync(string accessToken)
        {
            var request = new UpdateClientRequest() { Address = "New address", DateOfBirth = new DateTime(1962, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), Email = "newsome@gmail.com", LastName = "newlastname", Name = "newname", MiddleName = "newmiddlename", Phone = "05353" };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"/client");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.SendAsync(httpRequest);
        }
    }
}
