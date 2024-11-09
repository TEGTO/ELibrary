using Authentication.Models;
using FluentValidation;

namespace UserApi.Validators
{
    public class AccessTokenDataValidator : AbstractValidator<AccessTokenData>
    {
        public AccessTokenDataValidator()
        {
            RuleFor(x => x.AccessToken).NotNull().NotEmpty();
            RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
            RuleFor(x => x.RefreshTokenExpiryDate.ToUniversalTime()).GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
