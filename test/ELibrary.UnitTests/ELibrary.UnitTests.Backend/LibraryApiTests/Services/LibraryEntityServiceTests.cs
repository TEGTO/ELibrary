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
    internal class LibraryEntityServiceTests
    {
        private Mock<IDatabaseRepository<LibraryDbContext>> repositoryMock;
        private CancellationToken cancellationToken;
        private LibraryEntityService<TestEntity> service;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<LibraryDbContext>>();
            service = new LibraryEntityService<TestEntity>(repositoryMock.Object);
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
            var result = await service.GetByIdAsync(1, CancellationToken.None);
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
            var result = await service.GetByIdAsync(99, CancellationToken.None);
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
            var result = await service.GetByIdsAsync(ids, CancellationToken.None);
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
            var result = await service.GetPaginatedAsync(paginationRequest, CancellationToken.None);
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
            var result = await service.GetItemTotalAmountAsync(new LibraryFilterRequest() { ContainsName = "" }, CancellationToken.None);
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
            var result = await service.CreateAsync(entity, CancellationToken.None);
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
            var entities = new List<TestEntity> { entityInDb };
            var dbSetMock = GetDbSetMock(entities);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
               .ReturnsAsync(dbSetMock.Object);
            repositoryMock.Setup(repo => repo.UpdateAsync(entityInDb, cancellationToken))
               .ReturnsAsync(entityInDb);
            // Act
            await service.UpdateAsync(entity, CancellationToken.None);
            // Assert
            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.UpdateAsync(entityInDb, cancellationToken), Times.Once);
        }
        [Test]
        public async Task DeleteByIdAsync_ValidId_DeletesEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Entity1" };
            var entities = new List<TestEntity> { entity };
            var dbSetMock = GetDbSetMock(entities);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);
            repositoryMock.Setup(repo => repo.DeleteAsync(entities, cancellationToken))
                .Returns(Task.CompletedTask);
            // Act
            await service.DeleteByIdAsync(1, CancellationToken.None);
            // Assert
            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteAsync(entity, cancellationToken), Times.Once);
        }
        [Test]
        public async Task DeleteByIdAsync_InvalidId_DoesNothing()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Entity1" };
            var entities = new List<TestEntity> { entity };
            var dbSetMock = GetDbSetMock(entities);
            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);
            repositoryMock.Setup(repo => repo.DeleteAsync(entities, cancellationToken))
                .Returns(Task.CompletedTask);
            // Act
            await service.DeleteByIdAsync(99, CancellationToken.None);
            // Assert
            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteAsync(entity, cancellationToken), Times.Never);
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
