using FluentValidation.TestHelper;
using UserApi.Domain.Dtos;
using UserApi.Validators.Auth;

namespace AuthenticationApiTests.Validators
{
    [TestFixture]
    internal class UserAuthenticationRequestValidatorTests
    {
        private UserAuthenticationRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new UserAuthenticationRequestValidator();
        }

        [Test]
        public void Validate_LoginIsNull_HasValidationError()
        {
            // Arrange
            var request = new UserAuthenticationRequest { Login = null };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Login);
        }
        [Test]
        public void Validate_LoginTooBig_HasValidationError()
        {
            // Arrange
            var request = new UserAuthenticationRequest { Login = new string('A', 257), Password = "12345678" };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Login);
        }
        [Test]
        public void Validate_PasswordIsNull_HasValidationError()
        {
            // Arrange
            var request = new UserAuthenticationRequest { Password = null };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
        [Test]
        public void Validate_PasswordTooSmall_HasValidationError()
        {
            // Arrange
            var request = new UserAuthenticationRequest { Password = "1234" };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
        [Test]
        public void Validate_PasswordTooBig_HasValidationError()
        {
            // Arrange
            var request = new UserAuthenticationRequest { Password = new string('A', 257) };
            // Act
            var result = validator.TestValidate(request);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}
