using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.UpdateOrder.Tests
{
    [TestFixture]
    internal class UpdateOrderCommandHandlerTests
    {
        private Mock<IOrderService> orderServiceMock;
        private Mock<IClientService> clientServiceMock;
        private Mock<ILibraryService> libraryServiceMock;
        private Mock<IMapper> mapperMock;
        private UpdateOrderCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            orderServiceMock = new Mock<IOrderService>();
            clientServiceMock = new Mock<IClientService>();
            libraryServiceMock = new Mock<ILibraryService>();
            mapperMock = new Mock<IMapper>();
            handler = new UpdateOrderCommandHandler(orderServiceMock.Object, clientServiceMock.Object, libraryServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsUpdatedOrderResponse()
        {
            // Arrange
            var userId = "test-user-id";
            var client = new Client { Id = "client-id" };
            var command = new UpdateOrderCommand(userId, new ClientUpdateOrderRequest
            {
                Id = 1,
                DeliveryAddress = "123 Test St",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                PaymentMethod = PaymentMethod.Cash,
                DeliveryMethod = DeliveryMethod.AddressDelivery
            });
            var existingOrder = new Order { Id = 1, ClientId = client.Id, OrderBooks = new List<OrderBook> { new OrderBook { BookId = 1 } } };
            var updatedOrder = existingOrder;
            var bookResponse = new BookResponse { Id = 1, Name = "Sample Book" };
            var expectedResponse = new OrderResponse { Id = 1, OrderBooks = new List<OrderBookResponse> { new OrderBookResponse { BookId = bookResponse.Id, Book = bookResponse } } };
            clientServiceMock.Setup(s => s.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(client);
            orderServiceMock.Setup(s => s.GetOrderByIdAsync(command.Request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingOrder);
            libraryServiceMock.Setup(s => s.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<BookResponse> { bookResponse });
            mapperMock.Setup(m => m.Map(command.Request, existingOrder)).Callback<ClientUpdateOrderRequest, Order>((src, dest) =>
            {
                dest.DeliveryAddress = src.DeliveryAddress;
                dest.DeliveryTime = src.DeliveryTime;
            });
            orderServiceMock.Setup(s => s.UpdateOrderAsync(existingOrder, It.IsAny<CancellationToken>())).ReturnsAsync(updatedOrder);
            mapperMock.Setup(m => m.Map<OrderResponse>(updatedOrder)).Returns(expectedResponse);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(expectedResponse.Id));
            Assert.That("Sample Book", Is.EqualTo(expectedResponse.OrderBooks.First().Book.Name));
            clientServiceMock.Verify(s => s.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            orderServiceMock.Verify(s => s.GetOrderByIdAsync(command.Request.Id, It.IsAny<CancellationToken>()), Times.Once);
            orderServiceMock.Verify(s => s.UpdateOrderAsync(existingOrder, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void Handle_ClientNotFound_ThrowsInvalidDataException()
        {
            // Arrange
            var userId = "test-user-id";
            var command = new UpdateOrderCommand(userId, new ClientUpdateOrderRequest { Id = 1 });
            clientServiceMock.Setup(s => s.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((Client)null);
            // Act & Assert
            Assert.ThrowsAsync<InvalidDataException>(() => handler.Handle(command, CancellationToken.None));
            clientServiceMock.Verify(s => s.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void Handle_OrderNotFoundOrInvalidClientId_ThrowsInvalidOperationException()
        {
            // Arrange
            var userId = "test-user-id";
            var client = new Client { Id = "client-id" };
            var command = new UpdateOrderCommand(userId, new ClientUpdateOrderRequest { Id = 1 });
            var existingOrder = new Order { Id = 1, ClientId = "different-client-id" };
            clientServiceMock.Setup(s => s.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(client);
            orderServiceMock.Setup(s => s.GetOrderByIdAsync(command.Request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingOrder);
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        }
        [Test]
        public async Task Handle_MapsUpdatedOrder_ReturnsOrderResponseWithBooks()
        {
            // Arrange
            var userId = "test-user-id";
            var client = new Client { Id = "client-id" };
            var command = new UpdateOrderCommand(userId, new ClientUpdateOrderRequest { Id = 1 });
            var existingOrder = new Order { Id = 1, ClientId = client.Id, OrderBooks = new List<OrderBook> { new OrderBook { BookId = 1 } } };
            var updatedOrder = existingOrder;
            var bookResponse = new BookResponse { Id = 1, Name = "Sample Book" };
            var expectedResponse = new OrderResponse { Id = 1, OrderBooks = new List<OrderBookResponse> { new OrderBookResponse { BookId = bookResponse.Id, Book = bookResponse } } };
            clientServiceMock.Setup(s => s.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(client);
            orderServiceMock.Setup(s => s.GetOrderByIdAsync(command.Request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existingOrder);
            libraryServiceMock.Setup(s => s.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<BookResponse> { bookResponse });
            mapperMock.Setup(m => m.Map(command.Request, existingOrder));
            orderServiceMock.Setup(s => s.UpdateOrderAsync(existingOrder, It.IsAny<CancellationToken>())).ReturnsAsync(updatedOrder);
            mapperMock.Setup(m => m.Map<OrderResponse>(updatedOrder)).Returns(expectedResponse);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(expectedResponse.Id));
            Assert.That("Sample Book", Is.EqualTo(expectedResponse.OrderBooks.First().Book.Name));
            clientServiceMock.Verify(s => s.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            orderServiceMock.Verify(s => s.GetOrderByIdAsync(command.Request.Id, It.IsAny<CancellationToken>()), Times.Once);
            orderServiceMock.Verify(s => s.UpdateOrderAsync(existingOrder, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}