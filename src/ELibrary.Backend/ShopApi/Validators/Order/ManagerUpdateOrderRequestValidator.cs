using FluentValidation;
using ShopApi.Domain.Dtos.Order;

namespace ShopApi.Validators.Order
{
    public class ManagerUpdateOrderRequestValidator : AbstractValidator<ManagerUpdateOrderRequest>
    {
        public ManagerUpdateOrderRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.DeliveryAddress).NotNull().NotEmpty().MaximumLength(512);
            RuleFor(x => x.DeliveryTime).NotNull().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.OrderStatus).NotNull();
        }
    }
}
