using AutoMapper;
using ExceptionHandling;
using Microsoft.AspNetCore.Identity;
using Moq;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Models;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Admin.AdminUpdateUser.Tests
{
    [TestFixture]
    internal class AdminUpdateUserCommandHandlerTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<IUserService> userServiceMock;
        private AdminUpdateUserCommandHandler adminUpdateUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            userServiceMock = new Mock<IUserService>();

            adminUpdateUserCommandHandler = new AdminUpdateUserCommandHandler(userServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_UpdatesUserAndRoles()
        {
            // Arrange
            var roles = new List<string> { "Admin" };
            var user = new User { UserName = "user1" };
            var identityErrors = new List<IdentityError>();

            var adminResponse = new AdminUserResponse { Email = "admin@example.com" };
            var updateRequest = new AdminUserUpdateDataRequest { CurrentLogin = "user1", Roles = new List<string> { "Admin" } };

            userServiceMock.Setup(a => a.GetUserByLoginAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            userServiceMock.Setup(a => a.UpdateUserAsync(It.IsAny<User>(), It.IsAny<UserUpdateModel>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(identityErrors);
            userServiceMock.Setup(a => a.SetUserRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>(), It.IsAny<CancellationToken>())).ReturnsAsync(identityErrors);
            userServiceMock.Setup(a => a.GetUserRolesAsync(user, CancellationToken.None)).ReturnsAsync(roles);

            mapperMock.Setup(m => m.Map<UserUpdateModel>(updateRequest)).Returns(new UserUpdateModel() { Email = "", UserName = "" });
            mapperMock.Setup(m => m.Map<AdminUserResponse>(user)).Returns(adminResponse);

            // Act
            await adminUpdateUserCommandHandler.Handle(new AdminUpdateUserCommand(updateRequest), CancellationToken.None);

            // Assert
            userServiceMock.Verify(a => a.UpdateUserAsync(user, It.IsAny<UserUpdateModel>(), true, It.IsAny<CancellationToken>()), Times.Once);
            userServiceMock.Verify(a => a.SetUserRolesAsync(user, updateRequest.Roles, CancellationToken.None), Times.Once);
        }

        [Test]
        public void Handle_FailedUpdate_ThrowsAuthorizationException()
        {
            // Arrange
            var updateRequest = new AdminUserUpdateDataRequest { CurrentLogin = "user1", Roles = new List<string> { "Admin" } };
            var user = new User { UserName = "user1" };
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Update failed" } };

            mapperMock.Setup(m => m.Map<UserUpdateModel>(updateRequest)).Returns(new UserUpdateModel() { Email = "", UserName = "" });

            userServiceMock.Setup(a => a.GetUserByLoginAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            userServiceMock.Setup(a => a.UpdateUserAsync(It.IsAny<User>(), It.IsAny<UserUpdateModel>(), true, It.IsAny<CancellationToken>())).ReturnsAsync(identityErrors);

            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await adminUpdateUserCommandHandler.Handle(new AdminUpdateUserCommand(updateRequest), CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Update failed"));
        }
    }
}