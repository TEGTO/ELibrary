using ExceptionHandling;
using MediatR;
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
            var user = await userService.GetUserByLoginAsync(command.Info, cancellationToken);

            if (user == null) { throw new InvalidOperationException("User to delete is not found!"); }

            var result = await userService.DeleteUserAsync(user, cancellationToken);
            if (Utilities.HasErrors(result.Errors.ToList(), out var errorResponse)) throw new AuthorizationException(errorResponse);
            return Unit.Value;
        }
    }
}
