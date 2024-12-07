using DatabaseControl.Repositories;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

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
        [TestCase(1, "Book1", true, Description = "Book exists.")]
        [TestCase(99, null, false, Description = "Book does not exist.")]
        public async Task GetByIdAsync_TestCases(int id, string? expectedName, bool shouldExist)
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

            // Act
            var result = await repository.GetByIdAsync(id, cancellationToken);

            // Assert
            if (shouldExist)
            {
                Assert.IsNotNull(result);
                Assert.That(result!.Name, Is.EqualTo(expectedName));
            }
            else
            {
                Assert.IsNull(result);
            }

            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(new[] { 1, 2 }, 2, Description = "Finds two books by their ids.")]
        [TestCase(new[] { 1 }, 1, Description = "Finds one book by it id.")]
        [TestCase(new[] { 99, 100 }, 0, Description = "Finds zero books.")]
        public async Task GetByIdsAsync_TestCases(int[] ids, int expectedCount)
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

            // Act
            var result = await repository.GetByIdsAsync(ids, cancellationToken);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedCount));

            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(new[] { 1, 2 }, 2, new[] { 10, 5 }, Description = "Returns matching popularities for existing IDs.")]
        [TestCase(new[] { 99, 100 }, 0, new int[0], Description = "Returns an empty list for non-matching IDs.")]
        public async Task GetPopularitiesByIdsAsync_TestCases(int[] ids, int expectedCount, int[] expectedPopularities)
        {
            // Arrange
            var popularities = new List<BookPopularity>
            {
                new BookPopularity { BookId = 1, Popularity = 10 },
                new BookPopularity { BookId = 2, Popularity = 5 }
            };
            var dbSetMock = GetDbSetMock(popularities);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<BookPopularity>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);

            // Act
            var result = await repository.GetPopularitiesByIdsAsync(ids.ToList(), cancellationToken);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedCount));

            foreach (var id in ids)
            {
                var expectedPopularity = Array.Find(expectedPopularities, pop => popularities.Exists(p => p.BookId == id && p.Popularity == pop));
                if (expectedPopularity > 0)
                {
                    Assert.That(result.First(bp => bp.BookId == id).Popularity, Is.EqualTo(expectedPopularity));
                }
            }

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
        [TestCaseSource(nameof(BookPaginationFilterTestCases))]
        public async Task GetPaginatedAsync_TestCases(int expectedCount, string? firstBookName, LibraryFilterRequest filter)
        {
            // Arrange
            var books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Name = "Book1",
                    PublicationDate = DateTime.MinValue,
                    Price = 100,
                    CoverType = CoverType.Soft,
                    StockAmount = 0,
                    PageAmount = 100,
                    AuthorId = 1,
                    GenreId = 1,
                    PublisherId = 1,
                    BookPopularity = new BookPopularity() { Popularity = 1 },
                },
                new Book
                {
                    Id = 2,
                    Name = "Book2",
                    PublicationDate = DateTime.MaxValue,
                    Price = 1000,
                    CoverType = CoverType.Hard,
                    StockAmount = 100,
                    PageAmount = 1000,
                    AuthorId = 2,
                    GenreId = 2,
                    PublisherId = 2,
                    BookPopularity = new BookPopularity() { Popularity = 100 },
                },
                new Book
                {
                    Id = 3,
                    Name = "Book3",
                    PublicationDate = DateTime.MaxValue,
                    Price = 1000,
                    CoverType = CoverType.Hard,
                    StockAmount = 100,
                    PageAmount = 1000,
                    AuthorId = 3,
                    GenreId = 3,
                    PublisherId = 3,
                    BookPopularity = new BookPopularity() { Popularity = 1 },
                },
            };
            var dbSetMock = GetDbSetMock(books);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);

            // Act
            var result = await repository.GetPaginatedAsync(filter, cancellationToken);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedCount));
            if (expectedCount > 0)
            {
                Assert.That(result.First().Name, Is.EqualTo(firstBookName));
            }

            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCaseSource(nameof(BookPaginationAmountFilterTestCases))]
        public async Task GetItemTotalAmountAsync_TestCases(int expectedCount, LibraryFilterRequest filter)
        {
            // Arrange
            var books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Name = "Book1",
                    PublicationDate = DateTime.MinValue,
                    Price = 100,
                    CoverType = CoverType.Soft,
                    StockAmount = 0,
                    PageAmount = 100,
                    AuthorId = 1,
                    GenreId = 1,
                    PublisherId = 1,
                },
                new Book
                {
                    Id = 2,
                    Name = "Book2",
                    PublicationDate = DateTime.MaxValue,
                    Price = 1000,
                    CoverType = CoverType.Hard,
                    StockAmount = 100,
                    PageAmount = 1000,
                    AuthorId = 2,
                    GenreId = 2,
                    PublisherId = 2,
                },
            };

            var dbSetMock = GetDbSetMock(books);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);

            // Act
            var result = await repository.GetItemTotalAmountAsync(filter, cancellationToken);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCount));

            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(2, "Book1", Description = "Adds valid book and verifies it can be retrieved by ID.")]
        public async Task CreateAsync_TestCases(int id, string name)
        {
            // Arrange
            var book = new Book { Id = id, Name = name };
            var dbSetMock = GetDbSetMock(new List<Book> { book });

            repositoryMock.Setup(repo => repo.AddAsync(book, cancellationToken))
                .ReturnsAsync(book);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);

            // Act
            var result = await repository.CreateAsync(book, cancellationToken);

            // Assert
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo(name));

            repositoryMock.Verify(repo => repo.AddAsync(book, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }
        [Test]
        [TestCase(2, "Book2", Description = "Throws exception when created book cannot be retrieved by ID.")]
        public void CreateAsync_ThrowsException_WhenGetByIdFails(int id, string name)
        {
            // Arrange
            var book = new Book { Id = id, Name = name };

            repositoryMock.Setup(repo => repo.AddAsync(book, cancellationToken))
                .ReturnsAsync(book);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
                .ReturnsAsync(GetDbSetMock(new List<Book>()).Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await repository.CreateAsync(book, cancellationToken));

            Assert.That(exception.Message, Is.EqualTo("Can not get created book!"));
            repositoryMock.Verify(repo => repo.AddAsync(book, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(1, "UpdatedBook1", Description = "Updates valid book and verifies it can be retrieved by ID.")]
        public async Task UpdateAsync_TestCases(int id, string updatedName)
        {
            // Arrange
            var updatedBook = new Book { Id = id, Name = updatedName };
            var dbSetMock = GetDbSetMock(new List<Book> { updatedBook });

            repositoryMock.Setup(repo => repo.UpdateAsync(updatedBook, cancellationToken))
                .ReturnsAsync(updatedBook);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);

            // Act
            var result = await repository.UpdateAsync(updatedBook, cancellationToken);

            // Assert
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo(updatedName));

            repositoryMock.Verify(repo => repo.UpdateAsync(updatedBook, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }
        [Test]
        [TestCase(1, "UpdatedBook2", Description = "Throws exception when updated book cannot be retrieved by ID.")]
        public void UpdateAsync_ThrowsException_WhenGetByIdFails(int id, string updatedName)
        {
            // Arrange
            var updatedBook = new Book { Id = id, Name = updatedName };

            repositoryMock.Setup(repo => repo.UpdateAsync(updatedBook, cancellationToken))
                .ReturnsAsync(updatedBook);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<Book>(cancellationToken))
                .ReturnsAsync(GetDbSetMock(new List<Book>()).Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await repository.UpdateAsync(updatedBook, cancellationToken));

            Assert.That(exception.Message, Is.EqualTo("Can not get updated book!"));
            repositoryMock.Verify(repo => repo.UpdateAsync(updatedBook, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Book>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(2, Description = "Valid list with two books")]
        [TestCase(0, Description = "Empty list")]
        public async Task UpdateRangeAsync_TestCases(int bookCount)
        {
            // Arrange
            var booksToUpdate = Enumerable.Range(1, bookCount)
                .Select(i => new Book
                {
                    Id = i,
                    Name = $"UpdatedBook{i}",
                })
                .ToList();

            // Act
            await repository.UpdateRangeAsync(booksToUpdate, cancellationToken);

            // Assert
            if (bookCount > 0)
            {
                repositoryMock.Verify(repo => repo.UpdateRangeAsync(booksToUpdate, cancellationToken), Times.Once);
            }
            else
            {
                repositoryMock.Verify(repo => repo.UpdateRangeAsync(It.IsAny<IEnumerable<Book>>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [Test]
        [TestCase(1, "BookToDelete", Description = "Deletes valid book by calling generic repository.")]
        public async Task DeleteAsync_TestCases(int id, string name)
        {
            // Arrange
            var book = new Book { Id = id, Name = name };

            // Act
            await repository.DeleteAsync(book, cancellationToken);

            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync(book, cancellationToken), Times.Once);
        }

        public static object[] BookPaginationFilterTestCases()
        {
            return [
                new object[] { 3, "Book3", new LibraryFilterRequest() { PageNumber = 1, PageSize = 10, } },
                new object[] { 0, null!, new LibraryFilterRequest() { PageNumber = 2, PageSize = 10, } },
                new object[] { 3, "Book3", new LibraryFilterRequest() { PageNumber = 1, PageSize = 10, ContainsName = "Book" } },
                new object[] { 1, "Book1", new LibraryFilterRequest() { PageNumber = 1, PageSize = 10, ContainsName = "Book1" } },
                new object[] { 3, "Book2", new BookFilterRequest() { PageNumber = 1, PageSize = 10, } },
                new object[] { 0, null!, new BookFilterRequest() { PageNumber = 2, PageSize = 10, } },
                new object[] { 3, "Book2", new BookFilterRequest() { PageNumber = 1, PageSize = 10, ContainsName = "Book" } },
                new object[] { 1, "Book1", new BookFilterRequest() { PageNumber = 1, PageSize = 10, ContainsName = "Book1" } },
                new object[] { 3, "Book2", new BookFilterRequest() { PageNumber = 1, PageSize = 10, Sorting = BookSorting.MostPopular } },
                new object[] { 3, "Book3", new BookFilterRequest() { PageNumber = 1, PageSize = 10, Sorting = BookSorting.LeastPopular } },
                new object[] { 2, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, PublicationFrom = DateTime.UtcNow  } },
                new object[] { 1, "Book1", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, PublicationTo = DateTime.UtcNow  } },
                new object[] { 3, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, PublicationFrom = DateTime.MinValue, PublicationTo = DateTime.MaxValue  } },
                new object[] { 0, null!, new BookFilterRequest() {  PageNumber = 1, PageSize = 10, PublicationFrom = DateTime.UtcNow, PublicationTo = DateTime.UtcNow.AddDays(1) } },
                new object[] { 2, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, MinPrice = 101  } },
                new object[] { 1, "Book1", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, MaxPrice = 101  } },
                new object[] { 0, null!, new BookFilterRequest() {  PageNumber = 1, PageSize = 10, MinPrice = 101, MaxPrice = 102  } },
                new object[] { 2, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, CoverType = CoverType.Hard  } },
                new object[] { 1, "Book1", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, CoverType = CoverType.Soft  } },
                new object[] { 3, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, CoverType = CoverType.Any  } },
                new object[] { 2, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, OnlyInStock = true } },
                new object[] { 3, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, OnlyInStock = false } },
                new object[] { 2, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, MinPageAmount = 101  } },
                new object[] { 1, "Book1", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, MaxPageAmount = 101  } },
                new object[] { 0, null!, new BookFilterRequest() {  PageNumber = 1, PageSize = 10, MinPageAmount = 101, MaxPageAmount = 102  } },
                new object[] { 1, "Book1", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, AuthorId = 1, } },
                new object[] { 1, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, AuthorId = 2, } },
                new object[] { 0, null!, new BookFilterRequest() {  PageNumber = 1, PageSize = 10, AuthorId = 0, } },
                new object[] { 1, "Book1", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, GenreId = 1, } },
                new object[] { 1, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, GenreId = 2, } },
                new object[] { 0, null!, new BookFilterRequest() {  PageNumber = 1, PageSize = 10, GenreId = 0, } },
                new object[] { 1, "Book1", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, PublisherId = 1, } },
                new object[] { 1, "Book2", new BookFilterRequest() {  PageNumber = 1, PageSize = 10, PublisherId = 2, } },
                new object[] { 0, null!, new BookFilterRequest() {  PageNumber = 1, PageSize = 10, PublisherId = 0, } },
            ];
        }
        public static object[] BookPaginationAmountFilterTestCases()
        {
            return [
                new object[] { 2, new LibraryFilterRequest() },
                new object[] { 2, new LibraryFilterRequest() { ContainsName = "Book" } },
                new object[] { 1, new LibraryFilterRequest() { ContainsName = "Book2" } },
                new object[] { 2, new BookFilterRequest() },
                new object[] { 2, new BookFilterRequest() { ContainsName = "Book" } },
                new object[] { 1, new BookFilterRequest() { ContainsName = "Book2" } },
                new object[] { 1, new BookFilterRequest() { PublicationFrom = DateTime.UtcNow  } },
                new object[] { 1, new BookFilterRequest() { PublicationTo = DateTime.UtcNow  } },
                new object[] { 2, new BookFilterRequest() { PublicationFrom = DateTime.MinValue, PublicationTo = DateTime.MaxValue  } },
                new object[] { 0, new BookFilterRequest() { PublicationFrom = DateTime.UtcNow, PublicationTo = DateTime.UtcNow.AddDays(1) } },
                new object[] { 1, new BookFilterRequest() { MinPrice = 101  } },
                new object[] { 1, new BookFilterRequest() { MaxPrice = 101  } },
                new object[] { 0, new BookFilterRequest() { MinPrice = 101, MaxPrice = 102  } },
                new object[] { 1, new BookFilterRequest() { CoverType = CoverType.Hard  } },
                new object[] { 1, new BookFilterRequest() { CoverType = CoverType.Soft  } },
                new object[] { 2, new BookFilterRequest() { CoverType = CoverType.Any  } },
                new object[] { 1, new BookFilterRequest() { OnlyInStock = true } },
                new object[] { 2, new BookFilterRequest() { OnlyInStock = false } },
                new object[] { 1, new BookFilterRequest() { MinPageAmount = 101  } },
                new object[] { 1, new BookFilterRequest() { MaxPageAmount = 101  } },
                new object[] { 0, new BookFilterRequest() { MinPageAmount = 101, MaxPageAmount = 102  } },
                new object[] { 1, new BookFilterRequest() { AuthorId = 1, } },
                new object[] { 1, new BookFilterRequest() { AuthorId = 2, } },
                new object[] { 0, new BookFilterRequest() { AuthorId = 0, } },
                new object[] { 1, new BookFilterRequest() { GenreId = 1, } },
                new object[] { 1, new BookFilterRequest() { GenreId = 2, } },
                new object[] { 0, new BookFilterRequest() { GenreId = 0, } },
                new object[] { 1, new BookFilterRequest() { PublisherId = 1, } },
                new object[] { 1, new BookFilterRequest() { PublisherId = 2, } },
                new object[] { 0, new BookFilterRequest() { PublisherId = 0, } },
            ];
        }
    }
}