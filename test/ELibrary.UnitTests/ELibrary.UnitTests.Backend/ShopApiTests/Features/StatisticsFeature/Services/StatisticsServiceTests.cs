using Moq;
using ShopApi.Features.StatisticsFeature.Domain.Models;
using ShopApi.Features.StatisticsFeature.Repository;

namespace ShopApi.Features.StatisticsFeature.Services.Tests
{
    [TestFixture]
    internal class StatisticsServiceTests
    {
        private Mock<IStatisticsRepository> mockRepository;
        private StatisticsService statisticsService;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IStatisticsRepository>();
            statisticsService = new StatisticsService(mockRepository.Object);
        }

        [Test]
        public async Task GetStatisticsAsync_ReturnsCorrectStatistics()
        {
            // Arrange
            var getShopStatistics = new GetShopStatisticsFilter();
            var cancellationToken = CancellationToken.None;
            mockRepository.Setup(r => r.GetInCartCopiesAsync(getShopStatistics, cancellationToken)).ReturnsAsync(10);
            mockRepository.Setup(r => r.GetInOrderCopiesAsync(getShopStatistics, cancellationToken)).ReturnsAsync(20);
            mockRepository.Setup(r => r.GetSoldCopiesAsync(getShopStatistics, cancellationToken)).ReturnsAsync(30);
            mockRepository.Setup(r => r.GetCanceledCopiesAsync(getShopStatistics, cancellationToken)).ReturnsAsync(5);
            mockRepository.Setup(r => r.GetOrderAmountAsync(getShopStatistics, cancellationToken)).ReturnsAsync(50);
            mockRepository.Setup(r => r.GetCanceledOrdersAsync(getShopStatistics, cancellationToken)).ReturnsAsync(3);
            mockRepository.Setup(r => r.GetAveragePriceAsync(getShopStatistics, cancellationToken)).ReturnsAsync(25.5m);
            mockRepository.Setup(r => r.GetEarnedMoneyAsync(getShopStatistics, cancellationToken)).ReturnsAsync(2550m);
            mockRepository.Setup(r => r.GetOrderAmountInDaysAsync(getShopStatistics, cancellationToken)).ReturnsAsync(new Dictionary<DateTime, long> { { DateTime.Today, 5 } });
            // Act
            var result = await statisticsService.GetStatisticsAsync(getShopStatistics, cancellationToken);
            // Assert
            Assert.That(result.InCartCopies, Is.EqualTo(10));
            Assert.That(result.InOrderCopies, Is.EqualTo(20));
            Assert.That(result.SoldCopies, Is.EqualTo(30));
            Assert.That(result.CanceledCopies, Is.EqualTo(5));
            Assert.That(result.OrderAmount, Is.EqualTo(50));
            Assert.That(result.CanceledOrderAmount, Is.EqualTo(3));
            Assert.That(result.AveragePrice, Is.EqualTo(25.5m));
            Assert.That(result.EarnedMoney, Is.EqualTo(2550m));
            Assert.That(result.OrderAmountInDays[DateTime.Today], Is.EqualTo(5));
        }
    }
}