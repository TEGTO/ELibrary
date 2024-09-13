using FluentValidation;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Domain.Dtos.Order;

namespace ShopApi.Validators.Order
{
    public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.DeliveryAddress).NotNull().NotEmpty().MaximumLength(512);
            RuleFor(x => x.DeliveryTime).NotNull().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.OrderStatus).NotNull().NotEqual(OrderStatus.Delivered);
            RuleFor(x => x.Books).NotNull().NotEmpty();
        }
    }
}
