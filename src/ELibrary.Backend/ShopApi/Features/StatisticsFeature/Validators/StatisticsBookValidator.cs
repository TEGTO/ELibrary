using FluentValidation;
using ShopApi.Features.StatisticsFeature.Domain.Models;

namespace ShopApi.Features.StatisticsFeature.Validators
{
    public class StatisticsBookValidator : AbstractValidator<StatisticsBook>
    {
        public StatisticsBookValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
