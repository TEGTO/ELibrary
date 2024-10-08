using Authentication.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Shared.Exceptions;
using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services.Tests
{
    [TestFixture]
    internal class UserManagerTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<IAuthService> authServiceMock;
        private Mock<IConfiguration> configurationMock;
        private UserManager userManager;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();
            configurationMock = new Mock<IConfiguration>();

            configurationMock.Setup(c => c[It.Is<string>(s => s == Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS)])
                             .Returns("7");

            userManager = new UserManager(mapperMock.Object, authServiceMock.Object, configurationMock.Object);
        }

        [Test]
        public async Task GetUserByUserInfoAsync_UserExists_ReturnsAdminUserResponse()
        {
            // Arrange
            var login = "adminuser";
            var user = new User { UserName = login, Email = "adminuser@example.com" };
            var roles = new List<string> { "Admin" };
            var adminResponse = new AdminUserResponse { Email = "adminuser@example.com" };
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(login)).ReturnsAsync(user);
            mapperMock.Setup(m => m.Map<AdminUserResponse>(user)).Returns(adminResponse);
            authServiceMock.Setup(a => a.GetUserRolesAsync(user)).ReturnsAsync(roles);
            // Act
            var result = await userManager.GetUserByInfoAsync(login);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo("adminuser@example.com"));
            Assert.That(result.Roles, Is.EqualTo(roles));
        }
        [Test]
        public void GetUserByUserInfoAsync_UserDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var login = "nonexistentuser";
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(login)).ReturnsAsync((User)null);
            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await userManager.GetUserByInfoAsync(login));
            Assert.That(ex.Message, Is.EqualTo("User is not found!"));
        }
        [Test]
        public async Task GetPaginatedUsersAsync_ValidRequest_ReturnsUsersWithRoles()
        {
            // Arrange
            var filter = new AdminGetUserFilter { PageNumber = 1, PageSize = 10 };
            var users = new List<User> { new User { Email = "user1@example.com" }, new User { Email = "user2@example.com" } };
            var roles = new List<string> { "User" };
            var userResponse = new AdminUserResponse { Email = "user1@example.com", Roles = roles };
            authServiceMock.Setup(a => a.GetPaginatedUsersAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync(users);
            mapperMock.Setup(m => m.Map<AdminUserResponse>(It.IsAny<User>())).Returns(userResponse);
            authServiceMock.Setup(a => a.GetUserRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            // Act
            var result = await userManager.GetPaginatedUsersAsync(filter, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Email, Is.EqualTo("user1@example.com"));
        }
        [Test]
        public async Task GetPaginatedUserTotalAmountAsync_ValidRequest_ReturnsTotalCount()
        {
            // Arrange
            var filter = new AdminGetUserFilter { PageNumber = 1, PageSize = 10 };
            var totalCount = 100;
            authServiceMock.Setup(a => a.GetUserTotalAmountAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync(totalCount);
            // Act
            var result = await userManager.GetPaginatedUserTotalAmountAsync(filter, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(totalCount));
        }
        [Test]
        public async Task RegisterUserAsync_ValidRequest_ReturnsUserAuthenticationResponse()
        {
            // Arrange
            var registrationRequest = new UserRegistrationRequest { Email = "testuser@example.com", Password = "Password123" };
            var user = new User { Email = "testuser@example.com" };
            var tokenData = new AccessTokenData { AccessToken = "token", RefreshToken = "refreshToken" };
            var authToken = new AuthToken { AccessToken = "token", RefreshToken = "refreshToken" };
            var roles = new List<string> { "User" };
            mapperMock.Setup(m => m.Map<User>(registrationRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>())).ReturnsAsync(IdentityResult.Success);
            authServiceMock.Setup(a => a.SetUserRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>())).ReturnsAsync(new List<IdentityError>());
            authServiceMock.Setup(a => a.LoginUserAsync(It.IsAny<LoginUserParams>())).ReturnsAsync(tokenData);
            mapperMock.Setup(m => m.Map<AuthToken>(tokenData)).Returns(authToken);
            authServiceMock.Setup(a => a.GetUserRolesAsync(user)).ReturnsAsync(roles);
            // Act
            var result = await userManager.RegisterUserAsync(registrationRequest);
            // Assert
            Assert.That(result.Email, Is.EqualTo("testuser@example.com"));
            Assert.That(result.AuthToken.AccessToken, Is.EqualTo("token"));
            Assert.That(result.Roles, Is.EqualTo(roles));
        }
        [Test]
        public void RegisterUserAsync_FailedRegistration_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var registrationRequest = new UserRegistrationRequest { Email = "testuser@example.com", Password = "Password123" };
            var user = new User { Email = "testuser@example.com" };
            var errors = new List<IdentityError> { new IdentityError { Description = "Email already exists" } };
            mapperMock.Setup(m => m.Map<User>(registrationRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>())).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await userManager.RegisterUserAsync(registrationRequest));
            Assert.That(ex.Errors.First(), Is.EqualTo("Email already exists"));
        }
        [Test]
        public async Task LoginUserAsync_ValidRequest_ReturnsUserAuthenticationResponse()
        {
            // Arrange
            var loginRequest = new UserAuthenticationRequest { Login = "testuser", Password = "Password123" };
            var user = new User { Email = "testuser@example.com" };
            var tokenData = new AccessTokenData { AccessToken = "token", RefreshToken = "refreshToken" };
            var authToken = new AuthToken { AccessToken = "token", RefreshToken = "refreshToken" };
            var roles = new List<string> { "User" };
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(loginRequest.Login)).ReturnsAsync(user);
            authServiceMock.Setup(a => a.LoginUserAsync(It.IsAny<LoginUserParams>())).ReturnsAsync(tokenData);
            mapperMock.Setup(m => m.Map<AuthToken>(tokenData)).Returns(authToken);
            authServiceMock.Setup(a => a.GetUserRolesAsync(user)).ReturnsAsync(roles);
            // Act
            var result = await userManager.LoginUserAsync(loginRequest);
            // Assert
            Assert.That(result.Email, Is.EqualTo("testuser@example.com"));
            Assert.That(result.AuthToken.AccessToken, Is.EqualTo("token"));
            Assert.That(result.Roles, Is.EqualTo(roles));
        }
        [Test]
        public void LoginUserAsync_InvalidCredentials_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var loginRequest = new UserAuthenticationRequest { Login = "invaliduser", Password = "wrongpassword" };
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(loginRequest.Login)).ReturnsAsync((User)null);
            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await userManager.LoginUserAsync(loginRequest));
        }
        [Test]
        public async Task UpdateUserAsync_ValidRequest_UpdatesUserSuccessfully()
        {
            // Arrange
            var updateRequest = new UserUpdateDataRequest { Email = "newemail@example.com" };
            var updateData = new UserUpdateData { Email = "newemail@example.com" };
            var user = new User { Email = "testuser@example.com" };
            mapperMock.Setup(m => m.Map<UserUpdateData>(updateRequest)).Returns(updateData);
            authServiceMock.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.UpdateUserAsync(user, updateData, false)).ReturnsAsync(new List<IdentityError>());
            // Act
            await userManager.UpdateUserAsync(updateRequest, new Mock<ClaimsPrincipal>().Object, CancellationToken.None);
            // Assert
            authServiceMock.Verify(a => a.UpdateUserAsync(user, updateData, false), Times.Once);
        }
        [Test]
        public void UpdateUserAsync_FailedUpdate_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var updateRequest = new UserUpdateDataRequest { Email = "newemail@example.com" };
            var updateData = new UserUpdateData { Email = "newemail@example.com" };
            var user = new User { Email = "testuser@example.com" };
            var errors = new List<IdentityError> { new IdentityError { Description = "Update failed" } };
            mapperMock.Setup(m => m.Map<UserUpdateData>(updateRequest)).Returns(updateData);
            authServiceMock.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.UpdateUserAsync(user, updateData, false)).ReturnsAsync(errors);
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await userManager.UpdateUserAsync(updateRequest, new Mock<ClaimsPrincipal>().Object, CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Update failed"));
        }
        [Test]
        public async Task RefreshTokenAsync_ValidRequest_ReturnsNewAuthToken()
        {
            // Arrange
            var token = new AuthToken { AccessToken = "oldToken", RefreshToken = "oldRefreshToken" };
            var tokenData = new AccessTokenData { AccessToken = "newToken", RefreshToken = "newRefreshToken" };
            var newAuthToken = new AuthToken { AccessToken = "newToken", RefreshToken = "newRefreshToken" };
            mapperMock.Setup(m => m.Map<AccessTokenData>(token)).Returns(tokenData);
            authServiceMock.Setup(a => a.RefreshTokenAsync(tokenData, It.IsAny<double>())).ReturnsAsync(tokenData);
            mapperMock.Setup(m => m.Map<AuthToken>(tokenData)).Returns(newAuthToken);
            // Act
            var result = await userManager.RefreshTokenAsync(token);
            // Assert
            Assert.That(result.AccessToken, Is.EqualTo("newToken"));
            Assert.That(result.RefreshToken, Is.EqualTo("newRefreshToken"));
        }
        [Test]
        public async Task AdminRegisterUserAsync_ValidRequest_ReturnsAdminUserResponse()
        {
            // Arrange
            var roles = new List<string> { "Admin" };
            var adminRequest = new AdminUserRegistrationRequest { Email = "admin@example.com", Password = "Password123", Roles = roles };
            var user = new User { Email = "admin@example.com" };
            var adminResponse = new AdminUserResponse { Email = "admin@example.com" };
            mapperMock.Setup(m => m.Map<User>(adminRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>())).ReturnsAsync(IdentityResult.Success);
            authServiceMock.Setup(a => a.SetUserRolesAsync(user, adminRequest.Roles)).ReturnsAsync(new List<IdentityError>());
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>())).ReturnsAsync(user);
            mapperMock.Setup(m => m.Map<AdminUserResponse>(user)).Returns(adminResponse);
            authServiceMock.Setup(a => a.GetUserRolesAsync(user)).ReturnsAsync(roles);
            // Act
            var result = await userManager.AdminRegisterUserAsync(adminRequest);
            // Assert
            Assert.That(result.Email, Is.EqualTo("admin@example.com"));
        }
        [Test]
        public void AdminRegisterUserAsync_FailedRegistration_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var adminRequest = new AdminUserRegistrationRequest { Email = "admin@example.com", Password = "Password123", Roles = new List<string> { "Admin" } };
            var user = new User { Email = "admin@example.com" };
            var errors = new List<IdentityError> { new IdentityError { Description = "Admin registration failed" } };
            mapperMock.Setup(m => m.Map<User>(adminRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>())).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await userManager.AdminRegisterUserAsync(adminRequest));
            Assert.That(ex.Errors.First(), Is.EqualTo("Admin registration failed"));
        }
        [Test]
        public async Task AdminUpdateUserAsync_ValidRequest_UpdatesUserAndRoles()
        {
            // Arrange
            var roles = new List<string> { "Admin" };
            var adminResponse = new AdminUserResponse { Email = "admin@example.com" };
            var updateRequest = new AdminUserUpdateDataRequest { CurrentLogin = "user1", Roles = new List<string> { "Admin" } };
            var user = new User { UserName = "user1" };
            var identityErrors = new List<IdentityError>();
            mapperMock.Setup(m => m.Map<UserUpdateData>(updateRequest)).Returns(new UserUpdateData());
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.UpdateUserAsync(It.IsAny<User>(), It.IsAny<UserUpdateData>(), true)).ReturnsAsync(identityErrors);
            authServiceMock.Setup(a => a.SetUserRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>())).ReturnsAsync(identityErrors);
            mapperMock.Setup(m => m.Map<AdminUserResponse>(user)).Returns(adminResponse);
            authServiceMock.Setup(a => a.GetUserRolesAsync(user)).ReturnsAsync(roles);
            // Act
            await userManager.AdminUpdateUserAsync(updateRequest, CancellationToken.None);
            // Assert
            authServiceMock.Verify(a => a.UpdateUserAsync(user, It.IsAny<UserUpdateData>(), true), Times.Once);
            authServiceMock.Verify(a => a.SetUserRolesAsync(user, updateRequest.Roles), Times.Once);
        }
        [Test]
        public void AdminUpdateUserAsync_FailedUpdate_ThrowsAuthorizationException()
        {
            // Arrange
            var updateRequest = new AdminUserUpdateDataRequest { CurrentLogin = "user1", Roles = new List<string> { "Admin" } };
            var user = new User { UserName = "user1" };
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Update failed" } };
            mapperMock.Setup(m => m.Map<UserUpdateData>(updateRequest)).Returns(new UserUpdateData());
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.UpdateUserAsync(It.IsAny<User>(), It.IsAny<UserUpdateData>(), true)).ReturnsAsync(identityErrors);
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await userManager.AdminUpdateUserAsync(updateRequest, CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Update failed"));
        }
        [Test]
        public async Task AdminDeleteUserAsync_ValidRequest_DeletesUserSuccessfully()
        {
            // Arrange
            var user = new User { UserName = "user1" };
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.DeleteUserAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            // Act
            await userManager.AdminDeleteUserAsync("user1");
            // Assert
            authServiceMock.Verify(a => a.DeleteUserAsync(user), Times.Once);
        }
        [Test]
        public void AdminDeleteUserAsync_FailedDelete_ThrowsAuthorizationException()
        {
            // Arrange
            var user = new User { UserName = "user1" };
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Deletion failed" } };
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.DeleteUserAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await userManager.AdminDeleteUserAsync("user1"));
            Assert.That(ex.Errors.First(), Is.EqualTo("Deletion failed"));
        }
    }
}