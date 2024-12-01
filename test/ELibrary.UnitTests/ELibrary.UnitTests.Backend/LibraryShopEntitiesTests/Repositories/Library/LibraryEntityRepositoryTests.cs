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
        [TestCase(1, "Test1", true, Description = "Entity exists.")]
        [TestCase(99, null, false, Description = "Entity does not exist.")]
        public async Task GetByIdAsync_TestCases(int id, string? expectedName, bool shouldExist)
        {
            // Arrange
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Test1" },
                new TestEntity { Id = 2, Name = "Test2" }
            };
            var dbSetMock = GetDbSetMock(entities);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
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

            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
        }


        [Test]
        [TestCase(new[] { 1, 2 }, 2, Description = "Finds two entities by their ids.")]
        [TestCase(new[] { 1 }, 1, Description = "Finds one entity by it id.")]
        [TestCase(new[] { 99, 100 }, 0, Description = "Finds zero entities.")]
        public async Task GetByIdsAsync_TestCases(int[] ids, int expectedCount)
        {
            // Arrange
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Test1" },
                new TestEntity { Id = 2, Name = "Test2" }
            };
            var dbSetMock = GetDbSetMock(entities);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);

            // Act
            var result = await repository.GetByIdsAsync(ids, cancellationToken);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedCount));

            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(1, 10, 2, "", "Test2", Description = "Pagination takes first page with 10 items, gets two entities.")]
        [TestCase(1, 10, 1, "Test1", "Test1", Description = "Pagination takes first page with 10 items, gets one entity.")]
        [TestCase(2, 10, 0, "", null, Description = "Pagination takes second page with 10 items, gets zero entities.")]
        public async Task GetPaginatedAsync_TestCases(int pageNumber, int pageSize, int expectedCount, string containsName, string? firstAuthorName)
        {
            // Arrange
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Test1" },
                new TestEntity { Id = 2, Name = "Test2" }
            };
            var dbSetMock = GetDbSetMock(entities);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
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

            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(2, 2, "", Description = "Count all entities, returns count two.")]
        [TestCase(2, 1, "Test1", Description = "Count all entities, returns count one.")]
        [TestCase(0, 0, "", Description = "Count all entities, returns count zero.")]
        public async Task GetItemTotalAmountAsync_TestCases(int realCount, int expectedCount, string containsName)
        {
            // Arrange
            var entities = Enumerable.Range(1, realCount)
                .Select(i => new TestEntity { Id = i, Name = $"Test{i}" })
                .ToList();
            var dbSetMock = GetDbSetMock(entities);

            repositoryMock.Setup(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken))
                .ReturnsAsync(dbSetMock.Object);

            var filterRequest = new LibraryFilterRequest() { ContainsName = containsName };

            // Act
            var result = await repository.GetItemTotalAmountAsync(filterRequest, cancellationToken);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCount));

            repositoryMock.Verify(repo => repo.GetQueryableAsync<TestEntity>(cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(2, "Test1", Description = "Adds valid entity by calling generic repository.")]
        public async Task CreateAsync_TestCases(int id, string name)
        {
            // Arrange
            var entity = new TestEntity { Id = id, Name = name };

            repositoryMock.Setup(repo => repo.AddAsync(entity, cancellationToken))
                .ReturnsAsync(entity);

            // Act
            var result = await repository.CreateAsync(entity, cancellationToken);

            // Assert
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo(name));

            repositoryMock.Verify(repo => repo.AddAsync(entity, cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(1, "UpdatedTest1", Description = "Updates valid entity by calling generic repository.")]
        public async Task UpdateAsync_TestCases(int id, string updatedName)
        {
            // Arrange
            var updatedEntity = new TestEntity { Id = id, Name = updatedName };

            repositoryMock.Setup(repo => repo.UpdateAsync(updatedEntity, cancellationToken))
                .ReturnsAsync(updatedEntity);

            // Act
            var result = await repository.UpdateAsync(updatedEntity, cancellationToken);

            // Assert
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo(updatedName));

            repositoryMock.Verify(repo => repo.UpdateAsync(updatedEntity, cancellationToken), Times.Once);
        }

        [Test]
        [TestCase(2, Description = "Valid list with two entities")]
        [TestCase(0, Description = "Empty list")]
        public async Task UpdateRangeAsync_TestCases(int count)
        {
            // Arrange
            var entitiesToUpdate = Enumerable.Range(1, count)
                .Select(i => new TestEntity
                {
                    Id = i,
                    Name = $"UpdatedTest{i}",
                })
                .ToList();

            // Act
            await repository.UpdateRangeAsync(entitiesToUpdate, cancellationToken);

            // Assert
            if (count > 0)
            {
                repositoryMock.Verify(repo => repo.UpdateRangeAsync(entitiesToUpdate, cancellationToken), Times.Once);
            }
            else
            {
                repositoryMock.Verify(repo => repo.UpdateRangeAsync(It.IsAny<IEnumerable<TestEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [Test]
        [TestCase(1, "TestToDelete", Description = "Deletes valid entity by calling generic repository.")]
        public async Task DeleteAsync_TestCases(int id, string name)
        {
            // Arrange
            var entity = new TestEntity { Id = id, Name = name };

            // Act
            await repository.DeleteAsync(entity, cancellationToken);

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