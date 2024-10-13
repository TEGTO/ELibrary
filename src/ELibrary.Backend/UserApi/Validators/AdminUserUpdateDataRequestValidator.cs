using FluentValidation;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.Validators
{
    public class AdminUserUpdateDataRequestValidator : AbstractValidator<AdminUserUpdateDataRequest>
    {
        public AdminUserUpdateDataRequestValidator()
        {
            RuleFor(x => x.CurrentLogin).NotNull().NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Password).MinimumLength(8).When(x => !string.IsNullOrEmpty(x.Password)).MaximumLength(256);
            RuleFor(x => x.ConfirmPassword).Must((model, field) => field == model.Password)
                .When(x => !string.IsNullOrEmpty(x.Password)).WithMessage("Passwords do not match.");
            RuleFor(x => x.Roles).NotNull().NotEmpty();
        }
    }
}
