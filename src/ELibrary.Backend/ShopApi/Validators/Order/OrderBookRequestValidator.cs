using FluentValidation;
using ShopApi.Domain.Dtos.Order;

namespace ShopApi.Validators.Order
{
    public class OrderBookRequestValidator : AbstractValidator<OrderBookRequest>
    {
        public OrderBookRequestValidator(IConfiguration configuration)
        {
            int maxAmount = int.Parse(configuration[Configuration.SHOP_MAX_ORDER_AMOUNT]!);
            RuleFor(x => x.BookAmount).NotNull().GreaterThan(0).LessThanOrEqualTo(maxAmount);
            RuleFor(x => x.BookId).NotNull().GreaterThan(0);
        }
    }
}