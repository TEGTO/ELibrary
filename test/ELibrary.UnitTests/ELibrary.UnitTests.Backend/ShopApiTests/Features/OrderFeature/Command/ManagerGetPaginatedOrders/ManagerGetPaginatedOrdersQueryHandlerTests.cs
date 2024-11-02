using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetPaginatedOrders.Tests
{
    [TestFixture]
    internal class ManagerGetPaginatedOrdersQueryHandlerTests
    {
        private Mock<IOrderService> orderServiceMock;
        private Mock<ILibraryService> libraryServiceMock;
        private Mock<IMapper> mapperMock;
        private ManagerGetPaginatedOrdersQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            orderServiceMock = new Mock<IOrderService>();
            libraryServiceMock = new Mock<ILibraryService>();
            mapperMock = new Mock<IMapper>();
            handler = new ManagerGetPaginatedOrdersQueryHandler(orderServiceMock.Object, libraryServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidFilter_ReturnsMappedOrderResponses()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var query = new ManagerGetPaginatedOrdersQuery(filter);
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    OrderBooks = new List<OrderBook> { new OrderBook { BookId = 1, BookAmount = 2 } }
                }
            };
            var bookResponse = new BookResponse { Id = 1, Name = "Test Book" };
            var expectedOrderResponse = new OrderResponse
            {
                Id = 1,
                OrderBooks = new List<OrderBookResponse> { new OrderBookResponse { BookId = 1 } }
            };
            orderServiceMock.Setup(x => x.GetPaginatedOrdersAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync(orders);
            libraryServiceMock.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookResponse> { bookResponse });
            mapperMock.Setup(m => m.Map<OrderResponse>(It.IsAny<Order>())).Returns(expectedOrderResponse);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(expectedOrderResponse.Id));
            Assert.That(result.First().OrderBooks.First().Book.Name, Is.EqualTo(bookResponse.Name));
            orderServiceMock.Verify(x => x.GetPaginatedOrdersAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
            libraryServiceMock.Verify(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task Handle_EmptyOrderList_ReturnsEmptyOrderResponseList()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var query = new ManagerGetPaginatedOrdersQuery(filter);
            orderServiceMock.Setup(x => x.GetPaginatedOrdersAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync(new List<Order>());
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.IsEmpty(result);
            orderServiceMock.Verify(x => x.GetPaginatedOrdersAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void Handle_OrdersWithMissingBooks_ThrowsNullReferenceException()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var query = new ManagerGetPaginatedOrdersQuery(filter);
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    OrderBooks = new List<OrderBook> { new OrderBook { BookId = 99, BookAmount = 2 } }
                }
            };
            orderServiceMock.Setup(x => x.GetPaginatedOrdersAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync(orders);
            libraryServiceMock.Setup(x => x.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookResponse>());
            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}