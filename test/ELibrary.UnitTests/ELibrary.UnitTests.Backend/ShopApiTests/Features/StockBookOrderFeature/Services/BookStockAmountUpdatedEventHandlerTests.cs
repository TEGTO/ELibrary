using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;
using ShopApi.Features.StockBookOrderFeature.Models;

namespace ShopApi.Features.StockBookOrderFeature.Services.Tests
{
    [TestFixture]
    public class BookStockAmountUpdatedEventHandlerTests
    {
        private Mock<IDatabaseRepository<ShopDbContext>> repositoryMock;
        private BookStockAmountUpdatedEventHandler handler;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<ShopDbContext>>();
            handler = new BookStockAmountUpdatedEventHandler(repositoryMock.Object);
            cancellationToken = CancellationToken.None;
        }

        [Test]
        public async Task HandleAsync_WhenBookExists_UpdatesStockAmount()
        {
            // Arrange
            var book = new Book { Id = 1, StockAmount = 5 };
            var stockChange = new StockBookChange { BookId = 1, ChangeAmount = 3 };
            var stockBookOrder = new StockBookOrder { StockBookChanges = new List<StockBookChange> { stockChange } };
            var bookEvent = new BookStockAmountUpdatedEvent(stockBookOrder);
            var books = new List<Book> { book }.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<Book>(It.IsAny<CancellationToken>())).ReturnsAsync(books);
            // Act
            await handler.HandleAsync(bookEvent, cancellationToken);
            // Assert
            Assert.That(book.StockAmount, Is.EqualTo(8));
            repositoryMock.Verify(r => r.UpdateAsync(book, cancellationToken), Times.Once);
        }

        [Test]
        public async Task HandleAsync_WhenStockAmountBecomesNegative_UpdatesStockAmount()
        {
            // Arrange
            var book = new Book { Id = 1, StockAmount = 2 };
            var stockChange = new StockBookChange { BookId = 1, ChangeAmount = -5 };
            var stockBookOrder = new StockBookOrder { StockBookChanges = new List<StockBookChange> { stockChange } };
            var bookEvent = new BookStockAmountUpdatedEvent(stockBookOrder);
            var books = new List<Book> { book }.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<Book>(It.IsAny<CancellationToken>())).ReturnsAsync(books);
            // Act
            await handler.HandleAsync(bookEvent, cancellationToken);
            // Assert
            Assert.That(book.StockAmount, Is.EqualTo(-3));
            repositoryMock.Verify(r => r.UpdateAsync(book, cancellationToken), Times.Once);
        }
        [Test]
        public async Task HandleAsync_WhenMultipleBooks_UpdatesStockAmountForEach()
        {
            // Arrange
            var book1 = new Book { Id = 1, StockAmount = 10 };
            var book2 = new Book { Id = 2, StockAmount = 4 };
            var stockChange1 = new StockBookChange { BookId = 1, ChangeAmount = 3 };
            var stockChange2 = new StockBookChange { BookId = 2, ChangeAmount = -2 };
            var stockBookOrder = new StockBookOrder
            {
                StockBookChanges = new List<StockBookChange> { stockChange1, stockChange2 }
            };
            var bookEvent = new BookStockAmountUpdatedEvent(stockBookOrder);
            var books = new List<Book> { book1, book2 }.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<Book>(It.IsAny<CancellationToken>())).ReturnsAsync(books);
            // Act
            await handler.HandleAsync(bookEvent, cancellationToken);
            // Assert
            Assert.That(book1.StockAmount, Is.EqualTo(13));
            Assert.That(book2.StockAmount, Is.EqualTo(2));
            repositoryMock.Verify(r => r.UpdateAsync(book1, cancellationToken), Times.Once);
            repositoryMock.Verify(r => r.UpdateAsync(book2, cancellationToken), Times.Once);
        }
    }
}