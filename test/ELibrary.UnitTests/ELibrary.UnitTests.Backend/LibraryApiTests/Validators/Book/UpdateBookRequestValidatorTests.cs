﻿using FluentValidation.TestHelper;
using LibraryApi.Domain.Dto.Book;

namespace LibraryApi.Validators.Book
{
    [TestFixture]
    internal class UpdateBookRequestValidatorTests
    {
        [Test]
        public void UpdateBookRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var validator = new UpdateBookRequestValidator();
            var request = new UpdateBookRequest
            {
                Id = 1,
                Title = "Valid Book",
                AuthorId = "1",
                GenreId = "1",
                PublicationDate = DateTime.Now
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void UpdateBookRequestValidator_InvalidData_FailsValidation()
        {
            // Arrange
            var validator = new UpdateBookRequestValidator();
            var request = new UpdateBookRequest
            {
                Id = 0,
                Title = "",
                AuthorId = "",
                GenreId = "",
                PublicationDate = DateTime.Now
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Title);
            result.ShouldHaveValidationErrorFor(x => x.AuthorId);
            result.ShouldHaveValidationErrorFor(x => x.GenreId);
        }
    }
}