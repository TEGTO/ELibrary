using FluentValidation;
using LibraryApi.Domain.Dtos;
using Shared.Configurations;

namespace LibraryApi.Validators
{
    public class GetByIdsRequestValidator : AbstractValidator<GetByIdsRequest>
    {
        public GetByIdsRequestValidator(PaginationConfiguration paginationConfiguration)
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty().Must(x => x.Count <= paginationConfiguration.MaxPaginationPageSize);
        }
    }
}
