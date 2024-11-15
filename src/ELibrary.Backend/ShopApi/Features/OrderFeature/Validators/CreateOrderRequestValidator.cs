
using FluentValidation;
using ShopApi.Features.OrderFeature.Dtos;
using System.Text.RegularExpressions;

namespace ShopApi.Features.OrderFeature.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator(IConfiguration configuration)
        {
            int maxAmount = int.Parse(configuration[Configuration.SHOP_MAX_ORDER_AMOUNT]!);
            RuleFor(x => x.DeliveryAddress).NotNull().NotEmpty().MaximumLength(512);
            RuleFor(x => x.DeliveryTime).NotNull().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.PaymentMethod).NotNull();
            RuleFor(x => x.ContactClientName).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(p => p.ContactPhone).NotNull().NotEmpty().MinimumLength(10).MaximumLength(50)
             .Matches(new Regex(@"^\d*$")).WithMessage("Phone number is not valid!");
            RuleFor(x => x.DeliveryMethod).NotNull();
            RuleFor(x => x.OrderBooks)
              .NotNull()
              .NotEmpty()
              .Must(orderBooks => orderBooks != null && orderBooks.Count <= maxAmount)
              .WithMessage($"The maximum number of books in an order is {maxAmount}.");
        }
    }
}
