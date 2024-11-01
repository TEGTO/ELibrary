using FluentValidation;
using LibraryApi.Domain.Dtos;
using Shared.Configurations;

namespace LibraryApi.Validators
{
    public class GetByIdsRequestValidator : AbstractValidator<GetByIdsRequest>
    {
        public GetByIdsRequestValidator(PaginationConfiguration paginationConfiguration)
        {
            RuleFor(x => x.Ids).NotNull().Must(x => x != null && x.Count <= paginationConfiguration.MaxPaginationPageSize);
        }
    }
}
