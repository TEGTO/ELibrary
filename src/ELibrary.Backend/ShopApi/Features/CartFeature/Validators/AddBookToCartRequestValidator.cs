using FluentValidation;
using ShopApi.Features.CartFeature.Dtos;

namespace ShopApi.Features.CartFeature.Validators
{
    public class AddBookToCartRequestValidator : AbstractValidator<AddBookToCartRequest>
    {
        public AddBookToCartRequestValidator(IConfiguration configuration)
        {
            int maxAmount = int.Parse(configuration[Configuration.SHOP_MAX_ORDER_AMOUNT]!);
            RuleFor(x => x.BookAmount).NotNull().GreaterThan(0).LessThanOrEqualTo(maxAmount);
            RuleFor(x => x.BookId).NotNull().GreaterThan(0);
        }
    }
}
