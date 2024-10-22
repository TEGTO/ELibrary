using MediatR;
using Shared.Exceptions;
using UserApi.Services;

namespace UserApi.Command.Admin.AdminDeleteUser
{
    public class AdminDeleteUserCommandHandler : IRequestHandler<AdminDeleteUserCommand, Unit>
    {
        private readonly IUserService userService;

        public AdminDeleteUserCommandHandler(IUserService userServicee)
        {
            this.userService = userServicee;
        }

        public async Task<Unit> Handle(AdminDeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserByUserInfoAsync(command.Info);
            var result = await userService.DeleteUserAsync(user);
            if (Utilities.HasErrors(result.Errors.ToList(), out var errorResponse)) throw new AuthorizationException(errorResponse);
            return Unit.Value;
        }
    }
}
