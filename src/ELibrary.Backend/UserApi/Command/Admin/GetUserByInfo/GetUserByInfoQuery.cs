using MediatR;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Command.Admin.GetUserByInfo
{
    public record GetUserByInfoQuery(string Info) : IRequest<AdminUserResponse>;
}
