using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;

namespace LibraryShopEntities.Repositories.Library.Tests
{
    [TestFixture]
    internal class BookRepositoryTests
    {
        private Mock<IDatabaseRepository<LibraryDbContext>> repositoryMock;
        private CancellationToken cancellationToken;
        private BookRepository repository;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<LibraryDbContext>>();

            repository = new BookRepository(repositoryMock.Object);
            cancellationToken = new CancellationToken();
        }

        private static Mock<DbSet<T>> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMockDbSet();
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsBookWithEntities()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Name = "Book1",
                Author = new Author { Id = 1, Name = "Author1" },
                Genre = new Genre { Id = 1, Name = "Genre1" },
                Publisher = new Publisher { Id = 1, Name = "Publisher1" }
            };
            var books = new List<Book> { book };
            var dbSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            // Act
            var result = await repository.GetByIdAsync(1, cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result!.Id, Is.EqualTo(book.Id));
            Assert.That(result.Author!.Name, Is.EqualTo(book.Author.Name));
            Assert.That(result.Genre!.Name, Is.EqualTo(book.Genre.Name));
            Assert.That(result.Publisher!.Name, Is.EqualTo(book.Publisher.Name));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var books = new List<Book>();
            var dbSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            // Act
            var result = await repository.GetByIdAsync(99, cancellationToken);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetByIds_ValidIds_ReturnsEntities()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Name =  "Book1",
                    Author = new Author { Id = 1, Name = "Author1" },
                    Genre = new Genre { Id = 1, Name = "Genre1" },
                    Publisher = new Publisher { Id = 1, Name = "Publisher1" },
                    PublicationDate = DateTime.UtcNow,
                    Price = 10,
                    PageAmount = 10,
                    StockAmount = 10,
                },
                new Book
                {
                    Id = 2,
                    Name = "Book2",
                    Author = new Author { Id = 2, Name = "Author2" },
                    Genre = new Genre { Id = 2, Name = "Genre2" },
                    Publisher = new Publisher { Id = 1, Name = "Publisher2" },
                    PublicationDate = DateTime.UtcNow,
                    Price = 10,
                    PageAmount = 10,
                    StockAmount = 10,
                }
            };
            var orders = new List<Order>
            {
                new Order
                {
                    OrderBooks= new List<OrderBook>
                    {
                        new OrderBook
                        {
                            Id = "1",
                            BookId = 1
                        },
                    }
                }
            };
            var dbBookSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
               .ReturnsAsync(dbBookSetMock.Object);
            var dbOrderSetMock = GetDbSetMock(orders);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Order>(cancellationToken))
                .ReturnsAsync(dbOrderSetMock.Object);
            var ids = new List<int> { 1, 2, 3 };
            // Act
            var result = await repository.GetByIdsAsync(ids, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Author!.Name, Is.EqualTo("Author1"));
            Assert.That(result.First().Genre!.Name, Is.EqualTo("Genre1"));
            Assert.That(result.First().Publisher!.Name, Is.EqualTo("Publisher1"));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetPopularitiesByIdsAsync_ExistingIds_ReturnsMatchingPopularities()
        {
            // Arrange
            var popularities = new List<BookPopularity>
            {
                new BookPopularity { BookId = 1, Popularity = 10 },
                new BookPopularity { BookId = 2, Popularity = 5 }
            };
            var dbSetMock = GetDbSetMock(popularities);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<BookPopularity>(cancellationToken)).ReturnsAsync(dbSetMock.Object);
            var ids = new List<int> { 1, 2 };
            // Act
            var result = await repository.GetPopularitiesByIdsAsync(ids, cancellationToken);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First(bp => bp.BookId == 1).Popularity, Is.EqualTo(10));
            Assert.That(result.First(bp => bp.BookId == 2).Popularity, Is.EqualTo(5));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<BookPopularity>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetPopularitiesByIdsAsync_NoMatchingIds_ReturnsEmptyList()
        {
            // Arrange
            var popularities = new List<BookPopularity>
            {
                new BookPopularity { BookId = 1, Popularity = 10 },
                new BookPopularity { BookId = 2, Popularity = 5 }
            };
            var dbSetMock = GetDbSetMock(popularities);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<BookPopularity>(cancellationToken)).ReturnsAsync(dbSetMock.Object);
            var ids = new List<int> { 99, 100 };
            // Act
            var result = await repository.GetPopularitiesByIdsAsync(ids, cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            repositoryMock.Verify(repo => repo.GetQueryableAsync<BookPopularity>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task UpdatePopularityRangeAsync_ValidPopularities_UpdatesEntities()
        {
            // Arrange
            var popularities = new List<BookPopularity>
            {
                new BookPopularity { BookId = 1, Popularity = 15 },
                new BookPopularity { BookId = 2, Popularity = 20 }
            };
            // Act
            await repository.UpdatePopularityRangeAsync(popularities, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.UpdateRangeAsync(It.Is<IEnumerable<BookPopularity>>(p =>
                p.SequenceEqual(popularities)), cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetPaginatedAsync_ValidPage_ReturnsBooksWithEntities()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Name =  "Book1",
                    Author = new Author { Id = 1, Name = "Author1" },
                    Genre = new Genre { Id = 1, Name = "Genre1" },
                    Publisher = new Publisher { Id = 1, Name = "Publisher1" },
                    PublicationDate = DateTime.UtcNow,
                    Price = 10,
                    PageAmount = 10,
                    StockAmount = 10,
                },
                new Book
                {
                    Id = 2,
                    Name = "Book2",
                    Author = new Author { Id = 2, Name = "Author2" },
                    Genre = new Genre { Id = 2, Name = "Genre2" },
                    Publisher = new Publisher { Id = 1, Name = "Publisher2" },
                    PublicationDate = DateTime.UtcNow,
                    Price = 10,
                    PageAmount = 10,
                    StockAmount = 10,
                }
            };
            var orders = new List<Order>
            {
                new Order
                {
                    OrderBooks= new List<OrderBook>
                    {
                        new OrderBook
                        {
                            Id = "1",
                            BookId = 1
                        },
                    }
                }
            };
            var dbBookSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
               .ReturnsAsync(dbBookSetMock.Object);
            var dbOrderSetMock = GetDbSetMock(orders);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Order>(cancellationToken))
                .ReturnsAsync(dbOrderSetMock.Object);
            var paginationRequest = new BookFilterRequest() { PageNumber = 1, PageSize = 10 };
            // Act
            var result = await repository.GetPaginatedAsync(paginationRequest, cancellationToken);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Author!.Name, Is.EqualTo("Author1"));
            Assert.That(result.First().Genre!.Name, Is.EqualTo("Genre1"));
            Assert.That(result.First().Publisher!.Name, Is.EqualTo("Publisher1"));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetItemTotalAmountAsync_ReturnsCorrectCount()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Name = "Book1" },
                new Book { Id = 2, Name = "Book2" }
            };
            var dbSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            var filterRequest = new BookFilterRequest();
            // Act
            var result = await repository.GetItemTotalAmountAsync(filterRequest, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(2));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task CreateAsync_ValidBook_AddsBookWithEntities()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Name = "Book1",
                AuthorId = 1,
                GenreId = 2,
                PublisherId = 3,
            };
            var books = new List<Book> { book };
            var dbSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            repositoryMock.Setup(repo => repo.AddAsync(book, cancellationToken)).ReturnsAsync(book);
            // Act
            var result = await repository.CreateAsync(book, cancellationToken);
            // Assert
            Assert.That(result.AuthorId, Is.EqualTo(book.AuthorId));
            Assert.That(result.GenreId, Is.EqualTo(book.GenreId));
            Assert.That(result.PublisherId, Is.EqualTo(book.PublisherId));
            repositoryMock.Verify(d => d.AddAsync(book, It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task UpdateAsync_ValidBook_UpdatesBookWithEntities()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Name = "Book1",
                AuthorId = 1,
                GenreId = 1,
                Author = new Author { Id = 1, Name = "Author1" },
                Genre = new Genre { Id = 1, Name = "Genre1" },
                Publisher = new Publisher { Id = 1, Name = "Publisher1" },
            };
            var updatedBook = new Book
            {
                Id = 1,
                Name = "UpdatedBook1",
                AuthorId = 1,
                GenreId = 2,
                PublisherId = 3,
            };
            var dbSetMock = GetDbSetMock([updatedBook]);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);
            repositoryMock.Setup(repo => repo.UpdateAsync(book, cancellationToken))
                .ReturnsAsync(updatedBook);
            // Act
            var result = await repository.UpdateAsync(book, cancellationToken);
            // Assert
            Assert.That(result.Name, Is.EqualTo("UpdatedBook1"));
            Assert.That(result.AuthorId, Is.EqualTo(1));
            Assert.That(result.GenreId, Is.EqualTo(2));
            Assert.That(result.PublisherId, Is.EqualTo(3));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.UpdateAsync(book, cancellationToken), Times.Once);
        }
        [Test]
        public async Task DeleteByIdAsync_ValidId_DeletesBook()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Name = "Book1",
                Author = new Author { Id = 1, Name = "Author1" },
                Genre = new Genre { Id = 1, Name = "Genre1" },
                Publisher = new Publisher { Id = 1, Name = "Publisher1" },
            };
            // Act
            await repository.DeleteAsync(book, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync(book, cancellationToken), Times.Once);
        }
    }
}