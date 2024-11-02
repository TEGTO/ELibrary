using FluentValidation;
using ShopApi.Features.StatisticsFeature.Domain.Dtos;

namespace ShopApi.Features.StatisticsFeature.Validators
{
    public class GetBookStatisticsRequestValidator : AbstractValidator<GetShopStatisticsRequest>
    {
        public GetBookStatisticsRequestValidator()
        {
            RuleFor(x => x.FromUTC).LessThanOrEqualTo(x => x.ToUTC).When(x => x.FromUTC != null && x.ToUTC != null);
            RuleFor(x => x.ToUTC).GreaterThanOrEqualTo(x => x.FromUTC).When(x => x.FromUTC != null && x.ToUTC != null);
            RuleFor(x => x.IncludeBooks).NotNull();
        }
    }
}
