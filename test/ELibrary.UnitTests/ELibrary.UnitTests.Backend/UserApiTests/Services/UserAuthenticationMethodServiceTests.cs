using DatabaseControl.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UserEntities.Data;
using UserEntities.Domain.Entities;

namespace UserApi.Services.Tests
{
    [TestFixture]
    internal class UserAuthenticationMethodServiceTests
    {
        private Mock<IDatabaseRepository<UserIdentityDbContext>> mockDatabaseRepository;
        private UserAuthenticationMethodService service;

        [SetUp]
        public void SetUp()
        {
            mockDatabaseRepository = new Mock<IDatabaseRepository<UserIdentityDbContext>>();
            service = new UserAuthenticationMethodService(mockDatabaseRepository.Object);
        }

        private static Mock<DbSet<T>> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMockDbSet();
        }

        [Test]
        public async Task SetUserAuthenticationMethodAsync_MethodNotPresent_AddsMethod()
        {
            // Arrange
            var user = new User { Id = "1", AuthenticationMethods = new List<UserAuthenticationMethod>() };
            var users = new List<User> { user };

            var dbSetMock = GetDbSetMock(users);

            mockDatabaseRepository.Setup(repo => repo.GetQueryableAsync<User>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(dbSetMock.Object);

            // Act
            await service.SetUserAuthenticationMethodAsync(user, AuthenticationMethod.GoogleOAuth, CancellationToken.None);

            // Assert
            mockDatabaseRepository.Verify(
                repo => repo.UpdateAsync(It.Is<User>(u => u.AuthenticationMethods.Count == 1), It.IsAny<CancellationToken>()),
                Times.Once);

            Assert.That(user.AuthenticationMethods[0].AuthenticationMethod, Is.EqualTo(AuthenticationMethod.GoogleOAuth));
        }

        [Test]
        public async Task SetUserAuthenticationMethodAsync_MethodAlreadyExists_DoesNotAddMethod()
        {
            // Arrange
            var user = new User
            {
                Id = "1",
                AuthenticationMethods = new List<UserAuthenticationMethod>
                {
                    new UserAuthenticationMethod { AuthenticationMethod = AuthenticationMethod.GoogleOAuth }
                }
            };
            var users = new List<User> { user };

            var dbSetMock = GetDbSetMock(users);

            mockDatabaseRepository.Setup(repo => repo.GetQueryableAsync<User>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(dbSetMock.Object);

            // Act
            await service.SetUserAuthenticationMethodAsync(user, AuthenticationMethod.GoogleOAuth, CancellationToken.None);

            // Assert
            mockDatabaseRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}