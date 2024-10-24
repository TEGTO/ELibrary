﻿using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.OrderFeature.Services;

namespace ShopApiTests.Features.OrderFeature.Services.Services
{
    [TestFixture]
    internal class OrderServiceTests
    {
        private Mock<IDatabaseRepository<LibraryShopDbContext>> mockRepository;
        private OrderService orderService;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IDatabaseRepository<LibraryShopDbContext>>();
            orderService = new OrderService(mockRepository.Object);
        }
        private IQueryable<T> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMock();
        }

        [Test]
        public async Task GetOrderByIdAsync_OrderExists_ReturnsOrder()
        {
            // Arrange
            var orderId = 1;
            var orders = GetDbSetMock(new List<Order>
            {
                new Order { Id = orderId, DeliveryAddress = "Address 1" }
            });
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderService.GetOrderByIdAsync(orderId, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(orderId));
            Assert.That(result.DeliveryAddress, Is.EqualTo("Address 1"));
        }
        [Test]
        public async Task GetOrderByIdAsync_OrderDoesNotExist_ReturnsNull()
        {
            // Arrange
            var orderId = 1;
            var orders = GetDbSetMock(new List<Order>());
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderService.GetOrderByIdAsync(orderId, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetPaginatedOrdersAsync_ValidRequest_ReturnsPaginatedOrders()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 2 };
            var orders = GetDbSetMock(new List<Order>
            {
                new Order { Id = 1, DeliveryAddress = "Address 1" },
                new Order { Id = 2, DeliveryAddress = "Address 2" }
            });
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderService.GetPaginatedOrdersAsync(filter, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.Last().Id, Is.EqualTo(2));
        }
        [Test]
        public async Task GetOrderAmountAsync_ValidData_ReturnsOrderCount()
        {
            // Arrange
            var filter = new GetOrdersFilter { ClientId = "test-client-id" };
            var orders = GetDbSetMock(new List<Order>
            {
                new Order { Id = 1, ClientId = "test-client-id" },
                new Order { Id = 2, ClientId = "test-client-id" }
            });
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderService.GetOrderAmountAsync(filter, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(2));
        }
        [Test]
        public async Task CreateOrderAsync_ReturnsCreatedOrder()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                DeliveryAddress = "Test Address",
                OrderBooks = new List<OrderBook>
                {
                    new OrderBook { BookId = 1, BookAmount = 1 }
                }
            };
            var books = GetDbSetMock(new List<Book>
            {
                new Book { Id = 1, Price = 10 }
            });

            mockRepository.Setup(r => r.GetQueryableAsync<Book>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(books);
            mockRepository.Setup(r => r.AddAsync(order, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                         .ReturnsAsync(GetDbSetMock(new List<Order> { order }));
            // Act
            var result = await orderService.CreateOrderAsync(order, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.TotalPrice, Is.EqualTo(10));
            mockRepository.Verify(r => r.AddAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task UpdateOrderAsync_ReturnsUpdatedOrder()
        {
            // Arrange
            var order = new Order { Id = 1, DeliveryAddress = "Updated Address" };
            var orders = GetDbSetMock(new List<Order> { order });

            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            mockRepository.Setup(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            // Act
            var result = await orderService.UpdateOrderAsync(order, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.DeliveryAddress, Is.EqualTo("Updated Address"));
            mockRepository.Verify(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task DeleteOrderAsync_DeletesOrder()
        {
            // Arrange
            var order = new Order { Id = 1, DeliveryAddress = "Test Address" };
            var orders = GetDbSetMock(new List<Order> { order });

            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            mockRepository.Setup(r => r.DeleteAsync(order, It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);
            // Act
            await orderService.DeleteOrderAsync(order.Id, CancellationToken.None);
            // Assert
            mockRepository.Verify(r => r.DeleteAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}