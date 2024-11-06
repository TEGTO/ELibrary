using Microsoft.AspNetCore.Http;
using Moq;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ShopApi.IntegrationTests.Controllers.AdvisorController
{
    internal class SendQueryAdvisorControllerTests : BaseControllerTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mockAdvisorService.Setup(x => x.SendQueryAsync(
            It.IsAny<ChatAdvisorQueryRequest>(),
            It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(() => new ChatAdvisorResponse
            {
                Message = $"Here some interesting books: 1984, ...{new Random().Next(int.MaxValue)}"
            });
        }
        [SetUp]
        public void SetUp()
        {
            mockCachingHelper.Setup(x => x.GetCacheKey(It.IsAny<string>(), It.IsAny<HttpContext>()))
            .Returns(Guid.NewGuid().ToString());
        }

        [Test]
        public async Task SendQuery_ValidRequest_ReturnsOkWitResponse()
        {
            // Arrange
            var advisorRequest = new AdvisorQueryRequest
            {
                Query = "Give me something interesting...",
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/advisor");
            request.Content = new StringContent(JsonSerializer.Serialize(advisorRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var advisorResponse = JsonSerializer.Deserialize<AdvisorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(advisorResponse);
            Assert.NotNull(advisorResponse.Message);
            Assert.IsTrue(advisorResponse.Message.Contains("Here some interesting books: 1984, ..."));
        }
        [Test]
        public async Task SendQuery_ValidRequest_ReturnsOkWitResponseWInMemoryCache()
        {
            // Arrange
            var advisorRequest = new AdvisorQueryRequest
            {
                Query = "Give me something interesting...",
            };
            // Act
            var response1 = await MakeRequest(advisorRequest);
            var response2 = await MakeRequest(advisorRequest);
            await Task.Delay(3500);
            var response3 = await MakeRequest(advisorRequest);
            // Assert
            Assert.NotNull(response1);
            Assert.NotNull(response2);
            Assert.NotNull(response3);

            Assert.NotNull(response1.Message);
            Assert.NotNull(response2.Message);
            Assert.NotNull(response3.Message);

            Assert.IsTrue(response1.Message.Contains("Here some interesting books: 1984, ..."));
            Assert.IsTrue(response2.Message.Contains("Here some interesting books: 1984, ..."));
            Assert.IsTrue(response3.Message.Contains("Here some interesting books: 1984, ..."));

            Assert.IsTrue(response2.Message.Contains(response1.Message));
            Assert.IsFalse(response3.Message.Contains(response2.Message));
        }
        [Test]
        public async Task SendQuery_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var advisorRequest = new AdvisorQueryRequest
            {
                Query = new string('*', 5000),
            };
            using var request = new HttpRequestMessage(HttpMethod.Post, "/advisor");
            request.Content = new StringContent(JsonSerializer.Serialize(advisorRequest), Encoding.UTF8, "application/json");
            // Act
            var response = await httpClient.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        private async Task<AdvisorResponse> MakeRequest(AdvisorQueryRequest advisorRequest)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/advisor");
            request.Content = new StringContent(JsonSerializer.Serialize(advisorRequest), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var advisorResponse = JsonSerializer.Deserialize<AdvisorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return advisorResponse;
        }
    }
}
