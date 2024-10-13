using FluentValidation;
using ShopApi.Features.CartFeature.Dtos;

namespace ShopApi.Features.CartFeature.Validators
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
