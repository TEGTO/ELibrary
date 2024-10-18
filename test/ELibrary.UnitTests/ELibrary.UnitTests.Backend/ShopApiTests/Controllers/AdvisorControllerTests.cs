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
        public void Setup()
        {
            mockAdvisorService = new Mock<IAdvisorService>();
            advisorController = new AdvisorController(mockAdvisorService.Object);
        }

        [Test]
        public async Task SendQuery_ReturnsOk()
        {
            // Arrange
            var request = new AdvisorQueryRequest
            {
                Query = "What are the best hotels?"
            };
            var cancellationToken = CancellationToken.None;
            var mockResponse = "The best hotels are...";
            mockAdvisorService.Setup(service => service.SendQueryAsync(request.Query, cancellationToken))
                               .ReturnsAsync(mockResponse);
            // Act
            var result = await advisorController.SendQuery(request, cancellationToken);
            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.Value, Is.EqualTo(mockResponse));
        }
    }
}