using LibraryApi.Domain.Dtos;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;

namespace LibraryApi.Services.Tests
{
    [TestFixture]
    internal class BookServiceTests
    {
        private Mock<IDatabaseRepository<LibraryShopDbContext>> repositoryMock;
        private CancellationToken cancellationToken;
        private BookService service;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<LibraryShopDbContext>>();
            service = new BookService(repositoryMock.Object);
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
            var result = await service.GetByIdAsync(1, cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result!.Id, Is.EqualTo(book.Id));
            Assert.That(result.Author!.Name, Is.EqualTo(book.Author.Name));
            Assert.That(result.Genre!.Name, Is.EqualTo(book.Genre.Name));
            Assert.That(result.Publisher!.Name, Is.EqualTo(book.Publisher.Name));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
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
            var dbSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            var paginationRequest = new BookFilterRequest() { PageNumber = 1, PageSize = 10 };
            // Act
            var result = await service.GetPaginatedAsync(paginationRequest, cancellationToken);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Author!.Name, Is.EqualTo("Author2"));
            Assert.That(result.First().Genre!.Name, Is.EqualTo("Genre2"));
            Assert.That(result.First().Publisher!.Name, Is.EqualTo("Publisher2"));
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
            var result = await service.CreateAsync(book, cancellationToken);
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
            var bookInDb = new Book
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
            var books = new List<Book> { bookInDb };
            var dbSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);
            repositoryMock.Setup(repo => repo.UpdateAsync(bookInDb, cancellationToken))
                .ReturnsAsync(bookInDb);
            // Act
            await service.UpdateAsync(updatedBook, cancellationToken);
            // Assert
            Assert.That(bookInDb.Name, Is.EqualTo("UpdatedBook1"));
            Assert.That(bookInDb.AuthorId, Is.EqualTo(1));
            Assert.That(bookInDb.GenreId, Is.EqualTo(2));
            Assert.That(bookInDb.PublisherId, Is.EqualTo(3));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.UpdateAsync(bookInDb, cancellationToken), Times.Once);
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
            var books = new List<Book> { book };
            var dbSetMock = GetDbSetMock(books);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);
            repositoryMock.Setup(repo => repo.DeleteAsync(book, cancellationToken))
                .Returns(Task.CompletedTask);
            // Act
            await service.DeleteByIdAsync(1, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteAsync(book, cancellationToken), Times.Once);
        }
    }
}