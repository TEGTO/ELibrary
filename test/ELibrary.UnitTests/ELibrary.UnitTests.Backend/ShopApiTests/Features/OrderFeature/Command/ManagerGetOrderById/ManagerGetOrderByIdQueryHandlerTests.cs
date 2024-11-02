using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetOrderById.Tests
{
    [TestFixture]
    internal class ManagerGetOrderByIdQueryHandlerTests
    {
        private Mock<IOrderService> orderServiceMock;
        private Mock<ILibraryService> libraryServiceMock;
        private Mock<IMapper> mapperMock;
        private ManagerGetOrderByIdQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            orderServiceMock = new Mock<IOrderService>();
            libraryServiceMock = new Mock<ILibraryService>();
            mapperMock = new Mock<IMapper>();
            handler = new ManagerGetOrderByIdQueryHandler(orderServiceMock.Object, libraryServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_OrderExists_ReturnsOrderResponseWithBooks()
        {
            // Arrange
            var orderId = 1;
            var query = new ManagerGetOrderByIdQuery(orderId);
            var order = new Order
            {
                Id = orderId,
                OrderBooks = new List<OrderBook> { new OrderBook { BookId = 1, BookAmount = 2 } }
            };
            var bookResponse = new BookResponse { Id = 1, Name = "Test Book" };
            var orderResponse = new OrderResponse
            {
                Id = orderId,
                OrderBooks = new List<OrderBookResponse> { new OrderBookResponse { BookId = 1 } }
            };
            orderServiceMock.Setup(x => x.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            libraryServiceMock.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookResponse> { bookResponse });
            mapperMock.Setup(m => m.Map<OrderResponse>(order)).Returns(orderResponse);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(orderId));
            Assert.That(result.OrderBooks.First().Book.Name, Is.EqualTo(bookResponse.Name));
            orderServiceMock.Verify(x => x.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()), Times.Once);
            libraryServiceMock.Verify(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task Handle_OrderDoesNotExist_ReturnsNull()
        {
            // Arrange
            var orderId = 1;
            var query = new ManagerGetOrderByIdQuery(orderId);
            orderServiceMock.Setup(x => x.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>())).ReturnsAsync((Order)null);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
            orderServiceMock.Verify(x => x.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()), Times.Once);
            libraryServiceMock.Verify(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            mapperMock.Verify(x => x.Map<OrderResponse>(It.IsAny<Order>()), Times.Never);
        }
        [Test]
        public void Handle_BooksNotFoundInLibraryService_ThrowsException()
        {
            // Arrange
            var orderId = 1;
            var query = new ManagerGetOrderByIdQuery(orderId);
            var order = new Order
            {
                Id = orderId,
                OrderBooks = new List<OrderBook> { new OrderBook { BookId = 99 } }
            };
            orderServiceMock.Setup(x => x.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            // Simulate missing books by returning an empty list
            libraryServiceMock.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookResponse>());
            // Act & Assert
            Assert.ThrowsAsync<InvalidDataException>(() => handler.Handle(query, CancellationToken.None));
            orderServiceMock.Verify(x => x.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()), Times.Once);
            libraryServiceMock.Verify(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}