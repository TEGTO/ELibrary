using FluentValidation;
using ShopApi.Domain.Dtos.Order;

namespace ShopApi.Validators.Order
{
    public class ClientUpdateOrderRequestValidator : AbstractValidator<ClientUpdateOrderRequest>
    {
        public ClientUpdateOrderRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.DeliveryAddress).NotNull().NotEmpty().MaximumLength(512);
            RuleFor(x => x.DeliveryTime).NotNull().GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
