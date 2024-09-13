
using FluentValidation;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Domain.Dtos.Order;

namespace ShopApi.Validators.Order
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.DeliveryAddress).NotNull().NotEmpty().MaximumLength(512);
            RuleFor(x => x.DeliveryTime).NotNull().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.OrderStatus).NotNull().Equal(OrderStatus.InProcessing);
            RuleFor(x => x.Books).NotNull().NotEmpty();
        }
    }
}
