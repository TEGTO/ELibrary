using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.ManagerUpdateOrder.Tests
{
    [TestFixture]
    internal class ManagerUpdateOrderCommandHandlerTests
    {
        private Mock<IOrderService> orderServiceMock;
        private Mock<ILibraryService> libraryServiceMock;
        private Mock<IMapper> mapperMock;
        private ManagerUpdateOrderCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            orderServiceMock = new Mock<IOrderService>();
            libraryServiceMock = new Mock<ILibraryService>();
            mapperMock = new Mock<IMapper>();
            handler = new ManagerUpdateOrderCommandHandler(orderServiceMock.Object, libraryServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsUpdatedOrderResponse()
        {
            // Arrange
            var request = new ManagerUpdateOrderRequest
            {
                Id = 1,
                DeliveryAddress = "123 Test St",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderStatus = OrderStatus.InProcessing,
            };
            var command = new ManagerUpdateOrderCommand(request);
            var mappedOrder = new Order
            {
                Id = 1,
                DeliveryAddress = request.DeliveryAddress,
                DeliveryTime = request.DeliveryTime,
                OrderStatus = request.OrderStatus
            };
            var updatedOrder = mappedOrder;
            var expectedResponse = new OrderResponse
            {
                Id = updatedOrder.Id,
                DeliveryAddress = updatedOrder.DeliveryAddress,
                DeliveryTime = updatedOrder.DeliveryTime,
                OrderStatus = updatedOrder.OrderStatus,
                OrderBooks = new List<OrderBookResponse>()
            };
            mapperMock.Setup(m => m.Map<Order>(request)).Returns(mappedOrder);
            orderServiceMock.Setup(s => s.UpdateOrderAsync(mappedOrder, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(updatedOrder);
            libraryServiceMock.Setup(s => s.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<BookResponse>());
            mapperMock.Setup(m => m.Map<OrderResponse>(updatedOrder)).Returns(expectedResponse);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(expectedResponse.Id));
            Assert.That(result.DeliveryAddress, Is.EqualTo(expectedResponse.DeliveryAddress));
            Assert.That(result.DeliveryTime, Is.EqualTo(expectedResponse.DeliveryTime));
            Assert.That(result.OrderStatus, Is.EqualTo(expectedResponse.OrderStatus));
            mapperMock.Verify(m => m.Map<Order>(request), Times.Once);
            orderServiceMock.Verify(s => s.UpdateOrderAsync(mappedOrder, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Handle_OrderNotFound_ThrowsNullReferenceException()
        {
            // Arrange
            var request = new ManagerUpdateOrderRequest
            {
                Id = 99,
                DeliveryAddress = "123 Test St",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderStatus = OrderStatus.InProcessing
            };
            var command = new ManagerUpdateOrderCommand(request);
            var mappedOrder = new Order { Id = 99 };
            mapperMock.Setup(m => m.Map<Order>(request)).Returns(mappedOrder);
            orderServiceMock.Setup(s => s.UpdateOrderAsync(mappedOrder, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Order)null);
            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(command, CancellationToken.None));
        }
        [Test]
        public async Task Handle_BooksInOrder_MapsOrderWithBooks()
        {
            // Arrange
            var request = new ManagerUpdateOrderRequest { Id = 1, DeliveryAddress = "456 Another St", DeliveryTime = DateTime.UtcNow.AddDays(2) };
            var command = new ManagerUpdateOrderCommand(request);
            var mappedOrder = new Order { Id = 1, DeliveryAddress = request.DeliveryAddress, DeliveryTime = request.DeliveryTime };
            var updatedOrder = mappedOrder;
            var bookResponse = new BookResponse { Id = 1, Name = "Sample Book" };
            var orderResponse = new OrderResponse
            {
                Id = updatedOrder.Id,
                OrderBooks = new List<OrderBookResponse> { new OrderBookResponse { BookId = bookResponse.Id, Book = bookResponse } }
            };
            mapperMock.Setup(m => m.Map<Order>(request)).Returns(mappedOrder);
            orderServiceMock.Setup(s => s.UpdateOrderAsync(mappedOrder, It.IsAny<CancellationToken>())).ReturnsAsync(updatedOrder);
            libraryServiceMock.Setup(s => s.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<BookResponse> { bookResponse });
            mapperMock.Setup(m => m.Map<OrderResponse>(updatedOrder)).Returns(orderResponse);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(orderResponse.Id));
            Assert.That("Sample Book", Is.EqualTo(orderResponse.OrderBooks.First().Book.Name));
            mapperMock.Verify(m => m.Map<Order>(request), Times.Once);
            orderServiceMock.Verify(s => s.UpdateOrderAsync(mappedOrder, It.IsAny<CancellationToken>()), Times.Once);
            libraryServiceMock.Verify(s => s.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}