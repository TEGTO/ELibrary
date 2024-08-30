using LibraryApi.Data;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Shared.Repositories;

namespace LibraryApiTests.Services
{
    [TestFixture]
    internal class LibraryEntityServiceTests
    {
        public class TestEntity : BaseEntity
        {
            public string Name { get; set; } = string.Empty;

            public override void Copy(BaseEntity other)
            {
                if (other is TestEntity otherEntity)
                {
                    Name = otherEntity.Name;
                }
            }
        }

        private Mock<IDatabaseRepository<LibraryDbContext>> repositoryMock;
        private Mock<LibraryDbContext> dbContextMock;
        private LibraryEntityService<TestEntity> service;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<LibraryDbContext>>();
            dbContextMock = new Mock<LibraryDbContext>(new DbContextOptions<LibraryDbContext>());
            repositoryMock.Setup(r => r.CreateDbContextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextMock.Object);
            service = new LibraryEntityService<TestEntity>(repositoryMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Entity1" };
            var dbSetMock = CreateDbSetMock(new List<TestEntity> { entity });
            dbContextMock.Setup(db => db.Set<TestEntity>()).Returns(dbSetMock.Object);

            // Act
            var result = await service.GetByIdAsync(1, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Id, result!.Id);
            Assert.AreEqual(entity.Name, result.Name);
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var dbSetMock = CreateDbSetMock(new List<TestEntity>());
            dbContextMock.Setup(db => db.Set<TestEntity>()).Returns(dbSetMock.Object);

            // Act
            var result = await service.GetByIdAsync(99, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetPaginatedAsync_ValidPage_ReturnsEntities()
        {
            // Arrange
            var entities = new List<TestEntity>
        {
            new TestEntity { Id = 1, Name = "Entity1" },
            new TestEntity { Id = 2, Name = "Entity2" }
        };
            var dbSetMock = CreateDbSetMock(entities);
            dbContextMock.Setup(db => db.Set<TestEntity>()).Returns(dbSetMock.Object);

            // Act
            var result = await service.GetPaginatedAsync(1, 10, CancellationToken.None);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Entity1", result.First().Name);
        }

        [Test]
        public async Task CreateAsync_ValidEntity_AddsEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Entity1" };
            var dbSetMock = CreateDbSetMock(new List<TestEntity>());
            dbContextMock.Setup(db => db.Set<TestEntity>()).Returns(dbSetMock.Object);

            // Act
            var result = await service.CreateAsync(entity, CancellationToken.None);

            // Assert
            dbSetMock.Verify(d => d.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(entity, result);
        }

        [Test]
        public async Task UpdateAsync_ValidEntity_UpdatesEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Entity1" };
            var dbSetMock = CreateDbSetMock(new List<TestEntity> { entity });
            dbContextMock.Setup(db => db.Set<TestEntity>()).Returns(dbSetMock.Object);

            // Act
            await service.UpdateAsync(entity, CancellationToken.None);

            // Assert
            dbSetMock.Verify(d => d.Update(entity), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteByIdAsync_ValidId_DeletesEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Entity1" };
            var dbSetMock = CreateDbSetMock(new List<TestEntity> { entity });
            dbContextMock.Setup(db => db.Set<TestEntity>()).Returns(dbSetMock.Object);

            // Act
            await service.DeleteByIdAsync(1, CancellationToken.None);

            // Assert
            dbSetMock.Verify(d => d.Remove(entity), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteByIdAsync_InvalidId_DoesNothing()
        {
            // Arrange
            var dbSetMock = CreateDbSetMock(new List<TestEntity>());
            dbContextMock.Setup(db => db.Set<TestEntity>()).Returns(dbSetMock.Object);

            // Act
            await service.DeleteByIdAsync(99, CancellationToken.None);

            // Assert
            dbSetMock.Verify(d => d.Remove(It.IsAny<TestEntity>()), Times.Never);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        private Mock<DbSet<T>> CreateDbSetMock<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            dbSetMock.Setup(d => d.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                     .Callback<T, CancellationToken>((entity, token) => data.Add(entity))
                     .Returns((T entity, CancellationToken token) => new ValueTask<EntityEntry<T>>(Mock.Of<EntityEntry<T>>()));

            dbSetMock.Setup(d => d.Remove(It.IsAny<T>()))
                     .Callback<T>(entity => data.Remove(entity));

            dbSetMock.Setup(d => d.Update(It.IsAny<T>()))
                     .Callback<T>(entity =>
                     {
                         var existing = data.FirstOrDefault(x => x == entity);
                         if (existing != null)
                         {
                             data.Remove(existing);
                             data.Add(entity);
                         }
                     });

            return dbSetMock;
        }
    }
}
