using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;
using UserApi.Data;
using UserApi.Domain.Entities;

namespace UserApi.Services
{
    [TestFixture]
    internal class UserInfoServiceTests
    {
        private MockRepository mockRepository;
        private Mock<IDatabaseRepository<UserIdentityDbContext>> repositoryMock;
        private UserInfoService userInfoService;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            repositoryMock = new Mock<IDatabaseRepository<UserIdentityDbContext>>();
            userInfoService = new UserInfoService(repositoryMock.Object);
            cancellationToken = new CancellationToken();
        }
        private Mock<UserIdentityDbContext> CreateMockDbContext()
        {
            var options = new DbContextOptionsBuilder<UserIdentityDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var mockDbContext = mockRepository.Create<UserIdentityDbContext>(options);
            return mockDbContext;
        }
        private static Mock<DbSet<T>> GetDbSetMock<T>(IQueryable<T> data) where T : class
        {
            return data.BuildMockDbSet();
        }

        [Test]
        public async Task GetUserInfoAsync_ValidUserId_ReturnsUserInfo()
        {
            // Arrange
            var userId = "valid-user-id";
            var userInfos = new List<UserInfo>
            {
               new UserInfo { UserId = userId, Name = "John", LastName = "Doe" }
            };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(userInfos.AsQueryable());
            dbContextMock.Setup(x => x.UserInfos).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            var result = await userInfoService.GetUserInfoAsync(userId, CancellationToken.None);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.UserId, Is.EqualTo(userId));
        }
        [Test]
        public async Task GetUserInfoAsync_InvalidUserId_ReturnsNull()
        {
            // Arrange
            var userId = "invalid-user-id";
            var userInfos = new List<UserInfo>
            {
               new UserInfo { UserId = "valid-id", Name = "John", LastName = "Doe" }
            };
            var dbContextMock = CreateMockDbContext();
            var dbSetMock = GetDbSetMock(userInfos.AsQueryable());
            dbContextMock.Setup(x => x.UserInfos).Returns(dbSetMock.Object);
            repositoryMock.Setup(x => x.CreateDbContextAsync(cancellationToken)).ReturnsAsync(dbContextMock.Object);
            // Act
            var result = await userInfoService.GetUserInfoAsync(userId, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public void GetUserInfoAsync_DatabaseThrowsException_ThrowsException()
        {
            // Arrange
            var repositoryMock = new Mock<IDatabaseRepository<UserIdentityDbContext>>();
            repositoryMock.Setup(repo => repo.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                          .ThrowsAsync(new Exception("Database error"));
            userInfoService = new UserInfoService(repositoryMock.Object);
            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await userInfoService.GetUserInfoAsync("user-id", CancellationToken.None));
        }
    }
}