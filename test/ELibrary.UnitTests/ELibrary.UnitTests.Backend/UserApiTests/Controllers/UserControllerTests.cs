using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Exceptions;
using UserApi.Command.Admin.AdminDeleteUser;
using UserApi.Command.Admin.AdminRegisterUser;
using UserApi.Command.Admin.AdminUpdateUser;
using UserApi.Command.Admin.GetPaginatedUsers;
using UserApi.Command.Admin.GetPaginatedUserTotalAmount;
using UserApi.Command.Admin.GetUserByInfo;
using UserApi.Command.Client.LoginUser;
using UserApi.Command.Client.RefreshToken;
using UserApi.Command.Client.RegisterUser;
using UserApi.Command.Client.UpdateUser;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Controllers.Tests
{
    [TestFixture]
    internal class UserControllerTests
    {
        private Mock<IMediator> mediatorMock;
        private UserController userController;

        [SetUp]
        public void SetUp()
        {
            mediatorMock = new Mock<IMediator>();
            userController = new UserController(mediatorMock.Object);
        }

        [Test]
        public async Task Register_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var registrationRequest = new UserRegistrationRequest { Email = "testuser@example.com", Password = "Password123", ConfirmPassword = "Password123" };
            var userAuthResponse = new UserAuthenticationResponse { Email = "testuser@example.com", Roles = new List<string> { "User" } };
            mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(userAuthResponse);
            // Act
            var result = await userController.Register(registrationRequest, CancellationToken.None);
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
            mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(userAuthResponse);
            // Act
            var result = await userController.Login(loginRequest, CancellationToken.None);
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
            mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new UnauthorizedAccessException());
            // Act + Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await userController.Login(loginRequest, CancellationToken.None));
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
            mediatorMock.Verify(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task Refresh_ValidToken_ReturnsOk()
        {
            // Arrange
            var token = new AuthToken { AccessToken = "token", RefreshToken = "refreshToken" };
            var refreshedToken = new AuthToken { AccessToken = "newToken", RefreshToken = "newRefreshToken" };
            mediatorMock.Setup(m => m.Send(It.IsAny<RefreshTokenCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(refreshedToken);
            // Act
            var result = await userController.Refresh(token, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(refreshedToken));
        }
        [Test]
        public async Task DeleteUser_Valid_ReturnsOk()
        {
            // Act
            var result = await userController.DeleteUser(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task AdminRegister_ValidRequest_ReturnsOk()
        {
            // Arrange
            var adminRequest = new AdminUserRegistrationRequest { Email = "adminuser@example.com", Password = "Password123", Roles = new List<string> { "Admin" } };
            var adminResponse = new AdminUserResponse { Email = "adminuser@example.com" };
            mediatorMock.Setup(m => m.Send(It.IsAny<AdminRegisterUserCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(adminResponse);
            // Act
            var result = await userController.AdminRegister(adminRequest, CancellationToken.None);
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
            mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByInfoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(adminResponse);
            // Act
            var result = await userController.AdminGetUser(login, CancellationToken.None);
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
            // Act
            var result = await userController.AdminDeleteUser(login, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            mediatorMock.Verify(m => m.Send(It.IsAny<AdminDeleteUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void AdminDelete_InvalidUser_ThrowsException()
        {
            // Arrange
            var login = "adminuser";
            mediatorMock.Setup(m => m.Send(It.IsAny<AdminDeleteUserCommand>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new InvalidOperationException("Delete failed"));
            // Act + Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await userController.AdminDeleteUser(login, CancellationToken.None));
        }
        [Test]
        public async Task AdminGetPaginatedUsers_ValidRequest_ReturnsOk()
        {
            // Arrange
            var filter = new AdminGetUserFilter { PageNumber = 1, PageSize = 10 };
            var paginatedUsers = new List<AdminUserResponse>
            {
                new AdminUserResponse { Email = "user1@example.com" },
                new AdminUserResponse { Email = "user2@example.com" }
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<GetPaginatedUsersQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(paginatedUsers);
            // Act
            var result = await userController.AdminGetPaginatedUsers(filter, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(paginatedUsers));
        }
        [Test]
        public async Task AdminGetPaginatedUserAmount_ValidRequest_ReturnsOk()
        {
            // Arrange
            var filter = new AdminGetUserFilter { PageNumber = 1, PageSize = 10 };
            var totalUsers = 100;
            mediatorMock.Setup(m => m.Send(It.IsAny<GetPaginatedUserTotalAmountQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(totalUsers);
            // Act
            var result = await userController.AdminGetPaginatedUserAmount(filter, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(totalUsers));
        }
        [Test]
        public async Task AdminUpdate_ValidRequest_ReturnsOk()
        {
            // Arrange
            var updateRequest = new AdminUserUpdateDataRequest { CurrentLogin = "user1", Roles = new List<string> { "Admin" } };
            var response = new AdminUserResponse();
            mediatorMock.Setup(m => m.Send(It.IsAny<AdminUpdateUserCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(response);
            // Act
            var result = await userController.AdminUpdate(updateRequest, CancellationToken.None);

            // Assert
            mediatorMock.Verify(x => x.Send(It.IsAny<AdminUpdateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsInstanceOf<AdminUserResponse>(okResult?.Value);
            Assert.That(okResult?.Value, Is.EqualTo(response));
        }
        [Test]
        public void AdminUpdate_InvalidRequest_ThrowsAuthorizationException()
        {
            // Arrange
            var updateRequest = new AdminUserUpdateDataRequest { CurrentLogin = "user1", Roles = new List<string> { "Admin" } };
            mediatorMock.Setup(m => m.Send(It.IsAny<AdminUpdateUserCommand>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new AuthorizationException(["Authorization failed."]));
            // Act & Assert
            Assert.ThrowsAsync<AuthorizationException>(async () => await userController.AdminUpdate(updateRequest, CancellationToken.None));
        }
    }
}