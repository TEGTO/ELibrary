﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Exceptions;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Admin.AdminRegisterUser.Tests
{
    [TestFixture]
    internal class AdminRegisterUserCommandHandlerTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<IAuthService> authServiceMock;
        private AdminRegisterUserCommandHandler adminRegisterUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();

            adminRegisterUserCommandHandler = new AdminRegisterUserCommandHandler(authServiceMock.Object, mapperMock.Object);
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
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>())).ReturnsAsync(IdentityResult.Success);
            authServiceMock.Setup(a => a.SetUserRolesAsync(user, adminRequest.Roles)).ReturnsAsync(new List<IdentityError>());
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>())).ReturnsAsync(user);
            mapperMock.Setup(m => m.Map<AdminUserResponse>(user)).Returns(adminResponse);
            authServiceMock.Setup(a => a.GetUserRolesAsync(user)).ReturnsAsync(roles);
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
            authServiceMock.Setup(a => a.RegisterUserAsync(It.IsAny<RegisterUserParams>())).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await adminRegisterUserCommandHandler.Handle(new AdminRegisterUserCommand(adminRequest), CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Admin registration failed"));
        }
    }
}