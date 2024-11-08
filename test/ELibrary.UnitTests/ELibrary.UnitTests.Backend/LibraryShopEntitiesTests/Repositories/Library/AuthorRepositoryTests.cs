using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;

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
        public async Task GetByIdAsync_ValidId_ReturnsAuthorWithEntities()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                Name = "Author1",
            };
            var authors = new List<Author> { author };
            var dbSetMock = GetDbSetMock(authors);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Author>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            // Act
            var result = await repository.GetByIdAsync(1, cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result!.Id, Is.EqualTo(author.Id));
            Assert.That(result.Name, Is.EqualTo(author.Name));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Author>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var authors = new List<Author>();
            var dbSetMock = GetDbSetMock(authors);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Author>(cancellationToken))
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
            var authors = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    Name =  "Author1",
                },
                new Author
                {
                    Id = 2,
                    Name = "Author2",
                }
            };
            var dbAuthorSetMock = GetDbSetMock(authors);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Author>(cancellationToken))
               .ReturnsAsync(dbAuthorSetMock.Object);
            var ids = new List<int> { 1, 2, 3 };
            // Act
            var result = await repository.GetByIdsAsync(ids, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Author1"));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Author>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetPaginatedAsync_ValidPage_ReturnsAuthorsWithEntities()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    Name =  "Author1",
                },
                new Author
                {
                    Id = 2,
                    Name = "Author2",
                }
            };
            var dbAuthorSetMock = GetDbSetMock(authors);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Author>(cancellationToken))
               .ReturnsAsync(dbAuthorSetMock.Object);
            var paginationRequest = new LibraryFilterRequest() { PageNumber = 1, PageSize = 10 };
            // Act
            var result = await repository.GetPaginatedAsync(paginationRequest, cancellationToken);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Author2"));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Author>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetItemTotalAmountAsync_ReturnsCorrectCount()
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
            var filterRequest = new LibraryFilterRequest();
            // Act
            var result = await repository.GetItemTotalAmountAsync(filterRequest, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(2));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<Author>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task CreateAsync_ValidAuthor_AddsAuthorWithEntities()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                Name = "Author1",
            };
            var authors = new List<Author> { author };
            var dbSetMock = GetDbSetMock(authors);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<Author>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            repositoryMock.Setup(repo => repo.AddAsync(author, cancellationToken)).ReturnsAsync(author);
            // Act
            var result = await repository.CreateAsync(author, cancellationToken);
            // Assert
            Assert.That(result.Id, Is.EqualTo(author.Id));
            repositoryMock.Verify(d => d.AddAsync(author, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task UpdateAsync_ValidAuthor_UpdatesAuthorWithEntities()
        {
            // Arrange
            var updatedAuthor = new Author
            {
                Id = 1,
                Name = "UpdatedAuthor1",
            };
            // Act
            await repository.UpdateAsync(updatedAuthor, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.UpdateAsync(updatedAuthor, cancellationToken), Times.Once);
        }
        [Test]
        public async Task DeleteByIdAsync_ValidId_DeletesAuthor()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                Name = "Author1",
            };
            // Act
            await repository.DeleteAsync(author, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync(author, cancellationToken), Times.Once);
        }
    }
}