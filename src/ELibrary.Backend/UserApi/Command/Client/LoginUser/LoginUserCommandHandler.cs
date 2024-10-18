using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;

namespace UserApi.Command.Client.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserAuthenticationResponse>
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;
        private readonly double expiryInDays;

        public LoginUserCommandHandler(IAuthService authService, IMapper mapper, IConfiguration configuration)
        {
            this.authService = authService;
            this.mapper = mapper;
            expiryInDays = double.Parse(configuration[Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS]!);
        }

        public async Task<UserAuthenticationResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var user = await authService.GetUserByUserInfoAsync(request.Login);
            if (user == null) throw new UnauthorizedAccessException("Invalid authentication! Wrong password or login!");

            var loginParams = new LoginUserParams(request.Login, request.Password, expiryInDays);
            var token = await authService.LoginUserAsync(loginParams);

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await authService.GetUserRolesAsync(user);

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email,
                Roles = roles
            };
        }
    }
}
