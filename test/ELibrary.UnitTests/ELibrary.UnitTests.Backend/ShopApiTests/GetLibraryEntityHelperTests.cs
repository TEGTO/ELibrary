using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Services;

namespace ShopApi.Tests
{
    [TestFixture]
    internal class GetLibraryEntityHelperTests
    {
        private Mock<ILibraryService> mockLibraryService;
        private Mock<IMapper> mockMapper;

        [SetUp]
        public void SetUp()
        {
            mockLibraryService = new Mock<ILibraryService>();
            mockMapper = new Mock<IMapper>();
        }

        [Test]
        public async Task GetBookResponsesForIdsAsync_ValidIds_ReturnsExpectedBookResponses()
        {
            // Arrange
            var ids = new List<int> { 1, 2 };

            var expectedBooks = new List<BookResponse>
            {
                new BookResponse { Id = 1, Name = "Book 1" },
                new BookResponse { Id = 2, Name = "Book 2" }
            };

            mockLibraryService.Setup(service => service.GetByIdsAsync<BookResponse>(
                ids,
                $"/{LibraryConfiguration.LIBRARY_API_GET_BOOKS_BY_IDS_ENDPOINT}",
                CancellationToken.None))
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await GetLibraryEntityHelper.GetBookResponsesForIdsAsync(ids, mockLibraryService.Object, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBooks));
        }
        [Test]
        public void GetBookResponsesForIdsAsync_InvalidData_ThrowsInvalidDataException()
        {
            // Arrange
            var ids = new List<int> { 1, 2, 3 };

            var availableBooks = new List<BookResponse>
            {
                new BookResponse { Id = 1, Name = "Book 1" },
                new BookResponse { Id = 2, Name = "Book 2" }
            };

            mockLibraryService.Setup(service => service.GetByIdsAsync<BookResponse>(
                ids,
                $"/{LibraryConfiguration.LIBRARY_API_GET_BOOKS_BY_IDS_ENDPOINT}",
                CancellationToken.None))
                .ReturnsAsync(availableBooks);

            // Act & Assert
            Assert.ThrowsAsync<InvalidDataException>(() =>
                GetLibraryEntityHelper.GetBookResponsesForIdsAsync(ids, mockLibraryService.Object, CancellationToken.None));
        }
        [Test]
        public async Task GetCartResponseWithBooksAsync_ValidCart_ReturnsExpectedCartResponse()
        {
            // Arrange
            var cart = new Cart
            {
                Books = new List<CartBook>
                {
                    new CartBook { BookId = 1, BookAmount = 1 },
                    new CartBook { BookId = 2, BookAmount = 2 }
                }
            };

            var bookResponses = new List<BookResponse>
            {
                new BookResponse { Id = 1, Name = "Book 1" },
                new BookResponse { Id = 2, Name = "Book 2" }
            };

            var cartResponse = new CartResponse
            {
                Books = new List<CartBookResponse>
                {
                    new CartBookResponse { BookId = 1 },
                    new CartBookResponse { BookId = 2 }
                }
            };

            mockLibraryService.Setup(service => service.GetByIdsAsync<BookResponse>(
                It.IsAny<IEnumerable<int>>(),
                It.IsAny<string>(),
                CancellationToken.None))
                .ReturnsAsync(bookResponses);

            mockMapper.Setup(mapper => mapper.Map<CartResponse>(cart)).Returns(cartResponse);

            // Act
            var result = await GetLibraryEntityHelper.GetCartResponseWithBooksAsync(
                cart,
                mockLibraryService.Object,
                mockMapper.Object,
                CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(cartResponse));
            Assert.That(result.Books[0].Book, Is.EqualTo(bookResponses[0]));
            Assert.That(result.Books[1].Book, Is.EqualTo(bookResponses[1]));
        }
        [Test]
        public async Task GetOrderResponsesWithBooksAsync_ValidOrders_ReturnsExpectedOrderResponses()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    OrderBooks = new List<OrderBook>
                    {
                        new OrderBook { BookId = 1 },
                        new OrderBook { BookId = 2 }
                    }
                }
            };

            var orderResponses = new List<OrderResponse>
            {
                new OrderResponse
                {
                    OrderBooks = new List<OrderBookResponse>
                    {
                        new OrderBookResponse { BookId = 1 },
                        new OrderBookResponse { BookId = 2 }
                    }
                }
            };

            var bookResponses = new List<BookResponse>
            {
                new BookResponse { Id = 1, Name = "Book 1" },
                new BookResponse { Id = 2, Name = "Book 2" }
            };

            mockLibraryService.Setup(service => service.GetByIdsAsync<BookResponse>(
                It.IsAny<IEnumerable<int>>(),
                It.IsAny<string>(),
                CancellationToken.None))
                .ReturnsAsync(bookResponses);

            mockMapper.Setup(mapper => mapper.Map<OrderResponse>(It.IsAny<Order>()))
                .Returns((Order order) => orderResponses[orders.IndexOf(order)]);

            // Act
            var result = await GetLibraryEntityHelper.GetOrderResponsesWithBooksAsync(
                orders,
                mockLibraryService.Object,
                mockMapper.Object,
                CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(orderResponses));
            Assert.That(orderResponses[0].OrderBooks[0].Book, Is.EqualTo(bookResponses[0]));
            Assert.That(orderResponses[0].OrderBooks[1].Book, Is.EqualTo(bookResponses[1]));
        }
        [Test]
        public async Task GetStockBookOrderResponseWithBooksAsync_ValidOrders_ReturnsExpectedStockBookOrderResponses()
        {
            // Arrange
            var stockOrders = new List<StockBookOrder>
            {
                new StockBookOrder
                {
                    StockBookChanges = new List<StockBookChange>
                    {
                        new StockBookChange(),
                        new StockBookChange()
                    }
                }
            };

            var stockOrderResponses = new List<StockBookOrderResponse>
            {
                new StockBookOrderResponse
                {
                    StockBookChanges = new List<StockBookChangeResponse>
                    {
                        new StockBookChangeResponse { BookId = 1 },
                        new StockBookChangeResponse { BookId = 2 }
                    }
                }
            };

            var bookResponses = new List<BookResponse>
            {
                new BookResponse { Id = 1, Name = "Book 1" },
                new BookResponse { Id = 2, Name = "Book 2" }
            };

            mockLibraryService.Setup(service => service.GetByIdsAsync<BookResponse>(
                It.IsAny<IEnumerable<int>>(),
                It.IsAny<string>(),
                CancellationToken.None))
                .ReturnsAsync(bookResponses);

            mockMapper.Setup(mapper => mapper.Map<StockBookOrderResponse>(It.IsAny<StockBookOrder>()))
                .Returns((StockBookOrder order) => stockOrderResponses[stockOrders.IndexOf(order)]);

            // Act
            var result = await GetLibraryEntityHelper.GetStockBookOrderResponseWithBooksAsync(
                stockOrders,
                mockLibraryService.Object,
                mockMapper.Object,
                CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(stockOrderResponses));
            Assert.That(stockOrderResponses[0].StockBookChanges[0].Book, Is.EqualTo(bookResponses[0]));
            Assert.That(stockOrderResponses[0].StockBookChanges[1].Book, Is.EqualTo(bookResponses[1]));
        }
    }
}