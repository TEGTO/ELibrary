using FluentValidation;
using LibraryApi.Domain.Dtos;

namespace LibraryApi.Validators
{
    public class LibraryPaginationRequestValidator : AbstractValidator<LibraryFilterRequest>
    {
        public LibraryPaginationRequestValidator()
        {
            RuleFor(x => x.ContainsName).NotNull().MaximumLength(256);
            RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).NotNull().GreaterThanOrEqualTo(0);
        }
    }
}
