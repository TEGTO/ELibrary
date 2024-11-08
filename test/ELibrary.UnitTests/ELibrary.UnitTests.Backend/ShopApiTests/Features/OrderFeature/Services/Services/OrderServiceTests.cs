using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;
using LibraryShopEntities.Repositories.Shop;
using Moq;
using ShopApi.Features.OrderFeature.Services;

namespace ShopApiTests.Features.OrderFeature.Services.Services
{
    [TestFixture]
    internal class OrderServiceTests
    {
        private Mock<IOrderRepository> mockRepository;
        private OrderService orderService;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IOrderRepository>();
            orderService = new OrderService(mockRepository.Object);
        }

        [Test]
        public async Task GetOrderByIdAsync_OrderExists_ReturnsOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId, DeliveryAddress = "Address 1" };
            mockRepository.Setup(r => r.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
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
            mockRepository.Setup(r => r.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Order?)null);
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
            var orders = new List<Order>
            {
                new Order { Id = 1, DeliveryAddress = "Address 1" },
                new Order { Id = 2, DeliveryAddress = "Address 2" }
            };
            mockRepository.Setup(r => r.GetPaginatedOrdersAsync(filter, It.IsAny<CancellationToken>()))
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
            mockRepository.Setup(r => r.GetOrderCountAsync(filter, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(2);
            // Act
            var result = await orderService.GetOrderAmountAsync(filter, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(2));
        }
        [Test]
        public async Task CreateOrderAsync_ReturnsCreatedOrderWithCalculatedTotals()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                DeliveryAddress = "Test Address",
                OrderBooks = new List<OrderBook>
                {
                    new OrderBook { BookId = 1, BookAmount = 1, BookPrice = 10 }
                }
            };
            mockRepository.Setup(r => r.AddOrderAsync(order, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            mockRepository.Setup(r => r.GetOrderByIdAsync(order.Id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            // Act
            var result = await orderService.CreateOrderAsync(order, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.TotalPrice, Is.EqualTo(10));
            mockRepository.Verify(r => r.AddOrderAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task UpdateOrderAsync_OrderInProcessing_UpdatesOrder()
        {
            // Arrange
            var order = new Order { Id = 1, DeliveryAddress = "Updated Address", OrderStatus = OrderStatus.InProcessing };
            mockRepository.Setup(r => r.GetOrderByIdAsync(order.Id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            mockRepository.Setup(r => r.UpdateOrderAsync(order, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            // Act
            var result = await orderService.UpdateOrderAsync(order, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.DeliveryAddress, Is.EqualTo("Updated Address"));
            mockRepository.Verify(r => r.UpdateOrderAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void UpdateOrderAsync_OrderNotInProcessing_ThrowsInvalidOperationException()
        {
            // Arrange
            var order = new Order { Id = 1, OrderStatus = OrderStatus.Completed };
            mockRepository.Setup(r => r.GetOrderByIdAsync(order.Id, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => orderService.UpdateOrderAsync(order, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Only orders that are in processing can be updated."));
        }
        [Test]
        public async Task DeleteOrderAsync_OrderExists_DeletesOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId };
            mockRepository.Setup(r => r.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            mockRepository.Setup(r => r.DeleteOrderAsync(order, It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);
            // Act
            await orderService.DeleteOrderAsync(orderId, CancellationToken.None);
            // Assert
            mockRepository.Verify(r => r.DeleteOrderAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task DeleteOrderAsync_OrderDoesNotExist_DoesNotDeleteOrder()
        {
            // Arrange
            var orderId = 1;
            mockRepository.Setup(r => r.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Order?)null);
            // Act
            await orderService.DeleteOrderAsync(orderId, CancellationToken.None);
            // Assert
            mockRepository.Verify(r => r.DeleteOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}