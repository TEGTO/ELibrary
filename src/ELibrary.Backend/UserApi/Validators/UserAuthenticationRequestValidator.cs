﻿using UserApi.Domain.Dtos;
using FluentValidation;

namespace UserApi.Validators
{
    public class UserAuthenticationRequestValidator : AbstractValidator<UserAuthenticationRequest>
    {
        public UserAuthenticationRequestValidator()
        {
            RuleFor(x => x.Login).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(8).MaximumLength(256);
        }
    }
}