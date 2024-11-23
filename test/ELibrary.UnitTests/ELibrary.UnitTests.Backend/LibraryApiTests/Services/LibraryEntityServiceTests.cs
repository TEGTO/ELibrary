using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using LibraryShopEntities.Repositories.Library;
using Moq;

namespace LibraryApi.Services.Tests
{
    [TestFixture]
    internal class LibraryEntityServiceTests
    {
        private Mock<ILibraryEntityRepository<TestEntity>> repositoryMock;
        private LibraryEntityService<TestEntity> service;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<ILibraryEntityRepository<TestEntity>>();

            service = new LibraryEntityService<TestEntity>(repositoryMock.Object);
            cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task GetByIdAsync_ValidId_CallsRepositoryAndReturnsEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Entity1" };

            repositoryMock.Setup(repo => repo.GetByIdAsync(1, cancellationToken)).ReturnsAsync(entity);

            // Act
            var result = await service.GetByIdAsync(1, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result!.Id, Is.EqualTo(entity.Id));
            repositoryMock.Verify(repo => repo.GetByIdAsync(1, cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_CallsRepositoryAndReturnsNull()
        {
            // Arrange
            repositoryMock.Setup(repo => repo.GetByIdAsync(99, cancellationToken)).ReturnsAsync((TestEntity?)null);

            // Act
            var result = await service.GetByIdAsync(99, cancellationToken);

            // Assert
            Assert.IsNull(result);

            repositoryMock.Verify(repo => repo.GetByIdAsync(99, cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetByIdsAsync_ValidIds_CallsRepositoryAndReturnsEntities()
        {
            // Arrange
            var entities = new List<TestEntity> { new TestEntity { Id = 1 }, new TestEntity { Id = 2 } };

            var ids = new List<int> { 1, 2 };

            repositoryMock.Setup(repo => repo.GetByIdsAsync(ids, cancellationToken)).ReturnsAsync(entities);

            // Act
            var result = await service.GetByIdsAsync(ids, cancellationToken);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(entities.Count));

            repositoryMock.Verify(repo => repo.GetByIdsAsync(ids, cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetPaginatedAsync_ValidRequest_CallsRepositoryAndReturnsEntities()
        {
            // Arrange
            var request = new LibraryFilterRequest { PageNumber = 1, PageSize = 10 };

            var entities = new List<TestEntity> { new TestEntity { Id = 1 }, new TestEntity { Id = 2 } };

            repositoryMock.Setup(repo => repo.GetPaginatedAsync(request, cancellationToken)).ReturnsAsync(entities);

            // Act
            var result = await service.GetPaginatedAsync(request, cancellationToken);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(entities.Count));

            repositoryMock.Verify(repo => repo.GetPaginatedAsync(request, cancellationToken), Times.Once);
        }

        [Test]
        public async Task GetItemTotalAmountAsync_ValidRequest_CallsRepositoryAndReturnsCount()
        {
            // Arrange
            var request = new LibraryFilterRequest { ContainsName = "Entity" };

            repositoryMock.Setup(repo => repo.GetItemTotalAmountAsync(request, cancellationToken)).ReturnsAsync(5);

            // Act
            var result = await service.GetItemTotalAmountAsync(request, cancellationToken);

            // Assert
            Assert.That(result, Is.EqualTo(5));

            repositoryMock.Verify(repo => repo.GetItemTotalAmountAsync(request, cancellationToken), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ValidEntity_CallsRepositoryAndReturnsCreatedEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "NewEntity" };

            repositoryMock.Setup(repo => repo.CreateAsync(entity, cancellationToken)).ReturnsAsync(entity);

            // Act
            var result = await service.CreateAsync(entity, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(entity.Id));

            repositoryMock.Verify(repo => repo.CreateAsync(entity, cancellationToken), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ValidEntity_CallsRepositoryAndReturnsUpdatedEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "UpdatedEntity" };
            var entityInDb = new TestEntity { Id = 1, Name = "OldEntity" };

            repositoryMock.Setup(repo => repo.GetByIdAsync(entity.Id, cancellationToken)).ReturnsAsync(entityInDb);
            repositoryMock.Setup(repo => repo.UpdateAsync(entityInDb, cancellationToken)).ReturnsAsync(entityInDb);

            // Act
            var result = await service.UpdateAsync(entity, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(entityInDb.Id));
            Assert.That(result.Name, Is.EqualTo(entity.Name));

            repositoryMock.Verify(repo => repo.GetByIdAsync(entity.Id, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.UpdateAsync(entityInDb, cancellationToken), Times.Once);
        }

        [Test]
        public void UpdateAsync_InvalidEntity_ThrowsInvalidOperationException()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "UpdatedEntity" };

            repositoryMock.Setup(repo => repo.GetByIdAsync(entity.Id, cancellationToken)).ReturnsAsync((TestEntity?)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.UpdateAsync(entity, cancellationToken));

            repositoryMock.Verify(repo => repo.GetByIdAsync(entity.Id, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<TestEntity>(), cancellationToken), Times.Never);
        }

        [Test]
        public async Task DeleteByIdAsync_ValidId_CallsRepositoryToDeleteEntity()
        {
            // Arrange
            var entityInDb = new TestEntity { Id = 1, Name = "Entity1" };

            repositoryMock.Setup(repo => repo.GetByIdAsync(entityInDb.Id, cancellationToken)).ReturnsAsync(entityInDb);

            // Act
            await service.DeleteAsync(entityInDb.Id, cancellationToken);

            // Assert
            repositoryMock.Verify(repo => repo.GetByIdAsync(entityInDb.Id, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteAsync(entityInDb, cancellationToken), Times.Once);
        }

        [Test]
        public async Task DeleteByIdAsync_InvalidId_DoesNotCallDelete()
        {
            // Arrange
            repositoryMock.Setup(repo => repo.GetByIdAsync(99, cancellationToken)).ReturnsAsync((TestEntity?)null);

            // Act
            await service.DeleteAsync(99, cancellationToken);

            // Assert
            repositoryMock.Verify(repo => repo.GetByIdAsync(99, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<TestEntity>(), cancellationToken), Times.Never);
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