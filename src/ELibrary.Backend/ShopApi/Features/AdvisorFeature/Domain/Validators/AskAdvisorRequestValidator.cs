using FluentValidation;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;

namespace ShopApi.Features.AdvisorFeature.Domain.Validators
{
    public class AskAdvisorRequestValidator : AbstractValidator<AdvisorQueryRequest>
    {
        public AskAdvisorRequestValidator()
        {
            RuleFor(x => x.Query).NotNull().NotEmpty().MaximumLength(2048);
        }
    }
}