using FluentValidation;
using LibraryApi.Domain.Dtos;
using Pagination;

namespace LibraryApi.Validators
{
    public class LibraryFilterRequestValidator : AbstractValidator<LibraryFilterRequest>
    {
        public LibraryFilterRequestValidator(PaginationOptions paginationConfiguration)
        {
            Include(new PaginationRequestValidator(paginationConfiguration));
            RuleFor(x => x.ContainsName).NotNull().MaximumLength(256);
        }
    }
}
