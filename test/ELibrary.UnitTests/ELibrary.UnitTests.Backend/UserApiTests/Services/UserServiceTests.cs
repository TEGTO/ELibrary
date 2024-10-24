using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services.Tests
{
    [TestFixture()]
    public class UserServiceTests
    {
        private Mock<UserManager<User>> userManagerMock;
        private Mock<RoleManager<IdentityRole>> roleManagerMock;
        private UserService userService;

        [SetUp]
        public void SetUp()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();

            userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);

            userService = new UserService(userManagerMock.Object, roleManagerMock.Object);
        }

        [Test]
        public async Task GetUserAsync_ValidClaimsPrincipal_UserReturned()
        {
            // Arrange
            var user = new User { Id = "test-user-id", UserName = "testuser" };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }));
            userManagerMock.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);
            // Act
            var result = await userService.GetUserAsync(claimsPrincipal, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(user));
        }
        [Test]
        public async Task GetUserAsync_InvalidClaimsPrincipal_ReturnsNull()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            // Act
            var result = await userService.GetUserAsync(claimsPrincipal, CancellationToken.None);
            // Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task GetUserByUserInfoAsync_UserFoundByEmail_UserReturned()
        {
            // Arrange
            var user = new User { Id = "test-user-id", UserName = "testuser", Email = "testuser@example.com" };
            userManagerMock.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            // Act
            var result = await userService.GetUserByUserInfoAsync(user.Email, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(user));
        }
        [Test]
        public async Task GetUserByUserInfoAsync_UserNotFound_ReturnsNull()
        {
            // Arrange
            var info = "non-existent-user";
            userManagerMock.Setup(x => x.FindByEmailAsync(info)).ReturnsAsync((User)null);
            userManagerMock.Setup(x => x.FindByNameAsync(info)).ReturnsAsync((User)null);
            userManagerMock.Setup(x => x.FindByIdAsync(info)).ReturnsAsync((User)null);
            // Act
            var result = await userService.GetUserByUserInfoAsync(info, CancellationToken.None);
            // Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task GetPaginatedUsersAsync_ValidFilter_ReturnsPaginatedUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "test-user-id-1", UserName = "testuser1" },
                new User { Id = "test-user-id-2", UserName = "testuser2" }
            };
            var filter = new AdminGetUserFilter { PageNumber = 1, PageSize = 2 };
            userManagerMock.Setup(x => x.Users).Returns(users.AsQueryable().BuildMock());
            // Act
            var result = await userService.GetPaginatedUsersAsync(filter, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Is.EquivalentTo(users));
        }
        [Test]
        public async Task GetUserTotalAmountAsync_ValidFilter_ReturnsTotalUserCount()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "test-user-id-1", UserName = "testuser1" },
                new User { Id = "test-user-id-2", UserName = "testuser2" }
            };
            var filter = new AdminGetUserFilter();
            userManagerMock.Setup(x => x.Users).Returns(users.AsQueryable().BuildMock());
            // Act
            var result = await userService.GetUserTotalAmountAsync(filter, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(2));
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
            var result = await userService.SetUserRolesAsync(user, roles, CancellationToken.None);
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
            var result = await userService.SetUserRolesAsync(user, roles, CancellationToken.None);
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
            var result = await userService.UpdateUserAsync(user, updateData, false, CancellationToken.None);
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
            var result = await userService.UpdateUserAsync(user, updateData, false, CancellationToken.None);
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
            var result = await userService.DeleteUserAsync(user, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(identityResult));
        }
    }
}