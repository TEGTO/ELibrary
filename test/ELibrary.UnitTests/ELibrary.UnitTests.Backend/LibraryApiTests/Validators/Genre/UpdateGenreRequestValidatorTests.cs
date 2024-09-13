using FluentValidation.TestHelper;
using LibraryApi.Domain.Dtos.Library.Genre;
using LibraryApi.Validators.Genre;

namespace LibraryApi.Validators.Genre.Tests
{
    [TestFixture]
    internal class UpdateGenreRequestValidatorTests
    {
        [Test]
        public void UpdateGenreRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var validator = new UpdateGenreRequestValidator();
            var request = new UpdateGenreRequest { Id = 1, Name = "Valid Genre" };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void UpdateGenreRequestValidator_InvalidData_FailsValidation()
        {
            // Arrange
            var validator = new UpdateGenreRequestValidator();
            var request = new UpdateGenreRequest { Id = 0, Name = "" };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}