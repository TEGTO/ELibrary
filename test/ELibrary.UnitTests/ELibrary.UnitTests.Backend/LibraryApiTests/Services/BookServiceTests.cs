using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Services;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;

namespace LibraryApiTests.Services
{
    [TestFixture]
    internal class BookServiceTests
    {
        private MockRepository mockRepository;
        private Mock<IDatabaseRepository<LibraryDbContext>> repositoryMock;
        private CancellationToken cancellationToken;
        private BookService service;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            repositoryMock = new Mock<IDatabaseRepository<LibraryDbContext>>();
            service = new BookService(repositoryMock.Object);
            cancellationToken = new CancellationToken();
        }
        private Mock<LibraryDbContext> CreateMockDbContext()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var mockDbContext = mockRepository.Create<LibraryDbContext>(options);
            return mockDbContext;
        }
        private static Mock<DbSet<T>> GetDbSetMock<T>(IQueryable<T> data) where T : class
        {
            return data.BuildMockDbSet();
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsBookWithAuthorAndGenre()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Name = "Book1",
                Author = new Author { Id = 1, Name = "Author1" },
                Genre = new Genre { Id = 1, Name = "Genre1" }
            };
            var books = new List<Book> { book }.AsQueryable();
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(books);
            dbContextMock.Setup(db => db.Set<Book>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            var result = await service.GetByIdAsync(1, cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result!.Id, Is.EqualTo(book.Id));
            Assert.That(result.Author!.Name, Is.EqualTo(book.Author.Name));
            Assert.That(result.Genre!.Name, Is.EqualTo(book.Genre.Name));
        }
        [Test]
        public async Task GetPaginatedAsync_ValidPage_ReturnsBooksWithAuthorsAndGenres()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Name = "Book1", Author = new Author { Id = 1, Name = "Author1" }, Genre = new Genre { Id = 1, Name = "Genre1" } },
                new Book { Id = 2, Name = "Book2", Author = new Author { Id = 2, Name = "Author2" }, Genre = new Genre { Id = 2, Name = "Genre2" } }
            }.AsQueryable();
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(books);
            dbContextMock.Setup(db => db.Set<Book>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            var result = await service.GetPaginatedAsync(1, 10, cancellationToken);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Author!.Name, Is.EqualTo("Author2"));
            Assert.That(result.First().Genre!.Name, Is.EqualTo("Genre2"));
        }
        [Test]
        public async Task CreateAsync_ValidBook_AddsBookWithAuthorAndGenre()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Name = "Book1",
                AuthorId = 1,
                GenreId = 1
            };
            var authors = new List<Author> { new Author { Id = 1, Name = "Author1" } }.AsQueryable();
            var genres = new List<Genre> { new Genre { Id = 1, Name = "Genre1" } }.AsQueryable();
            var dbContextMock = CreateMockDbContext();
            var bookDbSetMock = GetDbSetMock(new List<Book>().AsQueryable());
            var authorDbSetMock = GetDbSetMock(authors);
            var genreDbSetMock = GetDbSetMock(genres);
            dbContextMock.Setup(db => db.Set<Book>()).Returns(bookDbSetMock.Object);
            dbContextMock.Setup(db => db.Authors).Returns(authorDbSetMock.Object);
            dbContextMock.Setup(db => db.Genres).Returns(genreDbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            var result = await service.CreateAsync(book, cancellationToken);
            // Assert
            bookDbSetMock.Verify(d => d.AddAsync(book, It.IsAny<CancellationToken>()), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(result.AuthorId, Is.EqualTo(book.AuthorId));
            Assert.That(result.GenreId, Is.EqualTo(book.GenreId));
        }
        [Test]
        public async Task UpdateAsync_ValidBook_UpdatesBookWithAuthorAndGenre()
        {
            // Arrange
            var bookInDb = new Book
            {
                Id = 1,
                Name = "Book1",
                AuthorId = 1,
                GenreId = 1,
                Author = new Author { Id = 1, Name = "Author1" },
                Genre = new Genre { Id = 1, Name = "Genre1" }
            };
            var updatedBook = new Book
            {
                Id = 1,
                Name = "UpdatedBook1",
                AuthorId = 2,
                GenreId = 2
            };
            var authors = new List<Author> { new Author { Id = 2, Name = "Author2" } }.AsQueryable();
            var genres = new List<Genre> { new Genre { Id = 2, Name = "Genre2" } }.AsQueryable();
            var dbContextMock = CreateMockDbContext();
            var bookDbSetMock = GetDbSetMock(new List<Book> { bookInDb }.AsQueryable());
            var authorDbSetMock = GetDbSetMock(authors);
            var genreDbSetMock = GetDbSetMock(genres);
            dbContextMock.Setup(db => db.Set<Book>()).Returns(bookDbSetMock.Object);
            dbContextMock.Setup(db => db.Set<Author>()).Returns(authorDbSetMock.Object);
            dbContextMock.Setup(db => db.Set<Genre>()).Returns(genreDbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            await service.UpdateAsync(updatedBook, cancellationToken);
            // Assert
            bookDbSetMock.Verify(d => d.Update(It.IsAny<Book>()), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(bookInDb.Name, Is.EqualTo("UpdatedBook1"));
            Assert.That(bookInDb.AuthorId, Is.EqualTo(2));
            Assert.That(bookInDb.GenreId, Is.EqualTo(2));
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
                Genre = new Genre { Id = 1, Name = "Genre1" }
            };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(new List<Book> { book }.AsQueryable());
            dbContextMock.Setup(db => db.Set<Book>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            await service.DeleteByIdAsync(1, cancellationToken);
            // Assert
            dbSetMock.Verify(d => d.Remove(book), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}