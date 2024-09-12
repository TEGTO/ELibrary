using FluentValidation.TestHelper;
using UserApi.Domain.Dtos;
using UserApi.Validators.Auth;

namespace UserApi.Validators
{
    [TestFixture]
    internal class UserInfoDtoValidatorTests
    {
        private UserInfoDtoValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new UserInfoDtoValidator();
        }

        [Test]
        public void Validate_NameIsNull_HasValidationError()
        {
            // Arrange
            var userInfo = new UserInfoDto { Name = null };
            // Act
            var result = validator.TestValidate(userInfo);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Test]
        public void Validate_NameIsEmpty_HasValidationError()
        {
            // Arrange
            var userInfo = new UserInfoDto { Name = string.Empty };
            // Act
            var result = validator.TestValidate(userInfo);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Test]
        public void Validate_NameTooLong_HasValidationError()
        {
            // Arrange
            var userInfo = new UserInfoDto { Name = new string('A', 257) };
            // Act
            var result = validator.TestValidate(userInfo);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Test]
        public void Validate_LastNameIsNull_HasValidationError()
        {
            // Arrange
            var userInfo = new UserInfoDto { LastName = null };
            // Act
            var result = validator.TestValidate(userInfo);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }
        [Test]
        public void Validate_LastNameTooLong_HasValidationError()
        {
            // Arrange
            var userInfo = new UserInfoDto { LastName = new string('A', 257) };
            // Act
            var result = validator.TestValidate(userInfo);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }
        [Test]
        public void Validate_AddressIsNull_HasValidationError()
        {
            // Arrange
            var userInfo = new UserInfoDto { Address = null };
            // Act
            var result = validator.TestValidate(userInfo);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Test]
        public void Validate_AddressTooLong_HasValidationError()
        {
            // Arrange
            var userInfo = new UserInfoDto { Address = new string('A', 257) };
            // Act
            var result = validator.TestValidate(userInfo);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Test]
        public void Validate_ValidUserInfo_ShouldNotHaveValidationError()
        {
            // Arrange
            var userInfo = new UserInfoDto
            {
                Name = "John",
                LastName = "Doe",
                Address = "123 Main St",
                DateOfBirth = DateTime.UtcNow.AddYears(-30)
            };
            // Act
            var result = validator.TestValidate(userInfo);
            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}