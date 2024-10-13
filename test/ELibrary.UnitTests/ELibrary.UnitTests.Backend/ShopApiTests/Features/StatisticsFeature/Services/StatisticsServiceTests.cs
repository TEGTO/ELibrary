using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;
using ShopApi.Features.StatisticsFeature.Domain.Models;
using System.Reflection;

namespace ShopApi.Features.StatisticsFeature.Services.Tests
{
    [TestFixture]
    internal class StatisticsServiceTests
    {
        private Mock<IDatabaseRepository<LibraryShopDbContext>> repositoryMock;
        private CancellationToken cancellationToken;
        private StatisticsService service;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<LibraryShopDbContext>>();
            service = new StatisticsService(repositoryMock.Object);
            cancellationToken = new CancellationToken();
        }

        private static Mock<DbSet<T>> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMockDbSet();
        }

        [Test]
        public async Task GetStatisticsAsync_ReturnsCorrectStatistics()
        {
            // Arrange
            var getBookStatistics = new GetBookStatistics { FromUTC = DateTime.UtcNow.AddDays(-10), ToUTC = DateTime.UtcNow.AddDays(10), IncludeBooks = new Book[] { new Book { Id = 1 } } };
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1, CreatedAt = DateTime.UtcNow, OrderAmount = 2, TotalPrice = 100, OrderStatus = OrderStatus.Completed,
                    OrderBooks = new List<OrderBook>
                    {
                        new OrderBook { BookId = 1, BookAmount = 2, Book = new Book { Price = 50 } }
                    }
                }
            };
            var orderDbSetMock = GetDbSetMock(orders);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orderDbSetMock.Object);
            var books = new List<Book> { new Book { Id = 1, Price = 50, StockAmount = 10 } };
            var bookDbSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken)).ReturnsAsync(bookDbSetMock.Object);
            var cartBooks = new List<CartBook> { new CartBook { BookId = 1 } };
            var carts = new List<Cart>
            {
                new Cart() { Books = cartBooks }
            };
            var cartDbSetMock = GetDbSetMock(carts);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Cart>(cancellationToken)).ReturnsAsync(cartDbSetMock.Object);
            // Act
            var result = await service.GetStatisticsAsync(getBookStatistics, cancellationToken);
            // Assert
            Assert.That(result.InOrderCopies, Is.EqualTo(2));
            Assert.That(result.SoldCopies, Is.EqualTo(2));
            Assert.That(result.AveragePrice, Is.EqualTo(50));
            Assert.That(result.StockAmount, Is.EqualTo(10));
            Assert.That(result.EarnedMoney, Is.EqualTo(100));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Order>(cancellationToken), Times.Exactly(4));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Exactly(2));
        }
        [Test]
        public async Task GetInOrderCopiesAsync_ReturnsCorrectOrderCopies()
        {
            // Arrange
            var getBookStatistics = new GetBookStatistics { FromUTC = DateTime.UtcNow.AddDays(-10), ToUTC = DateTime.UtcNow.AddDays(10) };
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1, CreatedAt = DateTime.UtcNow, OrderAmount = 2, OrderStatus = OrderStatus.Completed,
                    OrderBooks = new List<OrderBook>
                    {
                        new OrderBook { BookId = 1, BookAmount = 2 }
                    }
                }
            };
            var orderDbSetMock = GetDbSetMock(orders);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orderDbSetMock.Object);
            // Act
            var metohdInfo = service.GetType().GetMethod("GetInOrderCopiesAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = await (Task<int>)metohdInfo.Invoke(service, new object[] { getBookStatistics, cancellationToken });
            // Assert
            Assert.That(result, Is.EqualTo(2));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Order>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetCanceledCopiesAsync_ReturnsCorrectCanceledOrderCopies()
        {
            // Arrange
            var getBookStatistics = new GetBookStatistics { FromUTC = DateTime.UtcNow.AddDays(-10), ToUTC = DateTime.UtcNow.AddDays(10) };
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1, CreatedAt = DateTime.UtcNow, OrderAmount = 2, OrderStatus = OrderStatus.Canceled,
                    OrderBooks = new List<OrderBook>
                    {
                        new OrderBook { BookId = 1, BookAmount = 2 }
                    }
                }
            };
            var orderDbSetMock = GetDbSetMock(orders);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orderDbSetMock.Object);
            // Act
            var metohdInfo = service.GetType().GetMethod("GetCanceledCopiesAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = await (Task<int>)metohdInfo.Invoke(service, new object[] { getBookStatistics, cancellationToken });
            // Assert
            Assert.That(result, Is.EqualTo(2));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Order>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetEarnedMoneyAsync_ReturnsCorrectAmount()
        {
            // Arrange
            var getBookStatistics = new GetBookStatistics { FromUTC = DateTime.UtcNow.AddDays(-10), ToUTC = DateTime.UtcNow.AddDays(10) };
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1, CreatedAt = DateTime.UtcNow, OrderAmount = 2, OrderStatus = OrderStatus.Completed,
                    OrderBooks = new List<OrderBook>
                    {
                        new OrderBook { BookId = 1, BookAmount = 2, Book = new Book { Price = 50 } }
                    }
                }
            };
            var orderDbSetMock = GetDbSetMock(orders);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Order>(It.IsAny<CancellationToken>())).ReturnsAsync(orderDbSetMock.Object);
            // Act
            var metohdInfo = service.GetType().GetMethod("GetEarnedMoneyAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = await (Task<decimal>)metohdInfo.Invoke(service, new object[] { getBookStatistics, cancellationToken });
            // Assert
            Assert.That(result, Is.EqualTo(100));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Order>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetAveragePriceAsync_ReturnsCorrectAveragePrice()
        {
            // Arrange
            var getBookStatistics = new GetBookStatistics { IncludeBooks = new[] { new Book { Id = 1 } } };
            var books = new List<Book> { new Book { Id = 1, Price = 50 }, new Book { Id = 2, Price = 60 } };
            var bookDbSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken)).ReturnsAsync(bookDbSetMock.Object);
            // Act
            var metohdInfo = service.GetType().GetMethod("GetAveragePriceAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = await (Task<decimal>)metohdInfo.Invoke(service, new object[] { getBookStatistics, cancellationToken });
            // Assert
            Assert.That(result, Is.EqualTo(50));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }
    }
}