using AutoMapper;
using ExceptionHandling;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserApi.Services.Auth;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Admin.AdminRegisterUser
{
    public class AdminRegisterUserCommandHandler : IRequestHandler<AdminRegisterUserCommand, AdminUserResponse>
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AdminRegisterUserCommandHandler(IAuthService authService, IUserService userService, IMapper mapper)
        {
            this.authService = authService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<AdminUserResponse> Handle(AdminRegisterUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var user = mapper.Map<User>(request);

            var errors = new List<IdentityError>();

            var registerParams = new RegisterUserParams(user, request.Password);
            errors.AddRange((await authService.RegisterUserAsync(registerParams, cancellationToken)).Errors);
            if (Utilities.HasErrors(errors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            errors.AddRange(await userService.SetUserRolesAsync(user, request.Roles, cancellationToken));
            if (Utilities.HasErrors(errors, out errorResponse)) throw new AuthorizationException(errorResponse);

            user = await userService.GetUserByUserInfoAsync(request.Email, cancellationToken);

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await userService.GetUserRolesAsync(user, cancellationToken);
            return response;
        }
    }
}

