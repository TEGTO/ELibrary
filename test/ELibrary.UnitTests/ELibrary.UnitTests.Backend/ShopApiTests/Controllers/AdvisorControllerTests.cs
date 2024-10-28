using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;
using ShopApi.Features.AdvisorFeature.Services;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    internal class AdvisorControllerTests
    {
        private Mock<IAdvisorService> mockAdvisorService;
        private AdvisorController advisorController;

        [SetUp]
        public void SetUp()
        {
            mockAdvisorService = new Mock<IAdvisorService>();
            advisorController = new AdvisorController(mockAdvisorService.Object);
        }

        [Test]
        public async Task SendQuery_ReturnsOkResultWithAdvisorResponse()
        {
            // Arrange
            var request = new AdvisorQueryRequest
            {
                Query = "Tell me about the latest in AI."
            };
            var expectedResponse = new AdvisorResponse
            {
                Message = "The latest trends in AI include advancements in deep learning."
            };
            mockAdvisorService.Setup(s => s.SendQueryAsync(It.IsAny<AdvisorQueryRequest>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(expectedResponse);
            // Act
            var result = await advisorController.SendQuery(request, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf<AdvisorResponse>(okResult.Value);
            var response = okResult.Value as AdvisorResponse;
            Assert.That(response.Message, Is.EqualTo(expectedResponse.Message));
            mockAdvisorService.Verify(s => s.SendQueryAsync(It.IsAny<AdvisorQueryRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task SendQuery_ReturnsOkResultWithNullResponse_WhenNoAnswerFound()
        {
            // Arrange
            var request = new AdvisorQueryRequest
            {
                Query = "Can you tell me about quantum computing?"
            };
            mockAdvisorService.Setup(s => s.SendQueryAsync(It.IsAny<AdvisorQueryRequest>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((AdvisorResponse?)null);
            // Act
            var result = await advisorController.SendQuery(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsNull(okResult.Value);
            mockAdvisorService.Verify(s => s.SendQueryAsync(It.IsAny<AdvisorQueryRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}