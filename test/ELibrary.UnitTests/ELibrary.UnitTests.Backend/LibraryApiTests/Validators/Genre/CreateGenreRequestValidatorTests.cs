using FluentValidation.TestHelper;
using LibraryApi.Domain.Dto.Genre;

namespace LibraryApi.Validators.Genre.Tests
{
    [TestFixture]
    internal class CreateGenreRequestValidatorTests
    {
        private CreateGenreRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new CreateGenreRequestValidator();
        }

        [Test]
        public void CreateGenreRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var request = new CreateGenreRequest { Name = "Valid Genre" };

            // Act 
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void CreateGenreRequestValidator_InvalidData_FailsValidation()
        {
            // Arrange
            var request = new CreateGenreRequest { Name = "" };

            // Act 
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}