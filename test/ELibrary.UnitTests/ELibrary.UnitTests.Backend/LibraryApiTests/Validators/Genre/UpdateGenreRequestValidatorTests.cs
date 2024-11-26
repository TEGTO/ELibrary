using FluentValidation.TestHelper;
using LibraryApi.Domain.Dto.Genre;

namespace LibraryApi.Validators.Genre.Tests
{
    [TestFixture]
    internal class UpdateGenreRequestValidatorTests
    {
        private UpdateGenreRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new UpdateGenreRequestValidator();
        }

        [Test]
        public void UpdateGenreRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var request = new UpdateGenreRequest { Id = 1, Name = "Valid Genre" };

            // Act 
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void UpdateGenreRequestValidator_InvalidData_FailsValidation()
        {
            // Arrange
            var request = new UpdateGenreRequest { Id = 0, Name = "" };

            // Act 
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}