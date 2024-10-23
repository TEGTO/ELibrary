using Authentication.Models;
using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos;
using UserApi.Services.Auth;

namespace UserApi.Command.Client.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthToken>
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public RefreshTokenCommandHandler(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        public async Task<AuthToken> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var tokenData = mapper.Map<AccessTokenData>(command.Request);
            var newToken = await authService.RefreshTokenAsync(tokenData, cancellationToken);
            return mapper.Map<AuthToken>(newToken);
        }
    }
}
