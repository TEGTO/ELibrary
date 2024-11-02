using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Features.StockBookOrderFeature.Services;

namespace ShopApi.Features.OrderFeature.Command.ManagerCancelOrder.Tests
{
    [TestFixture]
    internal class ManagerCancelOrderCommandHandlerTests
    {
        private Mock<IOrderService> orderServiceMock;
        private Mock<IStockBookOrderService> stockBookOrderServiceMock;
        private ManagerCancelOrderCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            orderServiceMock = new Mock<IOrderService>();
            stockBookOrderServiceMock = new Mock<IStockBookOrderService>();
            handler = new ManagerCancelOrderCommandHandler(orderServiceMock.Object, stockBookOrderServiceMock.Object);
        }

        [Test]
        public async Task Handle_OrderExists_OrderCanceledAndStockBookOrderAdded()
        {
            // Arrange
            var order = new Order { Id = 1, OrderStatus = OrderStatus.InProcessing };
            var command = new ManagerCancelOrderCommand(order.Id);
            orderServiceMock.Setup(x => x.GetOrderByIdAsync(order.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            orderServiceMock.Setup(x => x.UpdateOrderAsync(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            // Act
            await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.Canceled));
            orderServiceMock.Verify(x => x.UpdateOrderAsync(It.Is<Order>(o => o.OrderStatus == OrderStatus.Canceled), It.IsAny<CancellationToken>()), Times.Once);
            stockBookOrderServiceMock.Verify(x => x.AddStockBookOrderAsyncFromCanceledOrderAsync(order, StockBookOrderType.ManagerOrderCancel, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void Handle_OrderDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = new ManagerCancelOrderCommand(99);
            orderServiceMock.Setup(x => x.GetOrderByIdAsync(command.OrderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Order not found."));
            orderServiceMock.Verify(x => x.UpdateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
            stockBookOrderServiceMock.Verify(x => x.AddStockBookOrderAsyncFromCanceledOrderAsync(It.IsAny<Order>(), It.IsAny<StockBookOrderType>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public async Task Handle_OrderCanceled_OrderStatusUpdatedAndStockBookOrderAdded()
        {
            // Arrange
            var order = new Order { Id = 1, OrderStatus = OrderStatus.Completed };
            var command = new ManagerCancelOrderCommand(order.Id);
            orderServiceMock.Setup(x => x.GetOrderByIdAsync(order.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            orderServiceMock.Setup(x => x.UpdateOrderAsync(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            // Act
            await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.Canceled));
            orderServiceMock.Verify(x => x.UpdateOrderAsync(order, It.IsAny<CancellationToken>()), Times.Once);
            stockBookOrderServiceMock.Verify(x => x.AddStockBookOrderAsyncFromCanceledOrderAsync(order, StockBookOrderType.ManagerOrderCancel, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}