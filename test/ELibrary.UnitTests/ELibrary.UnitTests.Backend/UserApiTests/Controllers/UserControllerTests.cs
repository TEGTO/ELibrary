using Authentication.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Shared.Dtos;
using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Models;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Controllers.Tests
{
    [TestFixture]
    internal class UserControllerTests
    {
        private const int EXPIRY_IN_DAYS = 7;

        private Mock<IMapper> mapperMock;
        private Mock<IAuthService> authServiceMock;
        private Mock<IConfiguration> configurationMock;
        private UserController userController;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();
            configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["AuthSettings:RefreshExpiryInDays"]).Returns(EXPIRY_IN_DAYS.ToString());
            userController = new UserController(mapperMock.Object, authServiceMock.Object, configurationMock.Object);
        }

        [Test]
        public async Task Register_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var registrationRequest = new UserRegistrationRequest { Email = "testuser@example.com", Password = "Password123", ConfirmPassword = "Password123" };
            var user = new User { Id = "1", UserName = "testuser", Email = "testuser@example.com" };
            var registerResult = IdentityResult.Success;
            mapperMock.Setup(m => m.Map<User>(registrationRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>())).ReturnsAsync(registerResult);
            authServiceMock.Setup(a => a.SetUserRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>())).ReturnsAsync(registerResult.Errors.ToList());
            // Act
            var result = await userController.Register(registrationRequest);
            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        }
        [Test]
        public async Task Register_FailedRegistration_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var registrationRequest = new UserRegistrationRequest { Email = "testuser@example.com", Password = "Password123", ConfirmPassword = "Password123" };
            var user = new User { Id = "1", UserName = "testuser", Email = "testuser@example.com" };
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Error during registration" } };
            mapperMock.Setup(m => m.Map<User>(registrationRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>())).ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));
            // Act
            var result = await userController.Register(registrationRequest);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            var responseError = badRequestResult.Value as ResponseError;
            Assert.That(responseError.Messages[0], Is.EqualTo("Error during registration"));
        }
        [Test]
        public async Task Login_ValidRequest_ReturnsOk()
        {
            // Arrange
            var loginRequest = new UserAuthenticationRequest { Login = "testuser", Password = "Password123" };
            var tokenData = new AccessTokenData { AccessToken = "token", RefreshToken = "refreshToken", RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(EXPIRY_IN_DAYS) };
            var authToken = new AuthToken { AccessToken = "token", RefreshToken = "refreshToken", RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(EXPIRY_IN_DAYS) };
            var user = new User { UserName = "testuser", Email = "testuser@example.com" };
            var roles = new List<string> { "Admin", "User" };
            authServiceMock.Setup(a => a.LoginUserAsync(It.IsAny<LoginUserParams>())).ReturnsAsync(tokenData);
            mapperMock.Setup(m => m.Map<AuthToken>(It.IsAny<AccessTokenData>())).Returns(authToken);
            authServiceMock.Setup(a => a.GetUserByLoginAsync(It.IsAny<string>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.GetUserRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            // Act
            var result = await userController.Login(loginRequest);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var response = okResult.Value as UserAuthenticationResponse;
            Assert.That(response.AuthToken, Is.EqualTo(authToken));
            Assert.That(response.Email, Is.EqualTo(user.Email));
            Assert.That(response.Roles, Is.EqualTo(roles));
        }
        [Test]
        public async Task Login_InvalidCredentials_ReturnsNotFound()
        {
            // Arrange
            var loginRequest = new UserAuthenticationRequest { Login = "invaliduser", Password = "wrongpassword" };
            authServiceMock.Setup(a => a.GetUserByLoginAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            // Act
            var result = await userController.Login(loginRequest);
            // Assert
            Assert.IsInstanceOf<UnauthorizedResult>(result.Result);
        }
        [Test]
        public async Task Update_ValidRequest_ReturnsOk()
        {
            // Arrange
            var updateRequest = new UserUpdateDataRequest { Email = "newemail@example.com", OldPassword = "oldpass", Password = "newpass" };
            var updateData = new UserUpdateData { Email = "newemail@example.com", OldPassword = "oldpass", Password = "newpass" };
            var user = new User { Email = "testuser@example.com" };
            var identityResult = IdentityResult.Success;
            mapperMock.Setup(m => m.Map<UserUpdateData>(updateRequest)).Returns(updateData);
            authServiceMock.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.UpdateUserAsync(user, updateData, false)).ReturnsAsync(identityResult.Errors.ToList());
            // Act
            var result = await userController.Update(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task Update_FailedUpdate_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var updateRequest = new UserUpdateDataRequest { Email = "newemail@example.com", OldPassword = "oldpass", Password = "newpass" };
            var updateData = new UserUpdateData { Email = "newemail@example.com", OldPassword = "oldpass", Password = "newpass" };
            var user = new User { Email = "testuser@example.com" };
            var identityErrors = IdentityResult.Failed(new IdentityError { Description = "Update failed" });
            mapperMock.Setup(m => m.Map<UserUpdateData>(updateRequest)).Returns(updateData);
            authServiceMock.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.UpdateUserAsync(user, updateData, false)).ReturnsAsync(identityErrors.Errors.ToList());
            // Act
            var result = await userController.Update(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            var responseError = badRequestResult.Value as ResponseError;
            Assert.That(responseError.Messages[0], Is.EqualTo("Update failed"));
        }
        [Test]
        public async Task AdminRegister_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var adminRequest = new AdminUserRegistrationRequest { Email = "adminuser@example.com", Password = "Password123", Roles = new List<string> { "Admin" } };
            var user = new User { Id = "1", UserName = "adminuser", Email = "adminuser@example.com" };
            var roles = new List<string> { "Admin", "User" };
            var identityResult = IdentityResult.Success;
            var adminUserResponse = new AdminUserResponse { Id = "1", UserName = "adminuser", Email = "adminuser@example.com" };
            mapperMock.Setup(m => m.Map<User>(adminRequest)).Returns(user);
            mapperMock.Setup(m => m.Map<AdminUserResponse>(user)).Returns(adminUserResponse);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>())).ReturnsAsync(identityResult);
            authServiceMock.Setup(a => a.SetUserRolesAsync(user, adminRequest.Roles)).ReturnsAsync(identityResult.Errors.ToList());
            authServiceMock.Setup(a => a.GetUserByLoginAsync(It.IsAny<string>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.GetUserRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            // Act
            var result = await userController.AdminRegister(adminRequest);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }
        [Test]
        public async Task AdminRegister_FailedRegistration_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var adminRequest = new AdminUserRegistrationRequest { Email = "adminuser@example.com", Password = "Password123", Roles = new List<string> { "Admin" } };
            var user = new User { Id = "1", UserName = "adminuser", Email = "adminuser@example.com" };
            var identityErrors = IdentityResult.Failed(new IdentityError { Description = "Registration failed" });
            mapperMock.Setup(m => m.Map<User>(adminRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>())).ReturnsAsync(identityErrors);
            // Act
            var result = await userController.AdminRegister(adminRequest);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            var responseError = badRequestResult.Value as ResponseError;
            Assert.That(responseError.Messages[0], Is.EqualTo("Registration failed"));
        }
        [Test]
        public async Task AdminGetUserByLogin_ValidUser_ReturnsOk()
        {
            // Arrange
            var login = "adminuser";
            var user = new User { UserName = login, Email = "adminuser@example.com" };
            var adminUserResponse = new AdminUserResponse { Id = "1", UserName = login, Email = "adminuser@example.com" };
            mapperMock.Setup(m => m.Map<AdminUserResponse>(user)).Returns(adminUserResponse);
            authServiceMock.Setup(a => a.GetUserByLoginAsync(login)).ReturnsAsync(user);
            authServiceMock.Setup(a => a.GetUserRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "Admin" });
            // Act
            var result = await userController.AdminGetUserByLogin(login);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var response = okResult.Value as AdminUserResponse;
            Assert.That(response.Email, Is.EqualTo("adminuser@example.com"));
        }
        [Test]
        public async Task AdminGetUserByLogin_NonExistentUser_ReturnsNotFound()
        {
            // Arrange
            var login = "nonexistentuser";
            authServiceMock.Setup(a => a.GetUserByLoginAsync(login)).ReturnsAsync((User)null);
            // Act
            var result = await userController.AdminGetUserByLogin(login);
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task AdminUpdate_ValidRequest_ReturnsOk()
        {
            // Arrange
            var updateRequest = new AdminUserUpdateDataRequest { CurrentLogin = "adminuser", Password = "newpass", Roles = new List<string> { "Admin" } };
            var updateData = new UserUpdateData { Password = "newpass" };
            var user = new User { UserName = "adminuser", Email = "adminuser@example.com" };
            var identityResult = IdentityResult.Success;
            mapperMock.Setup(m => m.Map<UserUpdateData>(updateRequest)).Returns(updateData);
            authServiceMock.Setup(a => a.GetUserByLoginAsync(updateRequest.CurrentLogin)).ReturnsAsync(user);
            authServiceMock.Setup(a => a.UpdateUserAsync(user, updateData, true)).ReturnsAsync(identityResult.Errors.ToList());
            authServiceMock.Setup(a => a.SetUserRolesAsync(user, updateRequest.Roles)).ReturnsAsync(identityResult.Errors.ToList());
            // Act
            var result = await userController.AdminUpdate(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task AdminUpdate_FailedUpdate_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var updateRequest = new AdminUserUpdateDataRequest { CurrentLogin = "adminuser", Password = "newpass", Roles = new List<string> { "Admin" } };
            var updateData = new UserUpdateData { Password = "newpass" };
            var user = new User { UserName = "adminuser", Email = "adminuser@example.com" };
            var identityErrors = IdentityResult.Failed(new IdentityError { Description = "Update failed" });
            mapperMock.Setup(m => m.Map<UserUpdateData>(updateRequest)).Returns(updateData);
            authServiceMock.Setup(a => a.GetUserByLoginAsync(updateRequest.CurrentLogin)).ReturnsAsync(user);
            authServiceMock.Setup(a => a.UpdateUserAsync(user, updateData, true)).ReturnsAsync(identityErrors.Errors.ToList());
            // Act
            var result = await userController.AdminUpdate(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            var responseError = badRequestResult.Value as ResponseError;
            Assert.That(responseError.Messages[0], Is.EqualTo("Update failed"));
        }
        [Test]
        public async Task AdminDelete_ValidUser_ReturnsOk()
        {
            // Arrange
            var login = "adminuser";
            var user = new User { UserName = login, Email = "adminuser@example.com" };
            var deleteResult = IdentityResult.Success;
            authServiceMock.Setup(a => a.GetUserByLoginAsync(login)).ReturnsAsync(user);
            authServiceMock.Setup(a => a.DeleteUserAsync(user)).ReturnsAsync(deleteResult);
            // Act
            var result = await userController.AdminDelete(login);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task AdminDelete_FailedDeletion_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var login = "adminuser";
            var user = new User { UserName = login, Email = "adminuser@example.com" };
            var identityErrors = IdentityResult.Failed(new IdentityError { Description = "Delete failed" });

            authServiceMock.Setup(a => a.GetUserByLoginAsync(login)).ReturnsAsync(user);
            authServiceMock.Setup(a => a.DeleteUserAsync(user)).ReturnsAsync(identityErrors);
            // Act
            var result = await userController.AdminDelete(login);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            var responseError = badRequestResult.Value as ResponseError;
            Assert.That(responseError.Messages[0], Is.EqualTo("Delete failed"));
        }
    }
}