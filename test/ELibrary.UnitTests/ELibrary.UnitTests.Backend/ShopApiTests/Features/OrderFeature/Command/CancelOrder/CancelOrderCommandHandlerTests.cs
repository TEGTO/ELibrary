using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using Moq;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Features.StockBookOrderFeature.Services;

namespace ShopApi.Features.OrderFeature.Command.CancelOrder.Tests
{
    [TestFixture()]
    public class CancelOrderCommandHandlerTests
    {
        private Mock<IOrderService> mockOrderService;
        private Mock<IClientService> mockClientService;
        private Mock<IStockBookOrderService> mockStockBookOrderService;
        private CancelOrderCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockOrderService = new Mock<IOrderService>();
            mockClientService = new Mock<IClientService>();
            mockStockBookOrderService = new Mock<IStockBookOrderService>();
            handler = new CancelOrderCommandHandler(mockOrderService.Object, mockClientService.Object, mockStockBookOrderService.Object);
        }

        [Test]
        public async Task Handle_ValidOrderAndClient_CancelsOrderSuccessfully()
        {
            // Arrange
            var command = new CancelOrderCommand("user123", 1);
            var client = new Client { Id = "client123", UserId = "user123" };
            var order = new Order { Id = 1, ClientId = "client123", OrderStatus = OrderStatus.InProcessing };
            mockClientService.Setup(s => s.GetClientByUserIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(client);
            mockOrderService.Setup(s => s.GetOrderByIdAsync(command.OrderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            mockOrderService.Setup(s => s.UpdateOrderAsync(order, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(Unit.Value));
            Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.Canceled));
            mockOrderService.Verify(s => s.UpdateOrderAsync(order, It.IsAny<CancellationToken>()), Times.Once);
            mockStockBookOrderService.Verify(s => s.AddStockBookOrderAsyncFromCanceledOrderAsync(order, StockBookOrderType.ClientOrderCancel, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void Handle_NonexistentClient_ThrowsInvalidDataException()
        {
            // Arrange
            var command = new CancelOrderCommand("user123", 1);
            mockClientService.Setup(s => s.GetClientByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null);
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidDataException>(() => handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Client is not found!"));
        }
        [Test]
        public void Handle_NonexistentOrder_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = new CancelOrderCommand("user123", 1);
            var client = new Client { Id = "client123", UserId = "user123" };
            mockClientService.Setup(s => s.GetClientByUserIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(client);
            mockOrderService.Setup(s => s.GetOrderByIdAsync(command.OrderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Order not found."));
        }
        [Test]
        public void Handle_OrderNotInProcessingStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = new CancelOrderCommand("user123", 1);
            var client = new Client { Id = "client123", UserId = "user123" };
            var order = new Order { Id = 1, ClientId = "client123", OrderStatus = OrderStatus.Completed };
            mockClientService.Setup(s => s.GetClientByUserIdAsync(command.UserId, It.IsAny<CancellationToken>())).ReturnsAsync(client);
            mockOrderService.Setup(s => s.GetOrderByIdAsync(command.OrderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("It is not possible to client to cancel an order with this order status."));
        }
    }
}