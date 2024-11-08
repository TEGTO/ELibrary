using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;
using Moq;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.GetOrders.Tests
{
    [TestFixture]
    internal class GetOrdersQueryHandlerTests
    {
        private Mock<IOrderService> orderServiceMock;
        private Mock<IClientService> mockClientService;
        private Mock<ILibraryService> libraryServiceMock;
        private Mock<IMapper> mapperMock;
        private GetOrdersQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            orderServiceMock = new Mock<IOrderService>();
            mockClientService = new Mock<IClientService>();
            libraryServiceMock = new Mock<ILibraryService>();
            mapperMock = new Mock<IMapper>();
            handler = new GetOrdersQueryHandler(orderServiceMock.Object, mockClientService.Object, libraryServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public void Handle_ClientNotFound_ThrowsInvalidDataException()
        {
            // Arrange
            mockClientService.Setup(x => x.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null);
            var command = new GetOrdersQuery("user-id", new GetOrdersFilter());
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidDataException>(() => handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Client is not found!"));
        }

        [Test]
        public async Task Handle_ValidClient_ReturnsMappedOrders()
        {
            // Arrange
            var client = new Client { Id = "client-id" };
            var orders = new List<Order>
            {
                new Order { Id = 1, ClientId = "client-id", OrderBooks = new List<OrderBook>() },
                new Order { Id = 2, ClientId = "client-id", OrderBooks = new List<OrderBook>() }
            };
            mockClientService.Setup(x => x.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            orderServiceMock.Setup(x => x.GetPaginatedOrdersAsync(It.IsAny<GetOrdersFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);
            mapperMock.Setup(m => m.Map<OrderResponse>(It.IsAny<Order>())).Returns(new OrderResponse());
            var bookResponse = new BookResponse { Id = 1, Name = "Sample Book" };
            var orderResponse = new OrderResponse
            {
                Id = 1,
                OrderBooks = new List<OrderBookResponse> { new OrderBookResponse { BookId = 1, BookPrice = 100 } }
            };
            mapperMock.Setup(m => m.Map<OrderResponse>(It.IsAny<Order>())).Returns(orderResponse);
            libraryServiceMock.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookResponse> { bookResponse });
            var command = new GetOrdersQuery("user-id", new GetOrdersFilter());
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            mockClientService.Verify(x => x.GetClientByUserIdAsync("user-id", It.IsAny<CancellationToken>()), Times.Once);
            orderServiceMock.Verify(x => x.GetPaginatedOrdersAsync(It.Is<GetOrdersFilter>(f => f.ClientId == "client-id"), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task Handle_ValidClientAndBooks_ReturnsOrderWithBooks()
        {
            // Arrange
            var client = new Client { Id = "client-id" };
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    ClientId = "client-id",
                    OrderBooks = new List<OrderBook> { new OrderBook { BookId = 1, BookPrice = 100 } }
                }
            };
            var bookResponse = new BookResponse { Id = 1, Name = "Sample Book" };
            var orderResponse = new OrderResponse
            {
                Id = 1,
                OrderBooks = new List<OrderBookResponse> { new OrderBookResponse { BookId = 1, BookPrice = 100 } }
            };
            mockClientService.Setup(x => x.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            orderServiceMock.Setup(x => x.GetPaginatedOrdersAsync(It.IsAny<GetOrdersFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);
            mapperMock.Setup(m => m.Map<OrderResponse>(It.IsAny<Order>())).Returns(orderResponse);
            libraryServiceMock.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookResponse> { bookResponse });
            var command = new GetOrdersQuery("user-id", new GetOrdersFilter());
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var orderWithBooks = result.FirstOrDefault();
            // Assert
            Assert.IsNotNull(orderWithBooks);
            Assert.That(orderWithBooks.OrderBooks.First().Book.Name, Is.EqualTo("Sample Book"));
            mockClientService.Verify(x => x.GetClientByUserIdAsync("user-id", It.IsAny<CancellationToken>()), Times.Once);
            orderServiceMock.Verify(x => x.GetPaginatedOrdersAsync(It.Is<GetOrdersFilter>(f => f.ClientId == "client-id"), It.IsAny<CancellationToken>()), Times.Once);
            libraryServiceMock.Verify(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}