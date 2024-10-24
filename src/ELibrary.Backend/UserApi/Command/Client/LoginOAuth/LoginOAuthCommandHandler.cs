using Authentication.Identity;
using Authentication.OAuth;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Exceptions;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserApi.Services.Auth;
using UserApi.Services.OAuth;

namespace UserApi.Command.Client.LoginOAuth
{
    public class LoginOAuthCommandHandler : IRequestHandler<LoginOAuthCommand, UserAuthenticationResponse>
    {
        private readonly Dictionary<OAuthLoginProvider, IOAuthService> oAuthServices;
        private readonly IUserService userService;
        private readonly IAuthService authService;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public LoginOAuthCommandHandler(Dictionary<OAuthLoginProvider, IOAuthService> oAuthServices, IAuthService authService, IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            this.oAuthServices = oAuthServices;
            this.authService = authService;
            this.userService = userService;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<UserAuthenticationResponse> Handle(LoginOAuthCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var token = await oAuthServices[request.OAuthLoginProvider].GetAccessOnCodeAsync(
                new GetAccessOnCodeParams(request.Code, request.CodeVerifier, request.RedirectUrl), cancellationToken);

            var principal = tokenService.GetPrincipalFromToken(token.AccessToken);
            var user = await userService.GetUserByUserInfoAsync(principal.Identity.Name, cancellationToken);
            if (user == null) throw new UnauthorizedAccessException("Invalid authentication!");

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await userService.GetUserRolesAsync(user, cancellationToken);

            if (!roles.Any())
            {
                var errors = new List<IdentityError>();
                errors.AddRange(await userService.SetUserRolesAsync(user, new() { Roles.CLIENT }, cancellationToken));
                if (Utilities.HasErrors(errors, out var errorResponse)) throw new AuthorizationException(errorResponse);

                token = await authService.RefreshTokenAsync(token, cancellationToken);
                tokenDto = mapper.Map<AuthToken>(token);
            }

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email,
                Roles = roles,
            };
        }
    }
}
