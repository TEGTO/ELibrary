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
    internal class AuthorRepositoryTests
    {
        private Mock<IDatabaseRepository<LibraryDbContext>> repositoryMock;
        private CancellationToken cancellationToken;
        private AuthorRepository repository;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<LibraryDbContext>>();

            repository = new AuthorRepository(repositoryMock.Object);
            cancellationToken = new CancellationToken();
        }

        private static Mock<DbSet<T>> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMockDbSet();
        }

        [Test]
        [TestCase(1, "Author1", true, Description = "Author exists.")]
        [TestCase(99, null, false, Description = "Author does not exist.")]
        public async Task GetByIdAsync_TestCases(int id, string? expectedName, bool shouldExist)
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "Author1" },
                new Author { Id = 2, Name = "Author2" }
            };
            var dbSetMock = GetDbSetMock(authors);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<Author>(cancellationToken))
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

            repositoryMock.Verify(repo => repo.GetQueryableAsync<Author>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(new[] { 1, 2 }, 2, Description = "Finds two authors by their ids.")]
        [TestCase(new[] { 1 }, 1, Description = "Finds one author by his id.")]
        [TestCase(new[] { 99, 100 }, 0, Description = "Finds zero authors.")]
        public async Task GetByIdsAsync_TestCases(int[] ids, int expectedCount)
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "Author1" },
                new Author { Id = 2, Name = "Author2" }
            };
            var dbSetMock = GetDbSetMock(authors);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<Author>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);

            // Act
            var result = await repository.GetByIdsAsync(ids, cancellationToken);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedCount));

            repositoryMock.Verify(repo => repo.GetQueryableAsync<Author>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(1, 10, 2, "", "Author2", Description = "Pagination takes first page with 10 items, gets two authors.")]
        [TestCase(1, 10, 1, "Author1", "Author1", Description = "Pagination takes first page with 10 items, gets one author.")]
        [TestCase(1, 10, 1, "Doe", "Author1", Description = "Pagination takes first page with 10 items, gets one author.")]
        [TestCase(1, 10, 1, "Author1 Doe", "Author1", Description = "Pagination takes first page with 10 items, gets one author.")]
        [TestCase(2, 10, 0, "", null, Description = "Pagination takes second page with 10 items, gets zero authors.")]
        public async Task GetPaginatedAsync_TestCases(int pageNumber, int pageSize, int expectedCount, string containsName, string? firstAuthorName)
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "Author1", LastName = "Doe" },
                new Author { Id = 2, Name = "Author2" }
            };
            var dbSetMock = GetDbSetMock(authors);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<Author>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);

            var paginationRequest = new LibraryFilterRequest { PageNumber = pageNumber, PageSize = pageSize, ContainsName = containsName };

            // Act
            var result = await repository.GetPaginatedAsync(paginationRequest, cancellationToken);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedCount));
            if (expectedCount > 0)
            {
                Assert.That(result.First().Name, Is.EqualTo(firstAuthorName));
            }

            repositoryMock.Verify(repo => repo.GetQueryableAsync<Author>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(2, 2, "", Description = "Count all authors, returns count two.")]
        [TestCase(2, 1, "Author1", Description = "Count all authors, returns count one.")]
        [TestCase(2, 1, "Author1 Doe", Description = "Count all authors, returns count one.")]
        [TestCase(2, 2, "Doe", Description = "Count all authors, returns count two.")]
        [TestCase(0, 0, "", Description = "Count all authors, returns count zero.")]
        public async Task GetItemTotalAmountAsync_TestCases(int realCount, int expectedCount, string containsName)
        {
            // Arrange
            var authors = Enumerable.Range(1, realCount)
                .Select(i => new Author { Id = i, Name = $"Author{i}", LastName = "Doe" })
                .ToList();
            var dbSetMock = GetDbSetMock(authors);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<Author>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);

            var filterRequest = new LibraryFilterRequest() { ContainsName = containsName };

            // Act
            var result = await repository.GetItemTotalAmountAsync(filterRequest, cancellationToken);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCount));

            repositoryMock.Verify(repo => repo.GetQueryableAsync<Author>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(2, "Author1", Description = "Adds valid author by calling generic repository.")]
        public async Task CreateAsync_TestCases(int id, string name)
        {
            // Arrange
            var author = new Author { Id = id, Name = name };

            repositoryMock.Setup(repo => repo.AddAsync(author, cancellationToken))
                .ReturnsAsync(author);

            // Act
            var result = await repository.CreateAsync(author, cancellationToken);

            // Assert
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo(name));

            repositoryMock.Verify(repo => repo.AddAsync(author, cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(1, "UpdatedAuthor1", Description = "Updates valid author by calling generic repository.")]
        public async Task UpdateAsync_TestCases(int id, string updatedName)
        {
            // Arrange
            var updatedAuthor = new Author { Id = id, Name = updatedName };

            repositoryMock.Setup(repo => repo.UpdateAsync(updatedAuthor, cancellationToken))
                .ReturnsAsync(updatedAuthor);

            // Act
            var result = await repository.UpdateAsync(updatedAuthor, cancellationToken);

            // Assert
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo(updatedName));

            repositoryMock.Verify(repo => repo.UpdateAsync(updatedAuthor, cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(2, Description = "Valid list with two authors")]
        [TestCase(0, Description = "Empty list")]
        public async Task UpdateRangeAsync_TestCases(int authorCount)
        {
            // Arrange
            var authorsToUpdate = Enumerable.Range(1, authorCount)
                .Select(i => new Author
                {
                    Id = i,
                    Name = $"UpdatedAuthor{i}",
                    LastName = $"LastName{i}",
                    DateOfBirth = new DateTime(1980 + i, i, i, 0, 0, 0, DateTimeKind.Utc)
                })
                .ToList();

            // Act
            await repository.UpdateRangeAsync(authorsToUpdate, cancellationToken);

            // Assert
            if (authorCount > 0)
            {
                repositoryMock.Verify(repo => repo.UpdateRangeAsync(authorsToUpdate, cancellationToken), Times.Once);
            }
            else
            {
                repositoryMock.Verify(repo => repo.UpdateRangeAsync(It.IsAny<IEnumerable<Author>>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [Test]
        [TestCase(1, "AuthorToDelete", Description = "Deletes valid author by calling generic repository.")]
        public async Task DeleteAsync_TestCases(int id, string name)
        {
            // Arrange
            var author = new Author { Id = id, Name = name };

            // Act
            await repository.DeleteAsync(author, cancellationToken);

            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync(author, cancellationToken), Times.Once);
        }
    }
}