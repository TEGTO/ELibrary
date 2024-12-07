using DatabaseControl.Repositories;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;
using ShopApi.Features.StatisticsFeature.Domain.Models;
using ShopApi.Features.StatisticsFeature.Repository;

namespace ShopApiTests.Features.StatisticsFeature.Repository.Tests
{
    [TestFixture]
    internal class StatisticsRepositoryTests
    {
        private Mock<IDatabaseRepository<ShopDbContext>> repositoryMock;
        private StatisticsRepository statisticsRepository;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<ShopDbContext>>();
            statisticsRepository = new StatisticsRepository(repositoryMock.Object);
            cancellationToken = CancellationToken.None;
        }

        [Test]
        public async Task GetInCartCopiesAsync_WhenCalled_ReturnsTotalCartBookAmount()
        {
            // Arrange
            var carts = new List<Cart>
            {
                new Cart
                {
                    Books = new List<CartBook>
                    {
                        new CartBook { BookAmount = 3 },
                        new CartBook { BookAmount = 5 }
                    }
                }
            };
            repositoryMock.Setup(r => r.GetQueryableAsync<Cart>(cancellationToken)).ReturnsAsync(carts.AsQueryable().BuildMockDbSet().Object);
            // Act
            var result = await statisticsRepository.GetInCartCopiesAsync(new GetShopStatisticsFilter(), cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(8));
        }
        [Test]
        public async Task GetInOrderCopiesAsync_WhenCalled_ReturnsTotalOrderAmount()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderAmount = 3 },
                new Order { OrderAmount = 5 }
            };
            repositoryMock.Setup(r => r.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orders.AsQueryable().BuildMockDbSet().Object);
            // Act
            var result = await statisticsRepository.GetInOrderCopiesAsync(new GetShopStatisticsFilter(), cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(8));
        }
        [Test]
        public async Task GetSoldCopiesAsync_WhenCalled_ReturnsTotalCompletedOrderAmount()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderAmount = 3, OrderStatus = OrderStatus.Completed },
                new Order { OrderAmount = 5, OrderStatus = OrderStatus.Canceled }
            };
            repositoryMock.Setup(r => r.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orders.AsQueryable().BuildMockDbSet().Object);
            // Act
            var result = await statisticsRepository.GetSoldCopiesAsync(new GetShopStatisticsFilter(), cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(3));
        }
        [Test]
        public async Task GetCanceledCopiesAsync_WhenCalled_ReturnsTotalCanceledOrderAmount()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderAmount = 3, OrderStatus = OrderStatus.Completed },
                new Order { OrderAmount = 5, OrderStatus = OrderStatus.Canceled }
            };
            repositoryMock.Setup(r => r.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orders.AsQueryable().BuildMockDbSet().Object);
            // Act
            var result = await statisticsRepository.GetCanceledCopiesAsync(new GetShopStatisticsFilter(), cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(5));
        }
        [Test]
        public async Task GetOrderAmountAsync_WhenCalled_ReturnsOrderCount()
        {
            // Arrange
            var orders = new List<Order> { new Order(), new Order() };
            repositoryMock.Setup(r => r.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orders.AsQueryable().BuildMockDbSet().Object);
            // Act
            var result = await statisticsRepository.GetOrderAmountAsync(new GetShopStatisticsFilter(), cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(2));
        }
        [Test]
        public async Task GetCanceledOrdersAsync_WhenCalled_ReturnsCanceledOrderCount()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderStatus = OrderStatus.Canceled },
                new Order { OrderStatus = OrderStatus.Completed }
            };
            repositoryMock.Setup(r => r.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orders.AsQueryable().BuildMockDbSet().Object);
            // Act
            var result = await statisticsRepository.GetCanceledOrdersAsync(new GetShopStatisticsFilter(), cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(1));
        }
        [Test]
        public async Task GetAveragePriceAsync_WhenCalled_ReturnsAverageOrderPrice()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { TotalPrice = 100 },
                new Order { TotalPrice = 200 }
            };
            repositoryMock.Setup(r => r.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orders.AsQueryable().BuildMockDbSet().Object);
            // Act
            var result = await statisticsRepository.GetAveragePriceAsync(new GetShopStatisticsFilter(), cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(150));
        }
        [Test]
        public async Task GetEarnedMoneyAsync_WhenCalled_ReturnsTotalCompletedOrderPrice()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { TotalPrice = 100, OrderStatus = OrderStatus.Completed },
                new Order { TotalPrice = 200, OrderStatus = OrderStatus.Canceled }
            };
            repositoryMock.Setup(r => r.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orders.AsQueryable().BuildMockDbSet().Object);
            // Act
            var result = await statisticsRepository.GetEarnedMoneyAsync(new GetShopStatisticsFilter(), cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(100));
        }
        [Test]
        public async Task GetOrderAmountInDaysAsync_WhenCalled_ReturnsOrderCountPerDay()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { CreatedAt = new DateTime(2023, 1, 1) },
                new Order { CreatedAt = new DateTime(2023, 1, 1) },
                new Order { CreatedAt = new DateTime(2023, 1, 2) }
            };
            repositoryMock.Setup(r => r.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orders.AsQueryable().BuildMockDbSet().Object);
            // Act
            var result = await statisticsRepository.GetOrderAmountInDaysAsync(new GetShopStatisticsFilter(), cancellationToken);
            // Assert
            Assert.That(result[new DateTime(2023, 1, 1)], Is.EqualTo(2));
            Assert.That(result[new DateTime(2023, 1, 2)], Is.EqualTo(1));
        }
    }
}