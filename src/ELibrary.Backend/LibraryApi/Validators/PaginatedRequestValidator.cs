using FluentValidation;
using LibraryApi.Domain.Dto;

namespace LibraryApi.Validators
{
    public class PaginatedRequestValidator : AbstractValidator<PaginatedRequest>
    {
        public PaginatedRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
