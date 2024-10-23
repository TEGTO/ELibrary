using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Exceptions;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserApi.Services.Auth;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Admin.AdminRegisterUser.Tests
{
    [TestFixture]
    internal class AdminRegisterUserCommandHandlerTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<IAuthService> authServiceMock;
        private Mock<IUserService> userService;
        private AdminRegisterUserCommandHandler adminRegisterUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();
            userService = new Mock<IUserService>();

            adminRegisterUserCommandHandler = new AdminRegisterUserCommandHandler(authServiceMock.Object, userService.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsAdminUserResponse()
        {
            // Arrange
            var roles = new List<string> { "Admin" };
            var adminRequest = new AdminUserRegistrationRequest { Email = "admin@example.com", Password = "Password123", Roles = roles };
            var user = new User { Email = "admin@example.com" };
            var adminResponse = new AdminUserResponse { Email = "admin@example.com" };
            mapperMock.Setup(m => m.Map<User>(adminRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success);
            userService.Setup(a => a.SetUserRolesAsync(user, adminRequest.Roles)).ReturnsAsync(new List<IdentityError>());
            userService.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>())).ReturnsAsync(user);
            mapperMock.Setup(m => m.Map<AdminUserResponse>(user)).Returns(adminResponse);
            userService.Setup(a => a.GetUserRolesAsync(user)).ReturnsAsync(roles);
            // Act
            var result = await adminRegisterUserCommandHandler.Handle(new AdminRegisterUserCommand(adminRequest), CancellationToken.None);
            // Assert
            Assert.That(result.Email, Is.EqualTo("admin@example.com"));
        }
        [Test]
        public void Handle_FailedRegistration_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var adminRequest = new AdminUserRegistrationRequest { Email = "admin@example.com", Password = "Password123", Roles = new List<string> { "Admin" } };
            var user = new User { Email = "admin@example.com" };
            var errors = new List<IdentityError> { new IdentityError { Description = "Admin registration failed" } };
            mapperMock.Setup(m => m.Map<User>(adminRequest)).Returns(user);
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await adminRegisterUserCommandHandler.Handle(new AdminRegisterUserCommand(adminRequest), CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Admin registration failed"));
        }
    }
}