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
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AdminUpdateUserCommandHandler(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<AdminUserResponse> Handle(AdminUpdateUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var updateData = mapper.Map<UserUpdateData>(request);
            var user = await userService.GetUserByUserInfoAsync(request.CurrentLogin);

            var identityErrors = await userService.UpdateUserAsync(user, updateData, true);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            identityErrors = await userService.SetUserRolesAsync(user, request.Roles);
            if (Utilities.HasErrors(identityErrors, out errorResponse)) throw new AuthorizationException(errorResponse);

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await userService.GetUserRolesAsync(user);

            return response;
        }
    }
}
