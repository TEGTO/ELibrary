using Authentication.Identity;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Exceptions;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserAuthenticationResponse>
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly double expiryInDays;

        public RegisterUserCommandHandler(IAuthService authService, IUserService userService, IMapper mapper, IConfiguration configuration)
        {
            this.authService = authService;
            this.userService = userService;
            this.mapper = mapper;
            expiryInDays = double.Parse(configuration[Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS]!);
        }

        public async Task<UserAuthenticationResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var user = mapper.Map<User>(request);
            var errors = new List<IdentityError>();

            var registerParams = new RegisterUserParams(user, request.Password);
            errors.AddRange((await authService.RegisterUserAsync(registerParams)).Errors);
            if (Utilities.HasErrors(errors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            errors.AddRange(await userService.SetUserRolesAsync(user, new() { Roles.CLIENT }));
            if (Utilities.HasErrors(errors, out errorResponse)) throw new AuthorizationException(errorResponse);

            var loginParams = new LoginUserParams(request.Email, request.Password, expiryInDays);
            var token = await authService.LoginUserAsync(loginParams);

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await userService.GetUserRolesAsync(user);

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email,
                Roles = roles
            };
        }
    }
}
