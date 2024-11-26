using FluentValidation.TestHelper;
using LibraryApi.Domain.Dto.Book;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.Validators.Book.Tests
{
    [TestFixture]
    internal class CreateBookRequestValidatorTests
    {
        private CreateBookRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new CreateBookRequestValidator();
        }

        [Test]
        public void CreateBookRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var request = new CreateBookRequest
            {
                Name = "Valid Book",
                PublicationDate = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Price = 500,
                CoverType = CoverType.Hard,
                PageAmount = 500,
                CoverImgUrl = "valid-url",
                AuthorId = 1,
                GenreId = 1,
                PublisherId = 1,
            };

            // Act
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void CreateBookRequestValidator_InvalidData_FailsValidation()
        {
            // Arrange
            var request = new CreateBookRequest
            {
                Name = "",
                PublicationDate = new DateTime(3000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Price = -1,
                CoverType = CoverType.Any,
                PageAmount = -1,
                CoverImgUrl = "",
                AuthorId = 0,
                GenreId = 0,
                PublisherId = 0,
            };

            // Act 
            var result = validator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.PublicationDate);
            result.ShouldHaveValidationErrorFor(x => x.Price);
            result.ShouldHaveValidationErrorFor(x => x.CoverType);
            result.ShouldHaveValidationErrorFor(x => x.CoverImgUrl);
            result.ShouldHaveValidationErrorFor(x => x.PageAmount);
            result.ShouldHaveValidationErrorFor(x => x.AuthorId);
            result.ShouldHaveValidationErrorFor(x => x.GenreId);
            result.ShouldHaveValidationErrorFor(x => x.PublisherId);
        }
    }
}