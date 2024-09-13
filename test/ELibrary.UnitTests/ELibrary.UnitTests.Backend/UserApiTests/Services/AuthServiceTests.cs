using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services.Tests
{
    [TestFixture]
    internal class AuthServiceTests
    {
        private const int EXPIRY_IN_DAYS = 7;

        private Mock<UserManager<User>> userManagerMock;
        private Mock<RoleManager<IdentityRole>> roleManagerMock;
        private Mock<ITokenHandler> tokenHandlerMock;
        private AuthService authService;

        [SetUp]
        public void SetUp()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();

            userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);
            tokenHandlerMock = new Mock<ITokenHandler>();

            authService = new AuthService(userManagerMock.Object, roleManagerMock.Object, tokenHandlerMock.Object);
        }

        [Test]
        public async Task RegisterUserAsync_UserAndPasswordProvided_IdentityResultReturned()
        {
            // Arrange
            var user = new User { UserName = "testuser", Email = "testuser@example.com" };
            var password = "Password123";
            var registerParams = new RegisterUserParams(user, password);
            var identityResult = IdentityResult.Success;
            userManagerMock.Setup(x => x.CreateAsync(user, password)).ReturnsAsync(identityResult);
            // Act
            var result = await authService.RegisterUserAsync(registerParams);
            // Assert
            Assert.That(result, Is.EqualTo(identityResult));
        }
        [Test]
        public async Task LoginUserAsync_ValidLoginAndPassword_TokenReturned()
        {
            // Arrange
            var user = new User { UserName = "testuser", Email = "testuser@example.com" };
            var loginParams = new LoginUserParams("testuser", "Password123", EXPIRY_IN_DAYS);
            var tokenData = new AccessTokenData { AccessToken = "token", RefreshToken = "refreshToken" };
            userManagerMock.Setup(x => x.FindByEmailAsync(loginParams.Login)).ReturnsAsync((User)null);
            userManagerMock.Setup(x => x.FindByNameAsync(loginParams.Login)).ReturnsAsync(user);
            userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginParams.Password)).ReturnsAsync(true);
            tokenHandlerMock.Setup(x => x.CreateToken(user, It.IsAny<IList<string>>())).Returns(tokenData);
            userManagerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
            // Act
            var result = await authService.LoginUserAsync(loginParams);
            // Assert
            Assert.That(result, Is.EqualTo(tokenData));
            Assert.That(result.RefreshTokenExpiryDate, Is.GreaterThan(DateTime.UtcNow.AddDays(EXPIRY_IN_DAYS - 1)));
        }
        [Test]
        public void LoginUserAsync_InvalidLoginOrPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var loginParams = new LoginUserParams("testuser", "wrongPassword", EXPIRY_IN_DAYS);
            var user = new User { UserName = "testuser", Email = "testuser@example.com" };
            userManagerMock.Setup(x => x.FindByNameAsync(loginParams.Login)).ReturnsAsync(user);
            userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginParams.Password)).ReturnsAsync(false);
            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => authService.LoginUserAsync(loginParams));
        }
        [Test]
        public async Task SetUserRolesAsync_ValidRoles_IdentityErrorsEmpty()
        {
            // Arrange
            var user = new User { UserName = "testuser", Email = "testuser@example.com" };
            var roles = new List<string> { "Admin", "User" };
            var currentRoles = new List<string> { "Admin" };
            userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(currentRoles);
            userManagerMock.Setup(x => x.RemoveFromRolesAsync(user, currentRoles)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(user, It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            // Act
            var result = await authService.SetUserRolesAsync(user, roles);
            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        public async Task SetUserRolesAsync_InvalidRole_AddToRoleFails_IdentityErrorsReturned()
        {
            // Arrange
            var user = new User { UserName = "testuser", Email = "testuser@example.com" };
            var roles = new List<string> { "InvalidRole" };
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Error adding role" } };
            userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string>());
            roleManagerMock.Setup(x => x.RoleExistsAsync("InvalidRole")).ReturnsAsync(false);
            userManagerMock.Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));
            // Act
            var result = await authService.SetUserRolesAsync(user, roles);
            // Assert
            Assert.That(result.Count, Is.EqualTo(identityErrors.Count));
            Assert.That(result[0].Description, Is.EqualTo(identityErrors[0].Description));
        }
        [Test]
        public async Task UpdateUserAsync_ValidUpdate_NoErrors()
        {
            // Arrange
            var user = new User { UserName = "testuser", Email = "testuser@example.com" };
            var updateData = new UserUpdateData { UserName = "newuser", Email = "newemail@example.com", Password = "newpass", OldPassword = "oldpass" };
            var identityResult = IdentityResult.Success;
            userManagerMock.Setup(x => x.SetUserNameAsync(user, updateData.UserName)).ReturnsAsync(identityResult);
            userManagerMock.Setup(x => x.GenerateChangeEmailTokenAsync(user, updateData.Email)).ReturnsAsync("emailToken");
            userManagerMock.Setup(x => x.ChangeEmailAsync(user, updateData.Email, "emailToken")).ReturnsAsync(identityResult);
            userManagerMock.Setup(x => x.ChangePasswordAsync(user, updateData.OldPassword, updateData.Password)).ReturnsAsync(identityResult);
            // Act
            var result = await authService.UpdateUserAsync(user, updateData, false);
            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        public async Task UpdateUserAsync_InvalidPasswordChange_ErrorsReturned()
        {
            // Arrange
            var user = new User { UserName = "testuser", Email = "testuser@example.com" };
            var updateData = new UserUpdateData { UserName = "newuser", Email = "newemail@example.com", Password = "newpass", OldPassword = "oldpass" };
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Password change failed" } };
            userManagerMock.Setup(x => x.SetUserNameAsync(user, updateData.UserName)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.GenerateChangeEmailTokenAsync(user, updateData.Email)).ReturnsAsync("emailToken");
            userManagerMock.Setup(x => x.ChangeEmailAsync(user, updateData.Email, "emailToken")).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.ChangePasswordAsync(user, updateData.OldPassword, updateData.Password)).ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));
            // Act
            var result = await authService.UpdateUserAsync(user, updateData, false);
            // Assert
            Assert.That(result.Count, Is.EqualTo(identityErrors.Count));
            Assert.That(result[0].Description, Is.EqualTo(identityErrors[0].Description));
        }
        [Test]
        public async Task DeleteUserAsync_ValidUser_IdentityResultReturned()
        {
            // Arrange
            var user = new User { UserName = "testuser", Email = "testuser@example.com" };
            var identityResult = IdentityResult.Success;
            userManagerMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(identityResult);
            // Act
            var result = await authService.DeleteUserAsync(user);
            // Assert
            Assert.That(result, Is.EqualTo(identityResult));
        }
    }
}