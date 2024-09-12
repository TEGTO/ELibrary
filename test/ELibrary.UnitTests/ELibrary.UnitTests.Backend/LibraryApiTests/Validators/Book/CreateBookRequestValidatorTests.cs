using FluentValidation.TestHelper;
using LibraryShopEntities.Domain.Dto.Book;

namespace LibraryShopEntities.Validators.Book
{
    [TestFixture]
    internal class CreateBookRequestValidatorTests
    {
        [Test]
        public void CreateBookRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var validator = new CreateBookRequestValidator();
            var request = new CreateBookRequest
            {
                Title = "Valid Book",
                AuthorId = 1,
                GenreId = 1,
                PublicationDate = DateTime.Now
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void CreateBookRequestValidator_InvalidData_FailsValidation()
        {
            // Arrange
            var validator = new CreateBookRequestValidator();
            var request = new CreateBookRequest
            {
                Title = "",
                AuthorId = 0,
                GenreId = 0,
                PublicationDate = DateTime.Now
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.AuthorId);
            result.ShouldHaveValidationErrorFor(x => x.GenreId);
        }
    }
}