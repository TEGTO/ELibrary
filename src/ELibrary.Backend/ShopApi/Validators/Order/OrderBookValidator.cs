using FluentValidation;
using ShopApi.Domain.Dtos.Order;

namespace ShopApi.Validators.Order
{
    public class OrderBookValidator : AbstractValidator<OrderBook>
    {
        public OrderBookValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}