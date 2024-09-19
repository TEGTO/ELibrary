using FluentValidation;
using ShopApi.Domain.Dtos.Cart;

namespace ShopApi.Validators.Cart
{
    public class UpdateCartBookRequestValidator : AbstractValidator<UpdateCartBookRequest>
    {
        public UpdateCartBookRequestValidator(IConfiguration configuration)
        {
            int maxAmount = int.Parse(configuration[Configuration.SHOP_MAX_ORDER_AMOUNT]!);
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.BookAmount).NotNull().GreaterThan(0).LessThanOrEqualTo(maxAmount);
        }
    }
}
