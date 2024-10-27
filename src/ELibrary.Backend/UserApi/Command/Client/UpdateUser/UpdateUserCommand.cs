using MediatR;
using System.Security.Claims;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.Command.Client.UpdateUser
{
    public record UpdateUserCommand(UserUpdateDataRequest Request, ClaimsPrincipal UserPricipal) : IRequest<Unit>;
}
