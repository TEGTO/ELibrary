using FluentValidation.TestHelper;
using LibraryApi.Domain.Dto.Genre;

namespace LibraryApi.Validators.Genre.Tests
{
    [TestFixture]
    internal class CreateGenreRequestValidatorTests
    {
        [Test]
        public void CreateGenreRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var validator = new CreateGenreRequestValidator();
            var request = new CreateGenreRequest { Name = "Valid Genre" };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void CreateGenreRequestValidator_InvalidData_FailsValidation()
        {
            // Arrange
            var validator = new CreateGenreRequestValidator();
            var request = new CreateGenreRequest { Name = "" };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}