using FluentValidation;
using Shared.Configurations;
using Shared.Validators;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Validators
{
    public class GetOrdersFilterValidator : AbstractValidator<GetOrdersFilter>
    {
        public GetOrdersFilterValidator(PaginationConfiguration paginationConfiguration)
        {
            Include(new PaginationRequestValidator(paginationConfiguration));
            RuleFor(x => x.ClientId).MaximumLength(256);
        }
    }
}
