using FluentValidation;
using LibraryShopEntities.Domain.Dtos;

namespace LibraryApi.Validators
{
    public class LibraryPaginationRequestValidator : AbstractValidator<LibraryPaginationRequest>
    {
        public LibraryPaginationRequestValidator()
        {
            RuleFor(x => x.ContainsName).NotNull().MaximumLength(256);
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
