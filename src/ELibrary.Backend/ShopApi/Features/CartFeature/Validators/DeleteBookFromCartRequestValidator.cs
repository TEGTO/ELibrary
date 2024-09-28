using FluentValidation;
using ShopApi.Features.CartFeature.Dtos;

namespace ShopApi.Features.CartFeature.Validators
{
    public class DeleteBookFromCartRequestValidator : AbstractValidator<DeleteCartBookFromCartRequest>
    {
        public DeleteBookFromCartRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
