using FluentValidation.TestHelper;
using ShopApi.Features.ClientFeature.Dtos;
using ShopApi.Features.ClientFeature.Validators;

namespace ShopApiTests.Features.ClientFeature.Validators
{
    [TestFixture]
    internal class CreateClientRequestValidatorTests
    {
        private CreateClientRequestValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new CreateClientRequestValidator();
        }

        [Test]
        public void Validate_NullName_ShouldHaveValidationError()
        {
            var request = new CreateClientRequest
            {
                Name = null
            };
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Test]
        public void Validate_EmptyName_ShouldHaveValidationError()
        {
            var request = new CreateClientRequest
            {
                Name = ""
            };
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        [Test]
        public void Validate_ValidData_ShouldNotHaveAnyValidationErrors()
        {
            var request = new CreateClientRequest
            {
                Name = "John",
                MiddleName = "A.",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 5, 15),
                Address = "123 Main St",
                Phone = "0123456789",
                Email = "john.doe@example.com"
            };
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void Validate_NullLastName_ShouldHaveValidationError()
        {
            var request = new CreateClientRequest
            {
                LastName = null
            };
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }
        [Test]
        public void Validate_MiddleNameExceedsMaxLength_ShouldHaveValidationError()
        {
            var request = new CreateClientRequest
            {
                MiddleName = new string('A', 257)
            };
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.MiddleName);
        }
        [Test]
        public void Validate_DateOfBirthInFuture_ShouldHaveValidationError()
        {
            var request = new CreateClientRequest
            {
                DateOfBirth = DateTime.UtcNow.AddDays(1)
            };
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }
        [Test]
        public void Validate_InvalidEmail_ShouldHaveValidationError()
        {
            var request = new CreateClientRequest
            {
                Email = "invalid-email"
            };
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Test]
        public void Validate_AddressExceedsMaxLength_ShouldHaveValidationError()
        {
            var request = new CreateClientRequest
            {
                Address = new string('A', 257)
            };
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
        [Test]
        public void Validate_NullPhone_ShouldHaveValidationError()
        {
            var request = new CreateClientRequest
            {
                Phone = null
            };
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }
    }
}