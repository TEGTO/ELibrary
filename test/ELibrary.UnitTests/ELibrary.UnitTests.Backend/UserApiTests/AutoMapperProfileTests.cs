using Authentication.Models;
using AutoMapper;
using UserApi;
using UserApi.Domain.Dtos;
using UserApi.Domain.Entities;

namespace UserApiTests
{
    [TestFixture]
    internal class AutoMapperProfileTests
    {
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            mapper = config.CreateMapper();
        }

        [Test]
        public void UserToUserRegistrationRequest_MapsCorrectly()
        {
            // Arrange
            var user = new User
            {
                UserName = "TestUser",
                RefreshToken = "refreshToken",
                RefreshTokenExpiryTime = DateTime.Now.AddDays(1),
                UserInfo = new UserInfo
                {
                    Name = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1985, 1, 1),
                    Address = "123 Main St"
                }
            };
            // Act
            var result = mapper.Map<UserRegistrationRequest>(user);
            // Assert
            Assert.That(result.UserName, Is.EqualTo(user.UserName));
            Assert.That(result.UserInfo.Name, Is.EqualTo(user.UserInfo.Name));
            Assert.That(result.UserInfo.LastName, Is.EqualTo(user.UserInfo.LastName));
            Assert.That(result.UserInfo.DateOfBirth, Is.EqualTo(user.UserInfo.DateOfBirth));
            Assert.That(result.UserInfo.Address, Is.EqualTo(user.UserInfo.Address));
        }
        [Test]
        public void AccessTokenDataToAuthToken_MapsCorrectly()
        {
            // Arrange
            var accessTokenData = new AccessTokenData
            {
                AccessToken = "access_token",
                RefreshToken = "refresh_token",
                RefreshTokenExpiryDate = DateTime.Now.AddMinutes(30)
            };
            // Act
            var result = mapper.Map<AuthToken>(accessTokenData);
            // Assert
            Assert.That(result.AccessToken, Is.EqualTo(accessTokenData.AccessToken));
            Assert.That(result.RefreshToken, Is.EqualTo(accessTokenData.RefreshToken));
            Assert.That(result.RefreshTokenExpiryDate, Is.EqualTo(accessTokenData.RefreshTokenExpiryDate));
        }
        [Test]
        public void UserInfoToUserInfoDto_MapsCorrectly()
        {
            // Arrange
            var userInfo = new UserInfo
            {
                Name = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1985, 1, 1),
                Address = "123 Main St"
            };
            // Act
            var result = mapper.Map<UserInfoDto>(userInfo);
            // Assert
            Assert.That(result.Name, Is.EqualTo(userInfo.Name));
            Assert.That(result.LastName, Is.EqualTo(userInfo.LastName));
            Assert.That(result.DateOfBirth, Is.EqualTo(userInfo.DateOfBirth));
            Assert.That(result.Address, Is.EqualTo(userInfo.Address));
        }
        [Test]
        public void UserToUserRegistrationRequest_Null()
        {
            // Arrange
            User? user = null;
            // Act & Assert
            var result = mapper.Map<UserRegistrationRequest>(user);
            Assert.That(result, Is.Null);
        }
        [Test]
        public void AccessTokenDataToAuthToken_Null()
        {
            // Arrange
            AccessTokenData? accessTokenData = null;
            // Act & Assert
            var result = mapper.Map<AuthToken>(accessTokenData);
            Assert.That(result, Is.Null);
        }
    }
}
