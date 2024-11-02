using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.StockBookOrderFeature.Models;
using ShopApi.Services;

namespace ShopApi.Features.StockBookOrderFeature.Services.Tests
{
    [TestFixture]
    public class BookStockAmountUpdatedEventHandlerTests
    {
        private Mock<ILibraryService> mockLibraryService;
        private BookStockAmountUpdatedEventHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockLibraryService = new Mock<ILibraryService>();
            handler = new BookStockAmountUpdatedEventHandler(mockLibraryService.Object);
        }

        [Test]
        public async Task HandleAsync_ValidEvent_CallsUpdateBookStockAmountAsync()
        {
            // Arrange
            var stockChanges = new List<StockBookChange>
            {
                new StockBookChange { BookId = 1, ChangeAmount = 5 },
                new StockBookChange { BookId = 2, ChangeAmount = -3 }
            };
            var stockBookOrder = new StockBookOrder
            {
                StockBookChanges = stockChanges
            };
            var bookStockAmountUpdatedEvent = new BookStockAmountUpdatedEvent(stockBookOrder);
            var cancellationToken = CancellationToken.None;
            // Act
            await handler.HandleAsync(bookStockAmountUpdatedEvent, cancellationToken);
            // Assert
            mockLibraryService.Verify(service => service.UpdateBookStockAmountAsync(
                It.Is<List<StockBookChange>>(changes => changes == stockChanges),
                cancellationToken), Times.Once);
        }
    }
}