using MediatR;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Command.Admin.AdminUpdateUser
{
    public record AdminUpdateUserCommand(AdminUserUpdateDataRequest Request) : IRequest<AdminUserResponse>;
}
