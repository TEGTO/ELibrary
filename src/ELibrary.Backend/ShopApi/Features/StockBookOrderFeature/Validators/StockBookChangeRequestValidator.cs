using FluentValidation;
using ShopApi.Features.StockBookOrderFeature.Dtos;

namespace ShopApi.Features.StockBookOrderFeature.Validators
{
    public class StockBookChangeRequestValidator : AbstractValidator<StockBookChangeRequest>
    {
        public StockBookChangeRequestValidator()
        {
            RuleFor(x => x.BookId).NotNull().GreaterThan(0);
            RuleFor(x => x.ChangeAmount).NotNull().NotEqual(0);
        }
    }
}
