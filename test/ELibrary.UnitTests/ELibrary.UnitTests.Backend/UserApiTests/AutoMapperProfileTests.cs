using Authentication.Models;
using AutoMapper;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserEntities.Domain.Entities;

namespace UserApi.Tests
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
        public void UserRegistrationRequestToUser_MapsCorrectly()
        {
            // Arrange
            var request = new UserRegistrationRequest
            {
                Email = "Email",
                Password = "password",
                ConfirmPassword = "password",
            };

            // Act
            var result = mapper.Map<User>(request);

            // Assert
            Assert.That(result.Email, Is.EqualTo(request.Email));
            Assert.That(result.UserName, Is.EqualTo(request.Email));
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
        public void AccessTokenDataToAuthToken_Null()
        {
            // Arrange
            AccessTokenData? accessTokenData = null;

            // Act
            var result = mapper.Map<AuthToken>(accessTokenData);

            //Assert
            Assert.That(result, Is.Null);
        }
    }
}
