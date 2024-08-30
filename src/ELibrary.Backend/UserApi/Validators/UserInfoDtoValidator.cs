using FluentValidation;
using UserApi.Domain.Dtos;

namespace UserApi.Validators
{
    public class UserInfoDtoValidator : AbstractValidator<UserInfoDto>
    {
        public UserInfoDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.LastName).NotNull().MaximumLength(256);
            RuleFor(x => x.Address).NotNull().MaximumLength(256);
        }
    }
}
