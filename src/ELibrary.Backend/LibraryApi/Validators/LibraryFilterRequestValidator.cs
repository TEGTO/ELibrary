using FluentValidation;
using LibraryShopEntities.Filters;
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
