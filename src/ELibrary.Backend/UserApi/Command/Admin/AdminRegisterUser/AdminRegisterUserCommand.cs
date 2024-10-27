using MediatR;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Command.Admin.AdminRegisterUser
{
    public record AdminRegisterUserCommand(AdminUserRegistrationRequest Request) : IRequest<AdminUserResponse>;
}
