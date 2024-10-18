using Authentication.Models;
using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos;
using UserApi.Services;

namespace UserApi.Command.Client.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthToken>
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;
        private readonly double expiryInDays;

        public RefreshTokenCommandHandler(IAuthService authService, IMapper mapper, IConfiguration configuration)
        {
            this.authService = authService;
            this.mapper = mapper;
            expiryInDays = double.Parse(configuration[Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS]!);
        }

        public async Task<AuthToken> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var tokenData = mapper.Map<AccessTokenData>(command.Request);
            var newToken = await authService.RefreshTokenAsync(tokenData, expiryInDays);
            return mapper.Map<AuthToken>(newToken);
        }
    }
}
