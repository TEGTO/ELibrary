﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Exceptions;
using System.Security.Claims;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Models;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.UpdateUser.Tests
{
    [TestFixture]
    internal class UpdateUserCommandHandlerTests
    {
        private Mock<IAuthService> authServiceMock;
        private Mock<IMapper> mapperMock;
        private UpdateUserCommandHandler updateUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();

            updateUserCommandHandler = new UpdateUserCommandHandler(authServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_UpdatesUserSuccessfully()
        {
            // Arrange
            var updateRequest = new UserUpdateDataRequest { Email = "newemail@example.com" };
            var updateData = new UserUpdateData { Email = "newemail@example.com" };
            var user = new User { Email = "testuser@example.com" };
            mapperMock.Setup(m => m.Map<UserUpdateData>(updateRequest)).Returns(updateData);
            authServiceMock.Setup(a => a.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.UpdateUserAsync(user, updateData, false)).ReturnsAsync(new List<IdentityError>());
            // Act
            await updateUserCommandHandler.Handle(new UpdateUserCommand(updateRequest, new Mock<ClaimsPrincipal>().Object), CancellationToken.None);
            // Assert
            authServiceMock.Verify(a => a.UpdateUserAsync(user, updateData, false), Times.Once);
        }
        [Test]
        public void Handle_FailedUpdate_ThrowsUnauthorizedAccessException()
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
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await updateUserCommandHandler.Handle(new UpdateUserCommand(updateRequest, new Mock<ClaimsPrincipal>().Object), CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Update failed"));
        }
    }
}