using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using Shared.Domain.Dtos;
using ShopApi.Features.StockBookOrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderPaginated.Tests
{
    [TestFixture]
    internal class GetStockOrderPaginatedQueryHandlerTests
    {
        private Mock<IStockBookOrderService> stockBookOrderServiceMock;
        private Mock<ILibraryService> libraryServiceMock;
        private Mock<IMapper> mapperMock;
        private GetStockOrderPaginatedQueryHandler handler;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            stockBookOrderServiceMock = new Mock<IStockBookOrderService>();
            libraryServiceMock = new Mock<ILibraryService>();
            mapperMock = new Mock<IMapper>();
            handler = new GetStockOrderPaginatedQueryHandler(stockBookOrderServiceMock.Object, libraryServiceMock.Object, mapperMock.Object);
            cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task Handle_ValidPaginationRequest_ReturnsPaginatedStockOrders()
        {
            // Arrange
            var paginationRequest = new PaginationRequest { PageNumber = 1, PageSize = 10 };
            var stockOrders = new List<StockBookOrder>
            {
                new StockBookOrder { Id = 1 },
                new StockBookOrder { Id = 2 }
            };
            stockOrders.Select(order => new StockBookOrderResponse { Id = order.Id }).ToList();
            stockBookOrderServiceMock.Setup(s => s.GetPaginatedStockBookOrdersAsync(paginationRequest, cancellationToken))
                .ReturnsAsync(stockOrders);
            mapperMock.Setup(m => m.Map<StockBookOrderResponse>(It.IsAny<StockBookOrder>()))
                .Returns((StockBookOrder order) => new StockBookOrderResponse { Id = order.Id, StockBookChanges = new List<StockBookChangeResponse>() });
            libraryServiceMock.Setup(s => s.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(new List<BookResponse>());
            // Act
            var result = await handler.Handle(new GetStockOrderPaginatedQuery(paginationRequest), cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.Last().Id, Is.EqualTo(2));
            stockBookOrderServiceMock.Verify(s => s.GetPaginatedStockBookOrdersAsync(paginationRequest, cancellationToken), Times.Once);
        }
        [Test]
        public async Task Handle_EmptyResult_ReturnsEmptyList()
        {
            // Arrange
            var paginationRequest = new PaginationRequest { PageNumber = 1, PageSize = 10 };
            stockBookOrderServiceMock.Setup(s => s.GetPaginatedStockBookOrdersAsync(paginationRequest, cancellationToken))
                .ReturnsAsync(new List<StockBookOrder>());
            // Act
            var result = await handler.Handle(new GetStockOrderPaginatedQuery(paginationRequest), cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            stockBookOrderServiceMock.Verify(s => s.GetPaginatedStockBookOrdersAsync(paginationRequest, cancellationToken), Times.Once);
        }
    }
}