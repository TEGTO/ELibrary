using FluentValidation.TestHelper;
using LibraryApi.Domain.Dtos;
using Shared.Configurations;

namespace LibraryApi.Validators.Tests
{
    [TestFixture]
    internal class LibraryFilterRequestValidatorTests
    {
        private LibraryFilterRequestValidator validator;
        private PaginationConfiguration paginationConfiguration;

        [SetUp]
        public void SetUp()
        {
            paginationConfiguration = new PaginationConfiguration(100);
            validator = new LibraryFilterRequestValidator(paginationConfiguration);
        }

        [Test]
        public void LibraryFilterRequestValidator_ValidData_PassesValidation()
        {
            // Arrange
            var request = new LibraryFilterRequest { PageNumber = 1, PageSize = 10, ContainsName = "Valid Name" };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void LibraryFilterRequestValidator_InvalidPageNumber_FailsValidation()
        {
            // Arrange
            var request = new LibraryFilterRequest { PageNumber = -1, PageSize = 10, ContainsName = "Valid Name" };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }
        [Test]
        public void LibraryFilterRequestValidator_InvalidPageSize_FailsValidation()
        {
            // Arrange
            var request = new LibraryFilterRequest { PageNumber = 1, PageSize = -1, ContainsName = "Valid Name" };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
        [Test]
        public void LibraryFilterRequestValidator_InvalidContainsName_FailsValidation()
        {
            // Arrange
            var request = new LibraryFilterRequest { PageNumber = 1, PageSize = 10, ContainsName = null };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.ContainsName);
        }
        [Test]
        public void LibraryFilterRequestValidator_ContainsNameExceedsMaxLength_FailsValidation()
        {
            // Arrange
            var request = new LibraryFilterRequest { PageNumber = 1, PageSize = 10, ContainsName = new string('a', 257) };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.ContainsName);
        }
        [Test]
        public void LibraryFilterRequestValidator_PageSizeExceedsMaxLimit_FailsValidation()
        {
            // Arrange
            var request = new LibraryFilterRequest { PageNumber = 1, PageSize = paginationConfiguration.MaxPaginationPageSize + 1, ContainsName = "Valid Name" };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
        [Test]
        public void LibraryFilterRequestValidator_PageSizeAtMaxLimit_PassesValidation()
        {
            // Arrange
            var request = new LibraryFilterRequest { PageNumber = 1, PageSize = paginationConfiguration.MaxPaginationPageSize, ContainsName = "Valid Name" };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}