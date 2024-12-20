﻿using Authentication.Identity;
using AutoMapper;
using ExceptionHandling;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Models;
using UserApi.Services;
using UserApi.Services.Auth;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserAuthenticationResponse>
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly IUserAuthenticationMethodService authMethodService;
        private readonly IMapper mapper;

        public RegisterUserCommandHandler(IAuthService authService, IUserService userService, IUserAuthenticationMethodService authMethodService, IMapper mapper)
        {
            this.authService = authService;
            this.userService = userService;
            this.authMethodService = authMethodService;
            this.mapper = mapper;
        }

        public async Task<UserAuthenticationResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);

            var request = command.Request;

            var user = mapper.Map<User>(request);

            var errors = new List<IdentityError>();

            var registerParams = new RegisterUserModel(user, request.Password);
            errors.AddRange((await authService.RegisterUserAsync(registerParams, cancellationToken)).Errors);
            if (Utilities.HasErrors(errors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            errors.AddRange(await userService.SetUserRolesAsync(user, new() { Roles.CLIENT }, cancellationToken));
            if (Utilities.HasErrors(errors, out errorResponse)) throw new AuthorizationException(errorResponse);

            var loginParams = new LoginUserModel(user, request.Password);
            var token = await authService.LoginUserAsync(loginParams, cancellationToken);

            await authMethodService.SetUserAuthenticationMethodAsync(user, AuthenticationMethod.BaseAuthentication, cancellationToken);

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await userService.GetUserRolesAsync(user, cancellationToken);

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email ?? "",
                Roles = roles,
            };
        }

        private void ValidateCommand(RegisterUserCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var request = command.Request;

            if (string.IsNullOrEmpty(request.Password))
                throw new InvalidDataException("Passwordcan't be null or empty!");
        }
    }
}
