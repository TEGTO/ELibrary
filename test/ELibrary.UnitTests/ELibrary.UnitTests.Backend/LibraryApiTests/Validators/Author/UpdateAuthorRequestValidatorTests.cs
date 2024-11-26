using FluentValidation.TestHelper;
using LibraryApi.Domain.Dto.Author;

namespace LibraryApi.Validators.Author.Tests
{
    [TestFixture]
    internal class UpdateAuthorRequestValidatorTests
    {
        private UpdateAuthorRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new UpdateAuthorRequestValidator();
        }

        [Test]
        public void UpdateAuthorRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var request = new UpdateAuthorRequest
            {
                Id = 1,
                Name = "Updated Author Name",
                LastName = "Updated Author Last Name",
                DateOfBirth = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            // Act
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void UpdateAuthorRequestValidator_InvalidData_FailsValidation()
        {
            // Arrange
            var request = new UpdateAuthorRequest
            {
                Id = 0,
                Name = "",
                LastName = "",
                DateOfBirth = new DateTime(3000, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            // Act
            var result = validator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }
    }
}