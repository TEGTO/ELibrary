using MediatR;
using System.Security.Claims;

namespace UserApi.Command.Client.DeleteUser
{
    public record DeleteUserCommand(ClaimsPrincipal ClaimsPrincipal) : IRequest<Unit>;
}
