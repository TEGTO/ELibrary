using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Exceptions;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Admin.AdminRegisterUser
{
    public class AdminRegisterUserCommandHandler : IRequestHandler<AdminRegisterUserCommand, AdminUserResponse>
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public AdminRegisterUserCommandHandler(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        public async Task<AdminUserResponse> Handle(AdminRegisterUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var user = mapper.Map<User>(request);
            var errors = new List<IdentityError>();

            var registerParams = new RegisterUserParams(user, request.Password);
            errors.AddRange((await authService.RegisterUserAsync(registerParams)).Errors);
            if (Utilities.HasErrors(errors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            errors.AddRange(await authService.SetUserRolesAsync(user, request.Roles));
            if (Utilities.HasErrors(errors, out errorResponse)) throw new AuthorizationException(errorResponse);

            user = await authService.GetUserByUserInfoAsync(request.Email);

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await authService.GetUserRolesAsync(user);
            return response;
        }
    }
}

