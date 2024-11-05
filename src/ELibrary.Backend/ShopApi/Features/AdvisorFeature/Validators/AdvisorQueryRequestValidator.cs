using FluentValidation;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;

namespace ShopApi.Features.AdvisorFeature.Validators
{
    public class AdvisorQueryRequestValidator : AbstractValidator<AdvisorQueryRequest>
    {
        public AdvisorQueryRequestValidator()
        {
            RuleFor(x => x.Query).NotNull().MaximumLength(2048);
        }
    }
}
