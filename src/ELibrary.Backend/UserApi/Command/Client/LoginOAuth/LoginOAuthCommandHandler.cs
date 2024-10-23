using Authentication.OAuth;
using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserApi.Services.OAuth;

namespace UserApi.Command.Client.LoginOAuth
{
    public class LoginOAuthCommandHandler : IRequestHandler<LoginOAuthCommand, UserAuthenticationResponse>
    {
        private readonly Dictionary<OAuthLoginProvider, IOAuthService> oAuthServices;
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public LoginOAuthCommandHandler(Dictionary<OAuthLoginProvider, IOAuthService> oAuthServices, IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            this.oAuthServices = oAuthServices;
            this.userService = userService;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<UserAuthenticationResponse> Handle(LoginOAuthCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var token = await oAuthServices[request.OAuthLoginProvider].GetAccessOnCodeAsync(
                new GetAccessOnCodeParams(request.Code, request.CodeVerifier, request.RedirectUrl), cancellationToken);

            var principal = tokenService.GetPrincipalFromExpiredToken(token.AccessToken);
            var user = await userService.GetUserByUserInfoAsync(principal.Identity.Name);
            if (user == null) throw new UnauthorizedAccessException("Invalid authentication!");

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await userService.GetUserRolesAsync(user);

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email,
                Roles = roles,
                LoginProvider = user.LoginProvider
            };
        }
    }
}
