using MediatR;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Command.Client.LoginOAuth
{
    public record LoginOAuthCommand(LoginOAuthRequest Request) : IRequest<UserAuthenticationResponse>;
}
