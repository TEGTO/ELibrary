using FluentValidation;
using Pagination;
using ShopApi.Features.OrderFeature.Dtos;

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
