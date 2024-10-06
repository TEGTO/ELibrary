using FluentValidation;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Validators
{
    public class ClientUpdateOrderRequestValidator : AbstractValidator<ClientUpdateOrderRequest>
    {
        public ClientUpdateOrderRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.DeliveryAddress).NotNull().NotEmpty().MaximumLength(512);
            RuleFor(x => x.DeliveryTime).NotNull().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.PaymentMethod).NotNull();
            RuleFor(x => x.DeliveryMethod).NotNull();
        }
    }
}
