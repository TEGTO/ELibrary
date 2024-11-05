using FluentValidation;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;

namespace ShopApi.Features.AdvisorFeature.Domain.Validators
{
    public class AdvisorQueryRequestValidator : AbstractValidator<AdvisorQueryRequest>
    {
        public AdvisorQueryRequestValidator()
        {
            RuleFor(x => x.Query).NotNull().NotEmpty().MaximumLength(2048);
        }
    }
}