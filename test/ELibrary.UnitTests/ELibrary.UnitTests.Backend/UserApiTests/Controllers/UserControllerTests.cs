using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;

namespace UserApi.Controllers.Tests
{
    [TestFixture]
    internal class UserControllerTests
    {
        private Mock<IUserManager> userManagerMock;
        private UserController userController;

        [SetUp]
        public void SetUp()
        {
            userManagerMock = new Mock<IUserManager>();
            userController = new UserController(userManagerMock.Object);
        }

        [Test]
        public async Task Register_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var registrationRequest = new UserRegistrationRequest { Email = "testuser@example.com", Password = "Password123", ConfirmPassword = "Password123" };
            var userAuthResponse = new UserAuthenticationResponse { Email = "testuser@example.com", Roles = new List<string> { "User" } };
            userManagerMock.Setup(m => m.RegisterUserAsync(It.IsAny<UserRegistrationRequest>()))
                           .ReturnsAsync(userAuthResponse);
            // Act
            var result = await userController.Register(registrationRequest);
            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.That(createdAtActionResult?.RouteValues["id"], Is.EqualTo(userAuthResponse.Email));
        }
        [Test]
        public async Task Login_ValidRequest_ReturnsOk()
        {
            // Arrange
            var loginRequest = new UserAuthenticationRequest { Login = "testuser", Password = "Password123" };
            var userAuthResponse = new UserAuthenticationResponse { Email = "testuser@example.com", Roles = new List<string> { "User" } };
            userManagerMock.Setup(m => m.LoginUserAsync(It.IsAny<UserAuthenticationRequest>()))
                           .ReturnsAsync(userAuthResponse);
            // Act
            var result = await userController.Login(loginRequest);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(userAuthResponse));
        }
        [Test]
        public void Login_InvalidCredentials_ThrowsException()
        {
            // Arrange
            var loginRequest = new UserAuthenticationRequest { Login = "invaliduser", Password = "wrongpassword" };
            userManagerMock.Setup(m => m.LoginUserAsync(It.IsAny<UserAuthenticationRequest>()))
                           .ThrowsAsync(new UnauthorizedAccessException());
            // Act + Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await userController.Login(loginRequest));
        }
        [Test]
        public async Task Update_ValidRequest_ReturnsOk()
        {
            // Arrange
            var updateRequest = new UserUpdateDataRequest { Email = "newemail@example.com", OldPassword = "oldpass", Password = "newpass" };
            // Act
            var result = await userController.Update(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            userManagerMock.Verify(m => m.UpdateUserAsync(It.IsAny<UserUpdateDataRequest>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task Refresh_ValidToken_ReturnsOk()
        {
            // Arrange
            var token = new AuthToken { AccessToken = "token", RefreshToken = "refreshToken" };
            var refreshedToken = new AuthToken { AccessToken = "newToken", RefreshToken = "newRefreshToken" };
            userManagerMock.Setup(m => m.RefreshTokenAsync(It.IsAny<AuthToken>())).ReturnsAsync(refreshedToken);
            // Act
            var result = await userController.Refresh(token);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(refreshedToken));
        }
        [Test]
        public async Task AdminRegister_ValidRequest_ReturnsOk()
        {
            // Arrange
            var adminRequest = new AdminUserRegistrationRequest { Email = "adminuser@example.com", Password = "Password123", Roles = new List<string> { "Admin" } };
            var adminResponse = new AdminUserResponse { Email = "adminuser@example.com" };

            userManagerMock.Setup(m => m.AdminRegisterUserAsync(It.IsAny<AdminUserRegistrationRequest>()))
                           .ReturnsAsync(adminResponse);
            // Act
            var result = await userController.AdminRegister(adminRequest);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(adminResponse));
        }
        [Test]
        public async Task AdminGetUserByLogin_ValidUser_ReturnsOk()
        {
            // Arrange
            var login = "adminuser";
            var adminResponse = new AdminUserResponse { Email = "adminuser@example.com" };
            userManagerMock.Setup(m => m.GetUserByLoginAsync(login))
                           .ReturnsAsync(adminResponse);
            // Act
            var result = await userController.AdminGetUserByLogin(login);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(adminResponse));
        }
        [Test]
        public async Task AdminDelete_ValidUser_ReturnsOk()
        {
            // Arrange
            var login = "adminuser";

            userManagerMock.Setup(m => m.AdminDeleteUserAsync(login))
                           .Returns(Task.CompletedTask);
            // Act
            var result = await userController.AdminDelete(login);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            userManagerMock.Verify(m => m.AdminDeleteUserAsync(login), Times.Once);
        }
        [Test]
        public void AdminDelete_InvalidUser_ThorwsException()
        {
            // Arrange
            var login = "adminuser";
            userManagerMock.Setup(m => m.AdminDeleteUserAsync(login))
                           .ThrowsAsync(new InvalidOperationException("Delete failed"));
            // Act + Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await userController.AdminDelete(login));
        }
    }
}