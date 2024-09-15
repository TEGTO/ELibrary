using FluentValidation.TestHelper;
using LibraryApi.Domain.Dto.Book;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.Validators.Book.Tests
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
                Name = "Valid Book",
                PublicationDate = new DateTime(1980, 1, 1),
                Price = 500,
                CoverType = CoverType.Hard,
                PageAmount = 500,
                StockAmount = 100,
                AuthorId = 1,
                GenreId = 1,
                PublisherId = 1,
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
                Name = "",
                PublicationDate = new DateTime(3000, 1, 1),
                Price = -1,
                CoverType = CoverType.Any,
                PageAmount = -1,
                StockAmount = -1,
                AuthorId = 0,
                GenreId = 0,
                PublisherId = 0,
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.PublicationDate);
            result.ShouldHaveValidationErrorFor(x => x.Price);
            result.ShouldHaveValidationErrorFor(x => x.CoverType);
            result.ShouldHaveValidationErrorFor(x => x.PageAmount);
            result.ShouldHaveValidationErrorFor(x => x.StockAmount);
            result.ShouldHaveValidationErrorFor(x => x.AuthorId);
            result.ShouldHaveValidationErrorFor(x => x.GenreId);
            result.ShouldHaveValidationErrorFor(x => x.PublisherId);
        }
    }
}