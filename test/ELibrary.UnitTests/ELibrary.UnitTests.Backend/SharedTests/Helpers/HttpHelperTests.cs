using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Shared.Helpers.Tests
{
    [TestFixture]
    internal class HttpHelperTests
    {
        private Mock<IHttpClientFactory> mockHttpClientFactory;
        private HttpHelper httpHelper;
        private Mock<HttpMessageHandler> mockHttpMessageHandler;

        [SetUp]
        public void SetUp()
        {
            mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            httpHelper = new HttpHelper(mockHttpClientFactory.Object);
        }

        [Test]
        public async Task SendGetRequestAsync_ReturnsExpectedResult()
        {
            // Arrange
            var endpoint = "https://example.com/api/data";
            var expectedResponse = new { Name = "Test", Value = 123 };
            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == endpoint),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });
            // Act
            var result = await httpHelper.SendGetRequestAsync<dynamic>(endpoint);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Name, result.Name.ToString());
            Assert.That((int)result.Value, Is.EqualTo(expectedResponse.Value));
        }
        [Test]
        public async Task SendPostRequestAsync_WithBodyParams_ReturnsExpectedResult()
        {
            // Arrange
            var endpoint = "https://example.com/api/post";
            var bodyParams = new Dictionary<string, string> { { "param1", "value1" }, { "param2", "value2" } };
            var expectedResponse = new { Status = "Success" };
            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.ToString() == endpoint),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });
            // Act
            var result = await httpHelper.SendPostRequestAsync<dynamic>(endpoint, bodyParams);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Success", result.Status.ToString());
        }
        [Test]
        public async Task SendPostRequestAsync_WithJsonBody_ReturnsExpectedResult()
        {
            // Arrange
            var endpoint = "https://example.com/api/json";
            var requestBody = JsonConvert.SerializeObject(new { id = 1, name = "test" });
            var expectedResponse = new { Status = "Processed" };
            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.ToString() == endpoint),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });
            // Act
            var result = await httpHelper.SendPostRequestAsync<dynamic>(endpoint, requestBody);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Processed", result.Status.ToString());
        }
        [Test]
        public void SendHttpRequestAsync_ThrowsException_OnNonSuccessStatusCode()
        {
            // Arrange
            var endpoint = "https://example.com/api/fail";
            var errorResponse = "Error occurred";
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == endpoint),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(errorResponse, Encoding.UTF8, "application/json")
                });
            // Act & Assert
            var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
                await httpHelper.SendGetRequestAsync<dynamic>(endpoint));
            Assert.IsTrue(ex.Message.Contains("Error occurred"));
        }
        [Test]
        public async Task SendPutRequestAsync_SendsCorrectRequest()
        {
            // Arrange
            var endpoint = "https://example.com/api/update";
            var requestBody = JsonConvert.SerializeObject(new { id = 1, name = "updated" });
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put && req.RequestUri.ToString() == endpoint),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });
            // Act
            await httpHelper.SendPutRequestAsync(endpoint, requestBody);
            // Assert
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri.ToString() == endpoint &&
                    req.Content != null),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}