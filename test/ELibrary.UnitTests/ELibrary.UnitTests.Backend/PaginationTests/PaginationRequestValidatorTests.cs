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
        [TestCase(1, 10, Description = "Valid PageNumber and PageSize")]
        [TestCase(0, 1, Description = "Edge case: minimum valid values")]
        [TestCase(1, 100, Description = "Edge case: max PageSize")]
        public void PaginatedRequestValidator_ValidData_PassesValidation(int pageNumber, int pageSize)
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = pageNumber, PageSize = pageSize };

            // Act
            var result = validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        [TestCase(-1, 10, Description = "Invalid PageNumber")]
        [TestCase(1, -1, Description = "Invalid PageSize")]
        [TestCase(-1, -1, Description = "Both PageNumber and PageSize invalid")]
        public void PaginatedRequestValidator_InvalidData_FailsValidation(int pageNumber, int pageSize)
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = pageNumber, PageSize = pageSize };

            // Act
            var result = validator.TestValidate(request);

            // Assert
            if (pageNumber < 0)
            {
                result.ShouldHaveValidationErrorFor(x => x.PageNumber);
            }

            if (pageSize < 0)
            {
                result.ShouldHaveValidationErrorFor(x => x.PageSize);
            }
        }

        [Test]
        [TestCase(1, 101, Description = "PageSize exceeds max limit")]
        [TestCase(1, 100, Description = "PageSize equals max limit")]
        [TestCase(1, 99, Description = "PageSize below max limit")]
        public void PaginatedRequestValidator_PageSizeValidation(int pageNumber, int pageSize)
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = pageNumber, PageSize = pageSize };

            // Act
            var result = validator.TestValidate(request);

            // Assert
            if (pageSize > paginationOptions.MaxPaginationPageSize)
            {
                result.ShouldHaveValidationErrorFor(x => x.PageSize);
            }
            else
            {
                result.ShouldNotHaveAnyValidationErrors();
            }
        }
    }
}