using FluentValidation.TestHelper;
using LibraryApi.Domain.Dtos.Library.Author;
using LibraryApi.Validators.Author;

namespace LibraryApi.Validators.Author.Tests
{
    [TestFixture]
    internal class UpdateAuthorRequestValidatorTests
    {
        [Test]
        public void UpdateAuthorRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var validator = new UpdateAuthorRequestValidator();
            var request = new UpdateAuthorRequest
            {
                Id = 1,
                Name = "Updated Author Name",
                LastName = "Updated Author Last Name",
                DateOfBirth = new DateTime(1980, 1, 1)
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void UpdateAuthorRequestValidator_InvalidData_FailsValidation()
        {
            // Arrange
            var validator = new UpdateAuthorRequestValidator();
            var request = new UpdateAuthorRequest
            {
                Id = 0,
                Name = "",
                LastName = "",
                DateOfBirth = new DateTime(3000, 1, 1)
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }
    }
}