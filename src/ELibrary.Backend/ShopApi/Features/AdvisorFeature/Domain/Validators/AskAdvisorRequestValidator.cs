using FluentValidation;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;

namespace ShopApi.Features.AdvisorFeature.Domain.Validators
{
    public class AskAdvisorRequestValidator : AbstractValidator<AskAdvisorRequest>
    {
        public AskAdvisorRequestValidator()
        {
            RuleFor(x => x.Message).NotNull().NotEmpty().MaximumLength(2048);
        }
    }
}