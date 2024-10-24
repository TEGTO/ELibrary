using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserApi.Command.Admin.AdminRegisterUser;
using UserApi.Command.Client.GetOAuthUrl;
using UserApi.Command.Client.LoginOAuth;
using UserApi.Command.Client.LoginUser;
using UserApi.Command.Client.RefreshToken;
using UserApi.Command.Client.RegisterUser;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Controllers.Tests
{
    [TestFixture]
    internal class AuthControllerTests
    {
        private Mock<IMediator> mediatorMock;
        private AuthController authController;

        [SetUp]
        public void SetUp()
        {
            mediatorMock = new Mock<IMediator>();
            authController = new AuthController(mediatorMock.Object);
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
            var result = await authController.Register(registrationRequest, CancellationToken.None);
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
            var result = await authController.Login(loginRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(userAuthResponse));
        }
        [Test]
        public async Task GetOAuthUrl_ValidRequest_ReturnsOkWithResponse()
        {
            // Arrange
            var queryParams = new GetOAuthUrlQueryParams();
            var response = new GetOAuthUrlResponse { Url = "someurl" };
            mediatorMock.Setup(m => m.Send(It.IsAny<GetOAuthUrlQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(response);
            // Act
            var result = await authController.GetOAuthUrl(queryParams, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(response));
        }
        [Test]
        public async Task LoginOAuth_ValidRequest_ReturnsOkWithResponse()
        {
            // Arrange
            var request = new LoginOAuthRequest();
            var response = new UserAuthenticationResponse { Email = "someemail" };
            mediatorMock.Setup(m => m.Send(It.IsAny<LoginOAuthCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(response);
            // Act
            var result = await authController.LoginOAuth(request, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(response));
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
            var result = await authController.Refresh(token, CancellationToken.None);
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
            mediatorMock.Setup(m => m.Send(It.IsAny<AdminRegisterUserCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(adminResponse);
            // Act
            var result = await authController.AdminRegister(adminRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(adminResponse));
        }
    }
}