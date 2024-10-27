using FluentValidation;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.Validators
{
    public class OAuthGetUrlQueryParamsValidator : AbstractValidator<GetOAuthUrlQueryParams>
    {
        public OAuthGetUrlQueryParamsValidator()
        {
            RuleFor(x => x.RedirectUrl).NotNull().NotEmpty().MaximumLength(1024);
            RuleFor(x => x.CodeVerifier).NotNull().NotEmpty().MaximumLength(1024);
        }
    }
}
