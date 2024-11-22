using FluentValidation;
using LibraryShopEntities.Domain.Dtos.SharedRequests;
using Pagination;

namespace LibraryApi.Validators
{
    public class GetByIdsRequestValidator : AbstractValidator<GetByIdsRequest>
    {
        public GetByIdsRequestValidator(PaginationOptions paginationConfiguration)
        {
            RuleFor(x => x.Ids).NotNull().Must(x => x != null && x.Count() <= paginationConfiguration.MaxPaginationPageSize);
        }
    }
}
