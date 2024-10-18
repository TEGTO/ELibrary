using MediatR;

namespace UserApi.Command.Admin.AdminDeleteUser
{
    public record AdminDeleteUserCommand(string Info) : IRequest<Unit>;
}
