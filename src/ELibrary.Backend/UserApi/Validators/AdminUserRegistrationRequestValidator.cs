using FluentValidation;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.Validators
{
    public class AdminUserRegistrationRequestValidator : AbstractValidator<AdminUserRegistrationRequest>
    {
        public AdminUserRegistrationRequestValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(8).MaximumLength(256);
            RuleFor(x => x.ConfirmPassword).Must((model, field) => field == model.Password)
                .WithMessage("Passwords do not match.").MaximumLength(256);
            RuleFor(x => x.Roles).NotNull().NotEmpty();
        }
    }
}
