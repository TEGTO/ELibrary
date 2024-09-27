using FluentValidation;
using ShopApi.Features.StockBookOrderFeature.Dtos;

namespace ShopApi.Features.StockBookOrderFeature.Validators
{
    public class CreateStockBookOrderRequestValidator : AbstractValidator<CreateStockBookOrderRequest>
    {
        public CreateStockBookOrderRequestValidator()
        {
            RuleFor(x => x.ClientId).NotNull().NotEmpty();
            RuleFor(x => x.StockBookChanges).NotNull().NotEmpty();
        }
    }
}
