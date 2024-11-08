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
    internal class LibraryEntityRepositoryTests
    {
        private Mock<IDatabaseRepository<LibraryDbContext>> repositoryMock;
        private CancellationToken cancellationToken;
        private LibraryEntityRepository<TestEntity> repository;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<LibraryDbContext>>();
            repository = new LibraryEntityRepository<TestEntity>(repositoryMock.Object);
            cancellationToken = new CancellationToken();
        }
        private static Mock<DbSet<T>> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMockDbSet();
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsEntity()
        {
            // Arrange
            var entities = new List<TestEntity> { new TestEntity { Id = 1, Name = "Entity1" } };
            var dbSetMock = GetDbSetMock(entities);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            // Act
            var result = await repository.GetByIdAsync(1, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result!.Id, Is.EqualTo(entities[0].Id));
            Assert.That(result.Name, Is.EqualTo(entities[0].Name));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var entities = new List<TestEntity> { new TestEntity { Id = 1, Name = "Entity1" } };
            var dbSetMock = GetDbSetMock(entities);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            // Act
            var result = await repository.GetByIdAsync(99, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetByIds_ValidIds_ReturnsEntities()
        {
            // Arrange
            var entities = new List<TestEntity> { new TestEntity { Id = 1, Name = "Entity1" }, new TestEntity { Id = 2, Name = "Entity2" } };
            var dbSetMock = GetDbSetMock(entities);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            var ids = new List<int> { 1, 2, 3 };
            // Act
            var result = await repository.GetByIdsAsync(ids, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Entity1"));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetPaginatedAsync_ValidPage_ReturnsEntities()
        {
            // Arrange
            var entities = new List<TestEntity> { new TestEntity { Id = 1, Name = "Entity1" }, new TestEntity { Id = 2, Name = "Entity2" } };
            var dbSetMock = GetDbSetMock(entities);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            var paginationRequest = new LibraryFilterRequest() { PageNumber = 1, PageSize = 10 };
            // Act
            var result = await repository.GetPaginatedAsync(paginationRequest, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Entity2"));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetItemTotalAmountAsync_ReturnsCorrectCount()
        {
            // Arrange
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Entity1" },
                new TestEntity { Id = 2, Name = "Entity2" },
                new TestEntity { Id = 3, Name = "Entity3" }
            };
            var dbSetMock = GetDbSetMock(entities);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            // Act
            var result = await repository.GetItemTotalAmountAsync(new LibraryFilterRequest() { ContainsName = "" }, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(entities.Count));
            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task CreateAsync_ValidEntity_AddsEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Entity2" };
            repositoryMock.Setup(repo => repo.AddAsync(entity, cancellationToken)).ReturnsAsync(entity);
            // Act
            var result = await repository.CreateAsync(entity, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(entity));
            repositoryMock.Verify(d => d.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task UpdateAsync_ValidEntity_UpdatesEntity()
        {
            // Arrange
            var entityInDb = new TestEntity { Id = 1, Name = "Entity1" };
            var entity = new TestEntity { Id = 1, Name = "UpdatedEntity1" };
            repositoryMock.Setup(repo => repo.UpdateAsync(entity, cancellationToken))
               .ReturnsAsync(entityInDb);
            // Act
            var result = await repository.UpdateAsync(entity, CancellationToken.None);
            // Assert
            repositoryMock.Verify(repo => repo.UpdateAsync(entity, cancellationToken), Times.Once);
            Assert.That(result, Is.EqualTo(entityInDb));
        }
        [Test]
        public async Task DeleteAsync_ValidId_DeletesEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Entity1" };
            // Act
            await repository.DeleteAsync(entity, CancellationToken.None);
            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync(entity, cancellationToken), Times.Once);
        }
    }

    public class TestEntity : BaseLibraryEntity
    {
        public override void Copy(BaseLibraryEntity other)
        {
            if (other is TestEntity otherEntity)
            {
                Name = otherEntity.Name;
            }
        }
    }
}