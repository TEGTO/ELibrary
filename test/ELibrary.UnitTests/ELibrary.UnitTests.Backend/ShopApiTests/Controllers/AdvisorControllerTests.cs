using AutoMapper;
using Caching.Helpers;
using Caching.Services;
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
        private Mock<ICacheService> mockCacheService;
        private Mock<ICachingHelper> mockCachingHelper;
        private Mock<IMapper> mockMapper;
        private AdvisorController advisorController;

        [SetUp]
        public void SetUp()
        {
            mockAdvisorService = new Mock<IAdvisorService>();
            mockCacheService = new Mock<ICacheService>();
            mockCachingHelper = new Mock<ICachingHelper>();
            mockMapper = new Mock<IMapper>();
            advisorController = new AdvisorController(mockAdvisorService.Object, mockCacheService.Object, mockCachingHelper.Object, mockMapper.Object);
        }

        [Test]
        public async Task SendQuery_ReturnsOkResultWithAdvisorResponse()
        {
            // Arrange
            var request = new AdvisorQueryRequest
            {
                Query = "Tell me about the latest in AI."
            };
            var chatExpectedResponse = new ChatAdvisorResponse
            {
                Message = "The latest trends in AI include advancements in deep learning."
            };
            var expectedResponse = new AdvisorResponse
            {
                Message = "The latest trends in AI include advancements in deep learning."
            };
            mockAdvisorService.Setup(s => s.SendQueryAsync(It.IsAny<ChatAdvisorQueryRequest>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(chatExpectedResponse);
            mockMapper.Setup(s => s.Map<AdvisorResponse>(It.IsAny<ChatAdvisorResponse>()))
                              .Returns(expectedResponse);
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
            mockAdvisorService.Verify(s => s.SendQueryAsync(It.IsAny<ChatAdvisorQueryRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task SendQuery_ReturnsOkResultWithNullResponse_WhenNoAnswerFound()
        {
            // Arrange
            var request = new AdvisorQueryRequest
            {
                Query = "Can you tell me about quantum computing?"
            };
            mockAdvisorService.Setup(s => s.SendQueryAsync(It.IsAny<ChatAdvisorQueryRequest>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ChatAdvisorResponse?)null);
            // Act
            var result = await advisorController.SendQuery(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsNull(okResult.Value);
            mockAdvisorService.Verify(s => s.SendQueryAsync(It.IsAny<ChatAdvisorQueryRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}