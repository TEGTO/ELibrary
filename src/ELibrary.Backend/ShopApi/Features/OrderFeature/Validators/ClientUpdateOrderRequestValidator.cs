using FluentValidation;
using ShopApi.Features.OrderFeature.Dtos;
using System.Text.RegularExpressions;

namespace ShopApi.Features.OrderFeature.Validators
{
    public class ClientUpdateOrderRequestValidator : AbstractValidator<ClientUpdateOrderRequest>
    {
        public ClientUpdateOrderRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.ContactClientName).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(p => p.ContactPhone).NotNull().NotEmpty().MinimumLength(10).MaximumLength(50)
             .Matches(new Regex(@"^\d*$")).WithMessage("Phone number is not valid!");
            RuleFor(x => x.DeliveryAddress).NotNull().NotEmpty().MaximumLength(512);
            RuleFor(x => x.DeliveryTime).NotNull().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.PaymentMethod).NotNull();
            RuleFor(x => x.DeliveryMethod).NotNull();
        }
    }
}
