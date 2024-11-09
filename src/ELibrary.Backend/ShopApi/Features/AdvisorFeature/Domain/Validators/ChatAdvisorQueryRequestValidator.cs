using FluentValidation;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;

namespace ShopApi.Features.AdvisorFeature.Domain.Validators
{
    public class ChatAdvisorQueryRequestValidator : AbstractValidator<ChatAdvisorQueryRequest>
    {
        public ChatAdvisorQueryRequestValidator()
        {
            RuleFor(x => x.Query).NotNull().NotEmpty().MaximumLength(2048);
        }
    }
}
