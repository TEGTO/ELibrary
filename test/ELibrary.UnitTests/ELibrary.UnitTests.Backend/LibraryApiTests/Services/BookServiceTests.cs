using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Repositories.Library;
using Moq;

namespace LibraryApi.Services.Tests
{
    [TestFixture]
    internal class BookServiceTests
    {
        private Mock<IBookRepository> repositoryMock;
        private BookService bookService;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IBookRepository>();
            bookService = new BookService(repositoryMock.Object);
            cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task RaisePopularityAsync_ValidIds_CallsGetAndUpdatePopularities()
        {
            // Arrange
            var bookIds = new List<int> { 1, 2, 3 };
            var existingPopularities = new List<BookPopularity>
            {
                new BookPopularity { BookId = 1, Popularity = 5 },
                new BookPopularity { BookId = 2, Popularity = 2 }
            };
            repositoryMock
                .Setup(repo => repo.GetPopularitiesByIdsAsync(bookIds, cancellationToken))
                .ReturnsAsync(existingPopularities);
            // Act
            await bookService.RaisePopularityAsync(bookIds, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.GetPopularitiesByIdsAsync(bookIds, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.UpdatePopularityRangeAsync(It.Is<List<BookPopularity>>(p =>
                p.Any(x => x.BookId == 1 && x.Popularity == 6) &&
                p.Any(x => x.BookId == 2 && x.Popularity == 3) &&
                p.Any(x => x.BookId == 3 && x.Popularity == 1)
            ), cancellationToken), Times.Once);
        }
        [Test]
        public async Task RaisePopularityAsync_EmptyIds_DoesNotCallGetOrUpdatePopularities()
        {
            // Arrange
            var emptyBookIds = new List<int>();
            // Act
            await bookService.RaisePopularityAsync(emptyBookIds, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.GetPopularitiesByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()), Times.Never);
            repositoryMock.Verify(repo => repo.UpdatePopularityRangeAsync(It.IsAny<List<BookPopularity>>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public async Task ChangeBookStockAmount_ValidChangeRequests_CallsGetAndUpdateBooks()
        {
            // Arrange
            var changeRequests = new Dictionary<int, int> { { 1, 10 }, { 2, -5 } };
            var books = new List<Book>
            {
                new Book { Id = 1, StockAmount = 5 },
                new Book { Id = 2, StockAmount = 15 }
            };
            repositoryMock
                .Setup(repo => repo.GetByIdsAsync(It.Is<List<int>>(ids => ids.SequenceEqual(changeRequests.Keys)), cancellationToken))
                .ReturnsAsync(books);
            // Act
            await bookService.ChangeBookStockAmount(changeRequests, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.GetByIdsAsync(It.Is<List<int>>(ids => ids.SequenceEqual(changeRequests.Keys)), cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.UpdateRangeAsync(It.Is<Book[]>(b =>
                b.Any(x => x.Id == 1 && x.StockAmount == 15) &&
                b.Any(x => x.Id == 2 && x.StockAmount == 10)
            ), cancellationToken), Times.Once);
        }
        [Test]
        public async Task ChangeBookStockAmount_EmptyChangeRequests_DoesNotCallGetOrUpdateBooks()
        {
            // Arrange
            var emptyChangeRequests = new Dictionary<int, int>();
            // Act
            await bookService.ChangeBookStockAmount(emptyChangeRequests, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.GetByIdsAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()), Times.Never);
            repositoryMock.Verify(repo => repo.UpdateRangeAsync(It.IsAny<Book[]>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}