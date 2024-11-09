using FluentValidation;
using LibraryShopEntities.Domain.Dtos.SharedRequests;

namespace LibraryApi.Validators.Book
{
    public class UpdateBookStockAmountRequestValidator : AbstractValidator<UpdateBookStockAmountRequest>
    {
        public UpdateBookStockAmountRequestValidator()
        {
            RuleFor(x => x.BookId).NotNull().GreaterThan(0);
            RuleFor(x => x.ChangeAmount).NotNull();
        }
    }
    public class UpdateBookStockAmountRequestCollectionValidator : AbstractValidator<List<UpdateBookStockAmountRequest>>
    {
        public UpdateBookStockAmountRequestCollectionValidator()
        {
            RuleForEach(x => x).SetValidator(new UpdateBookStockAmountRequestValidator());
        }
    }
}
