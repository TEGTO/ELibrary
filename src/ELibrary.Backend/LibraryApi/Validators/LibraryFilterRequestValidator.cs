using FluentValidation;
using LibraryApi.Domain.Dtos;
using Shared.Configurations;
using Shared.Validators;

namespace LibraryApi.Validators
{
    public class LibraryFilterRequestValidator : AbstractValidator<LibraryFilterRequest>
    {
        public LibraryFilterRequestValidator(PaginationConfiguration paginationConfiguration)
        {
            Include(new PaginationRequestValidator(paginationConfiguration));
            RuleFor(x => x.ContainsName).NotNull().MaximumLength(256);
        }
    }
}
