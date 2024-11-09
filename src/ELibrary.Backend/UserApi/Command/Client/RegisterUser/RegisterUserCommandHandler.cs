using Authentication.Identity;
using AutoMapper;
using ExceptionHandling;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserApi.Services.Auth;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserAuthenticationResponse>
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public RegisterUserCommandHandler(IAuthService authService, IUserService userService, IMapper mapper)
        {
            this.authService = authService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<UserAuthenticationResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var user = mapper.Map<User>(request);

            var errors = new List<IdentityError>();

            var registerParams = new RegisterUserParams(user, request.Password);
            errors.AddRange((await authService.RegisterUserAsync(registerParams, cancellationToken)).Errors);
            if (Utilities.HasErrors(errors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            errors.AddRange(await userService.SetUserRolesAsync(user, new() { Roles.CLIENT }, cancellationToken));
            if (Utilities.HasErrors(errors, out errorResponse)) throw new AuthorizationException(errorResponse);

            var loginParams = new LoginUserParams(request.Email, request.Password);
            var token = await authService.LoginUserAsync(loginParams, cancellationToken);

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await userService.GetUserRolesAsync(user, cancellationToken);

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email,
                Roles = roles,
            };
        }
    }
}
