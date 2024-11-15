using FluentValidation.TestHelper;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.Extensions.Configuration;
using Moq;
using ShopApi;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.OrderFeature.Validators;

namespace ShopApiTests.Features.OrderFeature.Services.Validators
{
    [TestFixture]
    internal class CreateOrderRequestValidatorTests
    {
        private CreateOrderRequestValidator validator;
        private Mock<IConfiguration> mockConfiguration;

        [SetUp]
        public void SetUp()
        {
            mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c[Configuration.SHOP_MAX_ORDER_AMOUNT]).Returns("5");
            validator = new CreateOrderRequestValidator(mockConfiguration.Object);
        }

        [Test]
        public void Validate_ValidRequest_NoValidationErrors()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                ContactClientName = "Client Name",
                ContactPhone = "0123456789",
                DeliveryAddress = "Valid Address",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderBooks = new List<OrderBookRequest> { new OrderBookRequest { BookId = 1, BookAmount = 1 } },
                PaymentMethod = PaymentMethod.Cash
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Test]
        public void Validate_EmptyDeliveryAddress_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderBooks = new List<OrderBookRequest> { new OrderBookRequest { BookId = 1, BookAmount = 1 } }
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.DeliveryAddress);
        }
        [Test]
        public void Validate_NullDeliveryAddress_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = null,
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderBooks = new List<OrderBookRequest> { new OrderBookRequest { BookId = 1, BookAmount = 1 } }
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.DeliveryAddress);
        }
        [Test]
        public void Validate_DeliveryTimeInPast_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "Valid Address",
                DeliveryTime = DateTime.UtcNow.AddDays(-1),
                OrderBooks = new List<OrderBookRequest> { new OrderBookRequest { BookId = 1, BookAmount = 1 } }
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.DeliveryTime);
        }
        [Test]
        public void Validate_NullBooks_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "Valid Address",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderBooks = null
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.OrderBooks);
        }
        [Test]
        public void Validate_EmptyBooks_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "Valid Address",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderBooks = new List<OrderBookRequest>()
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.OrderBooks);
        }
        [Test]
        public void Validate_OrderBooksExceedingMaxAmount_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "Valid Address",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderBooks = new List<OrderBookRequest>
                {
                    new OrderBookRequest { BookId = 1, BookAmount = 1 },
                    new OrderBookRequest { BookId = 2, BookAmount = 1 },
                    new OrderBookRequest { BookId = 3, BookAmount = 1 },
                    new OrderBookRequest { BookId = 4, BookAmount = 1 },
                    new OrderBookRequest { BookId = 5, BookAmount = 1 },
                    new OrderBookRequest { BookId = 6, BookAmount = 1 }
                }
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.OrderBooks).WithErrorMessage("The maximum number of books in an order is 5.");
        }
        [Test]
        public void Validate_OrderBooksWithinMaxAmount_NoValidationErrors()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "Valid Address",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderBooks = new List<OrderBookRequest>
                {
                    new OrderBookRequest { BookId = 1, BookAmount = 1 },
                    new OrderBookRequest { BookId = 2, BookAmount = 1 },
                    new OrderBookRequest { BookId = 3, BookAmount = 1 },
                    new OrderBookRequest { BookId = 4, BookAmount = 1 },
                    new OrderBookRequest { BookId = 5, BookAmount = 1 }
                }
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(x => x.OrderBooks);
        }
    }
}