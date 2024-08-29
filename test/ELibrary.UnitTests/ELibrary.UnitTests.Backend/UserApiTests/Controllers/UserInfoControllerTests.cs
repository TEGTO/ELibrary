using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Dtos;
using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Entities;
using UserApi.Services;

namespace UserApi.Controllers
{
    [TestFixture]
    internal class UserInfoControllerTests
    {
        private Mock<IUserInfoService> userInfoServiceMock;
        private Mock<IAuthService> authServiceMock;
        private Mock<IMapper> mapperMock;
        private UserInfoController controller;

        [SetUp]
        public void SetUp()
        {
            userInfoServiceMock = new Mock<IUserInfoService>();
            authServiceMock = new Mock<IAuthService>();
            mapperMock = new Mock<IMapper>();
            controller = new UserInfoController(userInfoServiceMock.Object, authServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task GetCurrentUser_ValidUser_ReturnsUserInfo()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "TestUser" };
            var userInfo = new UserInfo { Name = "John", LastName = "Doe", DateOfBirth = new DateTime(1985, 1, 1), Address = "123 Main St" };
            var userInfoDto = new UserInfoDto { Name = "John", LastName = "Doe", DateOfBirth = new DateTime(1985, 1, 1), Address = "123 Main St" };
            authServiceMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            userInfoServiceMock.Setup(x => x.GetUserInfoAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(userInfo);
            mapperMock.Setup(x => x.Map<UserInfoDto>(userInfo)).Returns(userInfoDto);
            // Act
            var result = await controller.GetCurrentUser(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as GetCurrentUserResponse;
            Assert.IsNotNull(response);
            Assert.That(response!.UserName, Is.EqualTo(user.UserName));
            Assert.That(response.UserInfo, Is.EqualTo(userInfoDto));
        }
        [Test]
        public async Task GetCurrentUser_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            authServiceMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((User)null);
            // Act
            var result = await controller.GetCurrentUser(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            var errorResponse = notFoundResult.Value as ResponseError;
            Assert.IsNotNull(errorResponse);
            Assert.That(errorResponse!.StatusCode, Is.EqualTo("404"));
            Assert.Contains("User not found.", errorResponse.Messages);
        }
    }
}