using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.StockBookOrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderById.Tests
{
    [TestFixture]
    internal class GetStockOrderByIdQueryHandlerTests
    {
        private Mock<IStockBookOrderService> stockBookOrderServiceMock;
        private Mock<ILibraryService> libraryServiceMock;
        private Mock<IMapper> mapperMock;
        private GetStockOrderByIdQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            stockBookOrderServiceMock = new Mock<IStockBookOrderService>();
            libraryServiceMock = new Mock<ILibraryService>();
            mapperMock = new Mock<IMapper>();

            handler = new GetStockOrderByIdQueryHandler(stockBookOrderServiceMock.Object, libraryServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidStockOrderId_ReturnsStockOrderResponse()
        {
            // Arrange
            var stockOrderId = 1;
            var stockBookOrder = new StockBookOrder
            {
                Id = stockOrderId,
                StockBookChanges = new List<StockBookChange>
                {
                    new StockBookChange { BookId = 1, ChangeAmount = 5 },
                    new StockBookChange { BookId = 2, ChangeAmount = 3 }
                }
            };

            var expectedResponse = new StockBookOrderResponse
            {
                Id = stockOrderId,
                StockBookChanges = stockBookOrder.StockBookChanges
                    .Select(change => new StockBookChangeResponse { BookId = change.BookId, ChangeAmount = change.ChangeAmount })
                    .ToList()
            };

            var bookResponses = new List<BookResponse>
            {
                new BookResponse { Id = 1, Name = "Book1" },
                new BookResponse { Id = 2, Name = "Book2" }
            };

            stockBookOrderServiceMock.Setup(s => s.GetStockBookOrderByIdAsync(stockOrderId, CancellationToken.None))
                .ReturnsAsync(stockBookOrder);

            mapperMock.Setup(m => m.Map<StockBookOrderResponse>(stockBookOrder))
                .Returns(expectedResponse);

            libraryServiceMock.Setup(s => s.GetByIdsAsync<BookResponse>(It.IsAny<IEnumerable<int>>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(bookResponses);

            // Act
            var result = await handler.Handle(new GetStockOrderByIdQuery(stockOrderId), CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(stockOrderId));
            Assert.That(result.StockBookChanges.Count, Is.EqualTo(2));
            Assert.That(result.StockBookChanges.First().Book.Name, Is.EqualTo("Book1"));
            Assert.That(result.StockBookChanges.Last().Book.Name, Is.EqualTo("Book2"));
        }
        [Test]
        public async Task Handle_StockOrderWithNoBookChanges_ReturnsResponseWithEmptyChanges()
        {
            // Arrange
            var stockOrderId = 1;

            var stockBookOrder = new StockBookOrder { Id = stockOrderId, StockBookChanges = new List<StockBookChange>() };

            var expectedResponse = new StockBookOrderResponse { Id = stockOrderId, StockBookChanges = new List<StockBookChangeResponse>() };

            stockBookOrderServiceMock.Setup(s => s.GetStockBookOrderByIdAsync(stockOrderId, CancellationToken.None))
                .ReturnsAsync(stockBookOrder);

            mapperMock.Setup(m => m.Map<StockBookOrderResponse>(stockBookOrder))
                .Returns(expectedResponse);

            // Act
            var result = await handler.Handle(new GetStockOrderByIdQuery(stockOrderId), CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(stockOrderId));
            Assert.IsEmpty(result.StockBookChanges);
        }
    }
}