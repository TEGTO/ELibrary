using ExceptionHandling;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserApi.Command.Admin.AdminDeleteUser;
using UserApi.Command.Admin.AdminUpdateUser;
using UserApi.Command.Admin.GetPaginatedUsers;
using UserApi.Command.Admin.GetPaginatedUserTotalAmount;
using UserApi.Command.Admin.GetUserByInfo;
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
        public async Task DeleteUser_Valid_ReturnsOk()
        {
            // Act
            var result = await userController.DeleteUser(CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
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
            Assert.IsNotNull(okResult);

            Assert.That(okResult.Value, Is.EqualTo(adminResponse));
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
            Assert.IsNotNull(okResult);

            Assert.That(okResult.Value, Is.EqualTo(paginatedUsers));
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
            Assert.IsNotNull(okResult);

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
            Assert.IsNotNull(okResult);

            Assert.That(okResult.Value, Is.EqualTo(response));
            Assert.IsInstanceOf<AdminUserResponse>(okResult.Value);
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