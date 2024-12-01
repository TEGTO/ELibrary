using Authentication.Identity;
using Authentication.OAuth;
using AutoMapper;
using ExceptionHandling;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Models;
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
            ValidateCommand(command);

            var request = command.Request;

            var token = await oAuthServices[request.OAuthLoginProvider].GetAccessOnCodeAsync(
                new GetAccessOnCodeParams(request.Code!, request.CodeVerifier, request.RedirectUrl), cancellationToken);

            if (string.IsNullOrEmpty(token.AccessToken))
                throw new InvalidOperationException("Access token os null or empty!");

            var principal = tokenService.GetPrincipalFromToken(token.AccessToken);

            if (principal == null || principal.Identity == null || string.IsNullOrEmpty(principal.Identity.Name))
                throw new InvalidOperationException("Claims principal are invalid!");

            var user = await userService.GetUserByLoginAsync(principal.Identity.Name, cancellationToken);
            if (user == null) throw new UnauthorizedAccessException("Invalid authentication!");

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await userService.GetUserRolesAsync(user, cancellationToken);

            if (roles.Count == 0)
            {
                var errors = new List<IdentityError>();
                errors.AddRange(await userService.SetUserRolesAsync(user, new() { Roles.CLIENT }, cancellationToken));
                if (Utilities.HasErrors(errors, out var errorResponse)) throw new AuthorizationException(errorResponse);

                var refreshTokenParams = new RefreshTokenModel(user, token);
                token = await authService.RefreshTokenAsync(refreshTokenParams, cancellationToken);

                tokenDto = mapper.Map<AuthToken>(token);
                roles = [Roles.CLIENT];
            }

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email ?? "",
                Roles = roles,
            };
        }

        private void ValidateCommand(LoginOAuthCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var request = command.Request;

            if (string.IsNullOrEmpty(request.Code))
                throw new InvalidDataException("Code can't be null or empty!");

            if (string.IsNullOrEmpty(request.Code))
                throw new InvalidDataException("Code verifier can't be null or empty!");

            if (string.IsNullOrEmpty(request.Code))
                throw new InvalidDataException("Redirect url can't be null or empty!");
        }
    }
}
