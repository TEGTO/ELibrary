using MediatR;
using UserApi.Domain.Dtos;

namespace UserApi.Command.Client.RefreshToken
{
    public record RefreshTokenCommand(AuthToken Request) : IRequest<AuthToken>;
}
