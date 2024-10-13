using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopApi.Features.StatisticsFeature.Domain.Dtos;
using ShopApi.Features.StatisticsFeature.Domain.Models;
using ShopApi.Features.StatisticsFeature.Services;

namespace ShopApi.Controllers.Tests
{
    namespace LibraryShop.Tests.Controllers
    {
        [TestFixture]
        internal class StatisticsControllerTests
        {
            private Mock<IMapper> mapperMock;
            private Mock<IStatisticsService> statisticsServiceMock;
            private StatisticsController controller;

            [SetUp]
            public void SetUp()
            {
                mapperMock = new Mock<IMapper>();
                statisticsServiceMock = new Mock<IStatisticsService>();
                controller = new StatisticsController(mapperMock.Object, statisticsServiceMock.Object);
            }

            [Test]
            public async Task GetBookStatistics_ValidRequest_ReturnsOkWithStatistics()
            {
                // Arrange
                var request = new GetBookStatisticsRequest();
                var getStatistics = new GetBookStatistics();
                var statistics = new BookStatistics();
                var response = new BookStatisticsResponse();
                mapperMock.Setup(m => m.Map<GetBookStatistics>(request)).Returns(getStatistics);
                statisticsServiceMock.Setup(s => s.GetStatisticsAsync(getStatistics, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(statistics);
                mapperMock.Setup(m => m.Map<BookStatisticsResponse>(statistics)).Returns(response);
                // Act
                var result = await controller.GetBookStatistics(request, CancellationToken.None);
                // Assert
                var okResult = result.Result as OkObjectResult;
                Assert.IsNotNull(okResult);
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(response));
            }
        }
    }
}