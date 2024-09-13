using FluentValidation.TestHelper;
using Shared.Dtos;

namespace Shared.Validators.Tests
{
    [TestFixture]
    internal class PaginationRequestValidatorTests
    {
        private PaginationRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new PaginationRequestValidator();
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
            var request = new PaginationRequest { PageNumber = 0, PageSize = 10 };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }
        [Test]
        public void PaginatedRequestValidator_InvalidPageSize_FailsValidation()
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = 1, PageSize = 0 };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
        [Test]
        public void PaginatedRequestValidator_InvalidPageNumberAndPageSize_FailsValidation()
        {
            // Arrange
            var request = new PaginationRequest { PageNumber = 0, PageSize = 0 };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
    }
}