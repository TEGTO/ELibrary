using FluentValidation;
using LibraryApi.Domain.Dtos;

namespace LibraryApi.Validators
{
    public class LibraryPaginationRequestValidator : AbstractValidator<LibraryPaginationRequest>
    {
        public LibraryPaginationRequestValidator()
        {
            RuleFor(x => x.ContainsName).NotNull().MaximumLength(256);
            RuleFor(x => x.PageNumber).NotNull().GreaterThan(0);
            RuleFor(x => x.PageSize).NotNull().GreaterThan(0);
        }
    }
}
