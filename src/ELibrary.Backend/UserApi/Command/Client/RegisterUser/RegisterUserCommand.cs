using MediatR;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Command.Client.RegisterUser
{
    public record RegisterUserCommand(UserRegistrationRequest Request) : IRequest<UserAuthenticationResponse>;
}
