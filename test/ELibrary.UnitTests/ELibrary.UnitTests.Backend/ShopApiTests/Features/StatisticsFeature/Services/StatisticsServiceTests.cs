﻿using LibraryShopEntities.Data;
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
        private Mock<IDatabaseRepository<ShopDbContext>> repositoryMock;
        private CancellationToken cancellationToken;
        private StatisticsService service;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<ShopDbContext>>();
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
            var getBookStatistics = new GetShopStatistics { FromUTC = DateTime.UtcNow.AddDays(-10), ToUTC = DateTime.UtcNow.AddDays(10), IncludeBooks = new[] { new StatisticsBook { Id = 1 } } };
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1, CreatedAt = DateTime.UtcNow, OrderAmount = 2, TotalPrice = 100, OrderStatus = OrderStatus.Completed,
                    OrderBooks = new List<OrderBook>
                    {
                        new OrderBook { BookId = 1, BookAmount = 2 }
                    }
                }
            };
            var orderDbSetMock = GetDbSetMock(orders);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orderDbSetMock.Object);
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
            Assert.That(result.AveragePrice, Is.EqualTo(100));
            Assert.That(result.EarnedMoney, Is.EqualTo(100));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Order>(cancellationToken), Times.Exactly(8));
        }
        [Test]
        public async Task GetInOrderCopiesAsync_ReturnsCorrectOrderCopies()
        {
            // Arrange
            var getBookStatistics = new GetShopStatistics { FromUTC = DateTime.UtcNow.AddDays(-10), ToUTC = DateTime.UtcNow.AddDays(10) };
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
            var result = await (Task<long>)metohdInfo.Invoke(service, new object[] { getBookStatistics, cancellationToken });
            // Assert
            Assert.That(result, Is.EqualTo(2));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Order>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetCanceledCopiesAsync_ReturnsCorrectCanceledOrderCopies()
        {
            // Arrange
            var getBookStatistics = new GetShopStatistics { FromUTC = DateTime.UtcNow.AddDays(-10), ToUTC = DateTime.UtcNow.AddDays(10) };
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
            var result = await (Task<long>)metohdInfo.Invoke(service, new object[] { getBookStatistics, cancellationToken });
            // Assert
            Assert.That(result, Is.EqualTo(2));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Order>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetOrderAmountAsync_ReturnsCorrectOrderCount()
        {
            // Arrange
            var getBookStatistics = new GetShopStatistics { FromUTC = DateTime.UtcNow.AddDays(-10), ToUTC = DateTime.UtcNow.AddDays(10) };
            var orders = new List<Order>
            {
                new Order { Id = 1, CreatedAt = DateTime.UtcNow, OrderAmount = 2, OrderStatus = OrderStatus.Completed },
                new Order { Id = 2, CreatedAt = DateTime.UtcNow, OrderAmount = 1, OrderStatus = OrderStatus.Canceled }
            };
            var orderDbSetMock = GetDbSetMock(orders);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orderDbSetMock.Object);
            // Act
            var methodInfo = service.GetType().GetMethod("GetOrderAmountAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = await (Task<long>)methodInfo.Invoke(service, new object[] { getBookStatistics, cancellationToken });
            // Assert
            Assert.That(result, Is.EqualTo(2));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Order>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetCanceledOrdersAsync_ReturnsCorrectCanceledOrderCount()
        {
            // Arrange
            var getBookStatistics = new GetShopStatistics { FromUTC = DateTime.UtcNow.AddDays(-10), ToUTC = DateTime.UtcNow.AddDays(10) };
            var orders = new List<Order>
            {
                new Order { Id = 1, CreatedAt = DateTime.UtcNow, OrderAmount = 2, OrderStatus = OrderStatus.Canceled },
                new Order { Id = 2, CreatedAt = DateTime.UtcNow, OrderAmount = 1, OrderStatus = OrderStatus.Completed }
            };
            var orderDbSetMock = GetDbSetMock(orders);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orderDbSetMock.Object);
            // Act
            var methodInfo = service.GetType().GetMethod("GetCanceledOrdersAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = await (Task<long>)methodInfo.Invoke(service, new object[] { getBookStatistics, cancellationToken });
            // Assert
            Assert.That(result, Is.EqualTo(1));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Order>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetAveragePriceAsync_ReturnsCorrectAveragePrice()
        {
            // Arrange
            var getBookStatistics = new GetShopStatistics();
            var orders = new List<Order>
            {
                new Order { Id = 1, CreatedAt = DateTime.UtcNow, OrderAmount = 2, OrderStatus = OrderStatus.Canceled, TotalPrice = 150 },
                new Order { Id = 2, CreatedAt = DateTime.UtcNow, OrderAmount = 1, OrderStatus = OrderStatus.Completed, TotalPrice = 0 }
            };
            var orderDbSetMock = GetDbSetMock(orders);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Order>(cancellationToken)).ReturnsAsync(orderDbSetMock.Object);
            // Act
            var methodInfo = service.GetType().GetMethod("GetAveragePriceAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = await (Task<decimal>)methodInfo.Invoke(service, new object[] { getBookStatistics, cancellationToken });
            // Assert
            Assert.That(result, Is.EqualTo(75));
        }
        [Test]
        public async Task GetEarnedMoneyAsync_ReturnsCorrectAmount()
        {
            // Arrange
            var getBookStatistics = new GetShopStatistics { FromUTC = DateTime.UtcNow.AddDays(-10), ToUTC = DateTime.UtcNow.AddDays(10) };
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1, CreatedAt = DateTime.UtcNow, OrderAmount = 2, OrderStatus = OrderStatus.Completed, TotalPrice= 100,
                    OrderBooks = new List<OrderBook>
                    {
                        new OrderBook { BookId = 1, BookAmount = 2 }
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
        public async Task GetOrderAmountInDaysAsync_ReturnsCorrectOrderCountPerDay()
        {
            // Arrange
            var getBookStatistics = new GetShopStatistics();

            var orders = new List<Order>
            {
                new Order { Id = 1, CreatedAt = new DateTime(2024, 4, 13), OrderAmount = 5 },
                new Order { Id = 2, CreatedAt = new DateTime(2024, 4, 13), OrderAmount = 3},
                new Order { Id = 3, CreatedAt = new DateTime(2024, 5, 14), OrderAmount = 2}
            };

            var orderDbSetMock = GetDbSetMock(orders);
            repositoryMock
                .Setup(repo => repo.GetQueryableAsync<Order>(cancellationToken))
                .ReturnsAsync(orderDbSetMock.Object);
            // Act
            var methodInfo = service.GetType()
                .GetMethod("GetOrderAmountInDaysAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = await (Task<Dictionary<DateTime, long>>)methodInfo
                .Invoke(service, new object[] { getBookStatistics, cancellationToken });
            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[new DateTime(2024, 4, 13)], Is.EqualTo(2));
            Assert.That(result[new DateTime(2024, 5, 14)], Is.EqualTo(1));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Order>(cancellationToken), Times.Once);
        }
    }
}