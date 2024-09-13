using FluentValidation.TestHelper;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Domain.Dtos.Order;

namespace ShopApi.Validators.Order.Tests
{
    [TestFixture]
    internal class CreateOrderRequestValidatorTests
    {
        private CreateOrderRequestValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new CreateOrderRequestValidator();
        }

        [Test]
        public void Validate_ValidRequest_NoValidationErrors()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "Valid Address",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderStatus = OrderStatus.InProcessing,
                Books = new List<OrderBook> { new OrderBook { Id = 1 } }
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
                OrderStatus = OrderStatus.InProcessing,
                Books = new List<OrderBook> { new OrderBook { Id = 1 } }
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
                OrderStatus = OrderStatus.InProcessing,
                Books = new List<OrderBook> { new OrderBook { Id = 1 } }
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
                OrderStatus = OrderStatus.InProcessing,
                Books = new List<OrderBook> { new OrderBook { Id = 1 } }
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.DeliveryTime);
        }
        [Test]
        public void Validate_NonInProcessingOrderStatus_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "Valid Address",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderStatus = OrderStatus.Packed,
                Books = new List<OrderBook> { new OrderBook { Id = 1 } }
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.OrderStatus);
        }
        [Test]
        public void Validate_NullBooks_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "Valid Address",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderStatus = OrderStatus.InProcessing,
                Books = null
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Books);
        }
        [Test]
        public void Validate_EmptyBooks_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                DeliveryAddress = "Valid Address",
                DeliveryTime = DateTime.UtcNow.AddDays(1),
                OrderStatus = OrderStatus.InProcessing,
                Books = new List<OrderBook>()
            };
            // Act & Assert
            var result = validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Books);
        }
    }
}