using AutoMapper;
using MediatR;
using Shared.Exceptions;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Models;
using UserApi.Services;

namespace UserApi.Command.Admin.AdminUpdateUser
{
    public class AdminUpdateUserCommandHandler : IRequestHandler<AdminUpdateUserCommand, AdminUserResponse>
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public AdminUpdateUserCommandHandler(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        public async Task<AdminUserResponse> Handle(AdminUpdateUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var updateData = mapper.Map<UserUpdateData>(request);
            var user = await authService.GetUserByUserInfoAsync(request.CurrentLogin);

            var identityErrors = await authService.UpdateUserAsync(user, updateData, true);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            identityErrors = await authService.SetUserRolesAsync(user, request.Roles);
            if (Utilities.HasErrors(identityErrors, out errorResponse)) throw new AuthorizationException(errorResponse);

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await authService.GetUserRolesAsync(user);

            return response;
        }
    }
}
