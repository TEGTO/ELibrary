using FluentValidation;
using ShopApi.Domain.Dtos.Cart;

namespace ShopApi.Validators.Cart
{
    public class AddCartBookToCartRequestValidator : AbstractValidator<AddCartBookToCartRequest>
    {
        public AddCartBookToCartRequestValidator(IConfiguration configuration)
        {
            int maxAmount = int.Parse(configuration[Configuration.SHOP_MAX_ORDER_AMOUNT]!);
            RuleFor(x => x.BookAmount).NotNull().GreaterThan(0).LessThanOrEqualTo(maxAmount);
            RuleFor(x => x.BookId).NotNull().GreaterThan(0);
        }
    }
}
