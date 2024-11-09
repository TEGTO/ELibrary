using FluentValidation;

namespace Pagination
{
    public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
    {
        public PaginationRequestValidator(PaginationOptions paginationOptions)
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(0).LessThanOrEqualTo(paginationOptions.MaxPaginationPageSize);
        }
    }
}
