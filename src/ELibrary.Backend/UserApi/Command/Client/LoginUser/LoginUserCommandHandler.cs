using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Models;
using UserApi.Services;
using UserApi.Services.Auth;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserAuthenticationResponse>
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly IUserAuthenticationMethodService authMethodService;
        private readonly IMapper mapper;

        public LoginUserCommandHandler(IAuthService authService, IUserService userService, IUserAuthenticationMethodService authMethodService, IMapper mapper)
        {
            this.authService = authService;
            this.userService = userService;
            this.authMethodService = authMethodService;
            this.mapper = mapper;
        }

        public async Task<UserAuthenticationResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            ValidateCommand(command);

            var request = command.Request;
            var user = await userService.GetUserByLoginAsync(request.Login, cancellationToken);
            if (user == null) throw new UnauthorizedAccessException("Invalid authentication! Wrong password or login!");

            var loginParams = new LoginUserModel(user, request.Password);
            var token = await authService.LoginUserAsync(loginParams, cancellationToken);

            await authMethodService.SetUserAuthenticationMethodAsync(user, AuthenticationMethod.BaseAuthentication, cancellationToken);

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await userService.GetUserRolesAsync(user, cancellationToken);

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email ?? "",
                Roles = roles,
            };
        }

        private void ValidateCommand(LoginUserCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var request = command.Request;

            if (string.IsNullOrEmpty(request.Login))
                throw new InvalidDataException("Login can't be null or empty!");

            if (string.IsNullOrEmpty(request.Password))
                throw new InvalidDataException("Passwordcan't be null or empty!");
        }
    }
}
