using FluentValidation;
using LibraryShopEntities.Filters;
using Pagination;

namespace ShopApi.Features.OrderFeature.Validators
{
    public class GetOrdersFilterValidator : AbstractValidator<GetOrdersFilter>
    {
        public GetOrdersFilterValidator(PaginationOptions paginationConfiguration)
        {
            Include(new PaginationRequestValidator(paginationConfiguration));
            RuleFor(x => x.ClientId).MaximumLength(256);
        }
    }
}
