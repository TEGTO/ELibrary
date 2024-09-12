using FluentValidation.TestHelper;
using LibraryShopEntities.Domain.Dto.Author;

namespace LibraryShopEntities.Validators.Author
{
    [TestFixture]
    internal class CreateAuthorRequestValidatorTests
    {
        [Test]
        public void CreateAuthorRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var validator = new CreateAuthorRequestValidator();
            var request = new CreateAuthorRequest
            {
                Name = "Author Name",
                LastName = "Author Last Name",
                DateOfBirth = new DateTime(1980, 1, 1)
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void CreateAuthorRequestValidator_InvalidData_FailsValidation()
        {
            // Arrange
            var validator = new CreateAuthorRequestValidator();
            var request = new CreateAuthorRequest
            {
                Name = "",
                LastName = "",
                DateOfBirth = new DateTime(1980, 1, 1)
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }
    }
}