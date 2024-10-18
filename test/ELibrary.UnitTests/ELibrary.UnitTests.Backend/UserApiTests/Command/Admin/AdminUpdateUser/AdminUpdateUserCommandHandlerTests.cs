using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Exceptions;
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
        private Mock<IAuthService> authServiceMock;
        private AdminUpdateUserCommandHandler adminUpdateUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();

            adminUpdateUserCommandHandler = new AdminUpdateUserCommandHandler(authServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_UpdatesUserAndRoles()
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
            await adminUpdateUserCommandHandler.Handle(new AdminUpdateUserCommand(updateRequest), CancellationToken.None);
            // Assert
            authServiceMock.Verify(a => a.UpdateUserAsync(user, It.IsAny<UserUpdateData>(), true), Times.Once);
            authServiceMock.Verify(a => a.SetUserRolesAsync(user, updateRequest.Roles), Times.Once);
        }
        [Test]
        public void Handle_FailedUpdate_ThrowsAuthorizationException()
        {
            // Arrange
            var updateRequest = new AdminUserUpdateDataRequest { CurrentLogin = "user1", Roles = new List<string> { "Admin" } };
            var user = new User { UserName = "user1" };
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Update failed" } };
            mapperMock.Setup(m => m.Map<UserUpdateData>(updateRequest)).Returns(new UserUpdateData());
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.UpdateUserAsync(It.IsAny<User>(), It.IsAny<UserUpdateData>(), true)).ReturnsAsync(identityErrors);
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await adminUpdateUserCommandHandler.Handle(new AdminUpdateUserCommand(updateRequest), CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Update failed"));
        }
    }
}