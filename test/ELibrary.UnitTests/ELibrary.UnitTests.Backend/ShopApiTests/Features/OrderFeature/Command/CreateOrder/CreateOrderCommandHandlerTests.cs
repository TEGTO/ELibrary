using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Features.StockBookOrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.CreateOrder.Tests
{
    [TestFixture]
    internal class CreateOrderCommandHandlerTests
    {
        private Mock<IOrderService> mockOrderService;
        private Mock<IClientService> mockClientService;
        private Mock<IStockBookOrderService> mockStockBookOrderService;
        private Mock<ILibraryService> mockLibraryService;
        private Mock<IMapper> mockMapper;
        private CreateOrderCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockOrderService = new Mock<IOrderService>();
            mockClientService = new Mock<IClientService>();
            mockStockBookOrderService = new Mock<IStockBookOrderService>();
            mockLibraryService = new Mock<ILibraryService>();
            mockMapper = new Mock<IMapper>();

            handler = new CreateOrderCommandHandler(
                mockOrderService.Object,
                mockClientService.Object,
                mockStockBookOrderService.Object,
                mockLibraryService.Object,
                mockMapper.Object
            );
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsOrderResponse()
        {
            // Arrange
            var clientId = "123";
            var userId = "user1";
            var createOrderRequest = new CreateOrderRequest { OrderBooks = new List<OrderBookRequest> { new OrderBookRequest { BookId = 1, BookAmount = 2 } } };
            var command = new CreateOrderCommand(userId, createOrderRequest);
            var client = new Client { Id = clientId };
            mockClientService.Setup(x => x.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(client);
            var order = new Order { ClientId = clientId, OrderBooks = new List<OrderBook> { new OrderBook { BookId = 1 } } };
            mockMapper.Setup(x => x.Map<Order>(createOrderRequest)).Returns(order);
            var bookResponse = new BookResponse { Id = 1, Price = 10 };
            var bookResponses = new List<BookResponse> { bookResponse };
            mockLibraryService.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(bookResponses);
            mockOrderService.Setup(x => x.CreateOrderAsync(order, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            var expectedResponse = new OrderResponse { Id = 1, OrderBooks = new List<OrderBookResponse>() };
            mockMapper.Setup(x => x.Map<OrderResponse>(order)).Returns(expectedResponse);
            mockOrderService.Setup(x => x.CreateOrderAsync(order, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            mockLibraryService.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(bookResponses);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(result.Id, Is.EqualTo(expectedResponse.Id));
            mockOrderService.Verify(x => x.CreateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            mockLibraryService.Verify(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void Handle_ClientNotFound_ThrowsInvalidDataException()
        {
            // Arrange
            var userId = "user1";
            var createOrderRequest = new CreateOrderRequest { OrderBooks = new List<OrderBookRequest> { new OrderBookRequest { BookId = 1, BookAmount = 2 } } };
            var command = new CreateOrderCommand(userId, createOrderRequest);
            mockClientService.Setup(x => x.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((Client)null);
            // Act & Assert
            Assert.ThrowsAsync<InvalidDataException>(() => handler.Handle(command, CancellationToken.None));
            mockClientService.Verify(x => x.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task Handle_ValidRequest_SetsOrderBooksPrices()
        {
            // Arrange
            var userId = "user1";
            var clientId = "123";
            var createOrderRequest = new CreateOrderRequest { OrderBooks = new List<OrderBookRequest> { new OrderBookRequest { BookId = 1, BookAmount = 2 } } };
            var command = new CreateOrderCommand(userId, createOrderRequest);
            var client = new Client { Id = clientId };
            mockClientService.Setup(x => x.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(client);
            var order = new Order { ClientId = clientId, OrderBooks = new List<OrderBook> { new OrderBook { BookId = 1 } } };
            mockMapper.Setup(x => x.Map<Order>(createOrderRequest)).Returns(order);
            var bookResponse = new BookResponse { Id = 1, Price = 10 };
            var bookResponses = new List<BookResponse> { bookResponse };
            mockLibraryService.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(bookResponses);
            mockOrderService.Setup(x => x.CreateOrderAsync(order, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            var orderResponse = new OrderResponse { OrderBooks = new List<OrderBookResponse> { new OrderBookResponse { BookId = 1 } } };
            mockMapper.Setup(x => x.Map<OrderResponse>(order)).Returns(orderResponse);
            mockOrderService.Setup(x => x.CreateOrderAsync(order, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            mockLibraryService.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(bookResponses);
            // Act
            await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(order.OrderBooks.First().BookPrice, Is.EqualTo(10));
            mockLibraryService.Verify(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task Handle_ValidRequest_RaisesBookPopularity()
        {
            // Arrange
            var userId = "user1";
            var clientId = "123";
            var createOrderRequest = new CreateOrderRequest { OrderBooks = new List<OrderBookRequest> { new OrderBookRequest { BookId = 1, BookAmount = 2 } } };
            var command = new CreateOrderCommand(userId, createOrderRequest);
            var client = new Client { Id = clientId };
            mockClientService.Setup(x => x.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(client);
            var order = new Order { ClientId = clientId, OrderBooks = new List<OrderBook> { new OrderBook { BookId = 1 } } };
            mockMapper.Setup(x => x.Map<Order>(createOrderRequest)).Returns(order);
            var orderResponse = new OrderResponse { OrderBooks = new List<OrderBookResponse> { new OrderBookResponse { BookId = 1 } } };
            mockMapper.Setup(x => x.Map<OrderResponse>(order)).Returns(orderResponse);
            mockOrderService.Setup(x => x.CreateOrderAsync(order, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            var bookResponses = new List<BookResponse> { { new BookResponse() } };
            mockLibraryService.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(bookResponses);
            // Act
            await handler.Handle(command, CancellationToken.None);
            // Assert
            mockLibraryService.Verify(x => x.RaiseBookPopularityByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}