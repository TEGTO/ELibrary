using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Pagination.Tests
{
    [TestFixture]
    internal class PaginationRequestValidatorTests
    {
        private PaginationRequestValidator validator;
        private PaginationOptions paginationOptions;

        [SetUp]
        public void SetUp()
        {
            paginationOptions = new PaginationOptions(100);
            validator = new PaginationRequestValidator(paginationOptions);
        }

        [Test]
        public void PaginatedRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = 1, PageSize = 10 };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void PaginatedRequestValidator_InvalidPageNumber_FailsValidation()
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = -1, PageSize = 10 };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }
        [Test]
        public void PaginatedRequestValidator_InvalidPageSize_FailsValidation()
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = 1, PageSize = -1 };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
        [Test]
        public void PaginatedRequestValidator_InvalidPageNumberAndPageSize_FailsValidation()
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = -1, PageSize = -1 };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
        [Test]
        public void PaginatedRequestValidator_PageSizeExceedsMaxLimit_FailsValidation()
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = 1, PageSize = paginationOptions.MaxPaginationPageSize + 1 };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
        [Test]
        public void PaginatedRequestValidator_PageSizeEqualsMaxLimit_PassesValidation()
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = 1, PageSize = paginationOptions.MaxPaginationPageSize };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void PaginatedRequestValidator_PageSizeBelowMaxLimit_PassesValidation()
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = 1, PageSize = paginationOptions.MaxPaginationPageSize - 1 };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}