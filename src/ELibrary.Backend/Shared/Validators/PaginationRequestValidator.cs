using FluentValidation;
using Shared.Configurations;
using Shared.Domain.Dtos;

namespace Shared.Validators
{
    public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
    {
        public PaginationRequestValidator(PaginationConfiguration paginationConfiguration)
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(0).LessThanOrEqualTo(paginationConfiguration.MaxPaginationPageSize);
        }
    }
}
