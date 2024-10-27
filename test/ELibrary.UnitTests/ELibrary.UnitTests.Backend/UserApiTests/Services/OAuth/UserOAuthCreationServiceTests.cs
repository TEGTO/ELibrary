using Microsoft.AspNetCore.Identity;
using Moq;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services.OAuth.Tests
{
    [TestFixture]
    internal class UserOAuthCreationServiceTests
    {
        private Mock<UserManager<User>> mockUserManager;
        private Mock<IUserAuthenticationMethodService> authMethodService;
        private UserOAuthCreationService userCreationService;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            authMethodService = new Mock<IUserAuthenticationMethodService>();

            userCreationService = new UserOAuthCreationService(mockUserManager.Object, authMethodService.Object);
        }

        [Test]
        public async Task CreateUserFromOAuthAsync_UserExistsByLogin_ReturnsExistingUser()
        {
            // Arrange
            var existingUser = new User { UserName = "existingUser@example.com" };
            mockUserManager.Setup(m => m.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(existingUser);
            var model = new CreateUserFromOAuth
            {
                Email = "newuser@example.com",
                LoginProviderSubject = "12345",
                AuthMethod = AuthenticationMethod.GoogleOAuth
            };
            // Act
            var result = await userCreationService.CreateUserFromOAuthAsync(model, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(existingUser));
        }
        [Test]
        public async Task CreateUserFromOAuthAsync_UserDoesntExist_CreatesNewUser()
        {
            // Arrange
            mockUserManager.Setup(m => m.AddLoginAsync(It.IsAny<User>(), It.IsAny<UserLoginInfo>()))
                            .ReturnsAsync(IdentityResult.Success);
            var model = new CreateUserFromOAuth
            {
                Email = "newuser@example.com",
                LoginProviderSubject = "12345",
                AuthMethod = AuthenticationMethod.GoogleOAuth
            };
            // Act
            var result = await userCreationService.CreateUserFromOAuthAsync(model, CancellationToken.None);
            // Assert
            Assert.That(result, Is.Not.Null);
            mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
            mockUserManager.Verify(x => x.AddLoginAsync(It.IsAny<User>(), It.IsAny<UserLoginInfo>()), Times.Once);
        }
    }
}