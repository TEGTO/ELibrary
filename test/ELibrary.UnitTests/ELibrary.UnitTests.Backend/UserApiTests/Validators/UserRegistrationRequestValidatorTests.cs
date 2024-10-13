using FluentValidation.TestHelper;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.Validators.Tests
{
    [TestFixture]
    internal class UserRegistrationRequestValidatorTests
    {
        private UserRegistrationRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new UserRegistrationRequestValidator();
        }

        [Test]
        public void Validate_UserNameIsNull_HasValidationError()
        {
            // Arrange
            var request = new UserRegistrationRequest { Email = null };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Test]
        public void Validate_UserNameTooBig_HasValidationError()
        {
            // Arrange
            var request = new UserRegistrationRequest { Email = new string('A', 257) };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Test]
        public void Validate_PasswordIsNull_HasValidationError()
        {
            // Arrange
            var request = new UserRegistrationRequest { Password = null };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
        [Test]
        public void Validate_PasswordTooSmall_HasValidationError()
        {
            // Arrange
            var request = new UserRegistrationRequest { Password = "1234" };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
        [Test]
        public void Validate_PasswordTooBig_HasValidationError()
        {
            // Arrange
            var request = new UserRegistrationRequest { Password = new string('A', 257) };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
        [Test]
        public void Validate_ConfirmPasswordIsNull_HasValidationError()
        {
            // Arrange
            var request = new UserRegistrationRequest { Email = "UserName", Password = "12345678", ConfirmPassword = null };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }
        [Test]
        public void Validate_ConfirmPasswordNotSameAsPassword_HasValidationError()
        {
            // Arrange
            var request = new UserRegistrationRequest { Password = "12345678", ConfirmPassword = "12345" };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }
        [Test]
        public void Validate_ConfirmPasswordTooSmall_HasValidationError()
        {
            // Arrange
            var request = new UserRegistrationRequest { ConfirmPassword = "1234" };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }
        [Test]
        public void Validate_ConfirmPasswordTooBig_HasValidationError()
        {
            // Arrange
            var request = new UserRegistrationRequest { ConfirmPassword = new string('A', 257) };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }
    }
}
