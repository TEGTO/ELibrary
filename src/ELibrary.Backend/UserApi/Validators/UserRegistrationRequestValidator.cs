﻿using UserApi.Domain.Dtos;
using FluentValidation;

namespace UserApi.Validators
{
    public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationRequestValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(8).MaximumLength(256);
            RuleFor(x => x.ConfirmPassword).Must((model, field) => field == model.Password)
                .WithMessage("Passwords do not match.").MaximumLength(256);
        }
    }
}