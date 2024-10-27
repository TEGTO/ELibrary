using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserApi.Services.Auth;

namespace UserApi.Command.Client.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserAuthenticationResponse>
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public LoginUserCommandHandler(IAuthService authService, IUserService userService, IMapper mapper)
        {
            this.authService = authService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<UserAuthenticationResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var user = await userService.GetUserByUserInfoAsync(request.Login, cancellationToken);
            if (user == null) throw new UnauthorizedAccessException("Invalid authentication! Wrong password or login!");

            var loginParams = new LoginUserParams(request.Login, request.Password);
            var token = await authService.LoginUserAsync(loginParams, cancellationToken);

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await userService.GetUserRolesAsync(user, cancellationToken);

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email,
                Roles = roles,
            };
        }
    }
}
