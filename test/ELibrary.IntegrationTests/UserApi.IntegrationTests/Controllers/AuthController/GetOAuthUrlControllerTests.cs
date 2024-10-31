using Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text.Json;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.IntegrationTests.Controllers.AuthController
{
    internal class GetOAuthUrlControllerTests : BaseAuthControllerTest
    {
        [Test]
        public async Task GetOAuthUrl_ValidRequest_ReturnsOkWithUrl()
        {
            // Arrange
            var getUrlParams = new GetOAuthUrlQueryParams
            {
                OAuthLoginProvider = OAuthLoginProvider.Google,
                RedirectUrl = "someurl",
                CodeVerifier = "someverifier"
            };
            using var request = new HttpRequestMessage(HttpMethod.Get, QueryHelpers.AddQueryString("/auth/oauth",
                new Dictionary<string, string?>
                {
                    {"OAuthLoginProvider", getUrlParams.OAuthLoginProvider.ToString() },
                    {"RedirectUrl", getUrlParams.RedirectUrl },
                    {"CodeVerifier", getUrlParams.CodeVerifier }
                }
            ));
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var getUrlResponse = JsonSerializer.Deserialize<GetOAuthUrlResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(getUrlResponse);
            Assert.NotNull(getUrlResponse.Url);
            Assert.IsTrue(getUrlResponse.Url.Contains(getUrlParams.RedirectUrl));
        }
        [Test]
        public async Task GetOAuthUrl_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var getUrlParams = new GetOAuthUrlQueryParams
            {
                OAuthLoginProvider = OAuthLoginProvider.Google,
                RedirectUrl = "",
                CodeVerifier = ""
            };
            using var request = new HttpRequestMessage(HttpMethod.Get, QueryHelpers.AddQueryString("/auth/oauth",
                new Dictionary<string, string?>
                {
                    {"OAuthLoginProvider", getUrlParams.OAuthLoginProvider.ToString() },
                    {"RedirectUrl", getUrlParams.RedirectUrl },
                    {"CodeVerifier", getUrlParams.CodeVerifier }
                }
            ));
            // Act
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}