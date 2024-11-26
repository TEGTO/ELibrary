using AutoMapper;
using ExceptionHandling;
using MediatR;
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
            ValidateCommand(command);

            var request = command.Request;

            var updateData = mapper.Map<UserUpdateData>(request);
            var user = await userService.GetUserByLoginAsync(request.CurrentLogin!, cancellationToken);

            if (user == null) { throw new InvalidDataException("User to update is not found!"); }

            var identityErrors = await userService.UpdateUserAsync(user, updateData, true, cancellationToken);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            identityErrors = await userService.SetUserRolesAsync(user, request.Roles, cancellationToken);
            if (Utilities.HasErrors(identityErrors, out errorResponse)) throw new AuthorizationException(errorResponse);

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await userService.GetUserRolesAsync(user, cancellationToken);

            return response;
        }

        private void ValidateCommand(AdminUpdateUserCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var request = command.Request;

            if (string.IsNullOrEmpty(request.CurrentLogin))
                throw new InvalidDataException("Login can't be null or empty!");
        }
    }
}
