using MediatR;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Command.Client.LoginUser
{
    public record LoginUserCommand(UserAuthenticationRequest Request) : IRequest<UserAuthenticationResponse>;
}
