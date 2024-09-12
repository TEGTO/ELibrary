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
    internal class LibraryEntityServiceTests
    {
        private MockRepository mockRepository;
        private Mock<IDatabaseRepository<LibraryDbContext>> repositoryMock;
        private CancellationToken cancellationToken;
        private LibraryEntityService<Author> service;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            repositoryMock = new Mock<IDatabaseRepository<LibraryDbContext>>();
            service = new LibraryEntityService<Author>(repositoryMock.Object);
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
        public async Task GetByIdAsync_ValidId_ReturnsEntity()
        {
            // Arrange
            var entities = new List<Author> { new Author { Id = 1, Name = "Entity1" } };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(entities.AsQueryable());
            dbContextMock.Setup(db => db.Set<Author>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            var result = await service.GetByIdAsync(1, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result!.Id, Is.EqualTo(entities[0].Id));
            Assert.That(result.Name, Is.EqualTo(entities[0].Name));
        }
        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var entities = new List<Author> { new Author { Id = 1, Name = "Entity1" } };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(entities.AsQueryable());
            dbContextMock.Setup(db => db.Set<Author>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            var result = await service.GetByIdAsync(99, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetPaginatedAsync_ValidPage_ReturnsEntities()
        {
            // Arrange
            var entities = new List<Author> { new Author { Id = 1, Name = "Entity1" }, new Author { Id = 2, Name = "Entity2" } };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(entities.AsQueryable());
            dbContextMock.Setup(db => db.Set<Author>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            var result = await service.GetPaginatedAsync(1, 10, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Entity2"));
        }
        [Test]
        public async Task GetItemTotalAmountAsync_ReturnsCorrectCount()
        {
            // Arrange
            var entities = new List<Author>
            {
                new Author { Id = 1, Name = "Entity1" },
                new Author { Id = 2, Name = "Entity2" },
                new Author { Id = 3, Name = "Entity3" }
            };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(entities.AsQueryable());
            dbContextMock.Setup(db => db.Set<Author>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            var result = await service.GetItemTotalAmountAsync(CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(entities.Count));
        }
        [Test]
        public async Task CreateAsync_ValidEntity_AddsEntity()
        {
            // Arrange
            var entity = new Author { Id = 1, Name = "Entity2" };
            var entities = new List<Author> { new Author { Id = 1, Name = "Entity1" } };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(entities.AsQueryable());
            dbContextMock.Setup(db => db.Set<Author>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            var result = await service.CreateAsync(entity, CancellationToken.None);
            // Assert
            dbSetMock.Verify(d => d.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(result, Is.EqualTo(entity));
        }
        [Test]
        public async Task UpdateAsync_ValidEntity_UpdatesEntity()
        {
            // Arrange
            var entity = new Author { Id = 1, Name = "UpdatedEntity1" };
            var entities = new List<Author> { new Author { Id = 1, Name = "Entity1" } };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(entities.AsQueryable());
            dbContextMock.Setup(db => db.Set<Author>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            await service.UpdateAsync(entity, CancellationToken.None);
            // Assert
            dbSetMock.Verify(d => d.Update(It.IsAny<Author>()), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task DeleteByIdAsync_ValidId_DeletesEntity()
        {
            // Arrange
            var entities = new List<Author> { new Author { Id = 1, Name = "Entity1" } };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(entities.AsQueryable());
            dbContextMock.Setup(db => db.Set<Author>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            await service.DeleteByIdAsync(1, CancellationToken.None);
            // Assert
            dbSetMock.Verify(d => d.Remove(entities[0]), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task DeleteByIdAsync_InvalidId_DoesNothing()
        {
            // Arrange
            var entities = new List<Author> { new Author { Id = 1, Name = "Entity1" } };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(entities.AsQueryable());
            dbContextMock.Setup(db => db.Set<Author>()).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            await service.DeleteByIdAsync(99, CancellationToken.None);
            // Assert
            dbSetMock.Verify(d => d.Remove(It.IsAny<Author>()), Times.Never);
            dbContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
