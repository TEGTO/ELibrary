using Authentication.Models;
using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos;
using UserApi.Domain.Models;
using UserApi.Services;
using UserApi.Services.Auth;

namespace UserApi.Command.Client.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthToken>
    {
        private readonly IAuthService authService;
        private readonly ITokenService tokenService;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public RefreshTokenCommandHandler(IAuthService authService, IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            this.authService = authService;
            this.tokenService = tokenService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<AuthToken> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);

            var tokenData = mapper.Map<AccessTokenData>(command.Request);

            var principal = tokenService.GetPrincipalFromToken(tokenData.AccessToken);

            var user = await userService.GetUserAsync(principal, cancellationToken);

            if (user == null)
            {
                throw new InvalidOperationException("User is not found by access token!");
            }

            var refreshParams = new RefreshTokenModel(user, tokenData);
            var newToken = await authService.RefreshTokenAsync(refreshParams, cancellationToken);

            return mapper.Map<AuthToken>(newToken);
        }

        private void ValidateCommand(RefreshTokenCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var request = command.Request;

            if (string.IsNullOrEmpty(request.AccessToken))
                throw new InvalidOperationException("Trying to refresh empty access token!");
        }
    }
}
