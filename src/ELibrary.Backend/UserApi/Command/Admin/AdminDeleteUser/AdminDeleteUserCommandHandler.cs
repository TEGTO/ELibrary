using MediatR;
using Shared.Exceptions;
using UserApi.Services;

namespace UserApi.Command.Admin.AdminDeleteUser
{
    public class AdminDeleteUserCommandHandler : IRequestHandler<AdminDeleteUserCommand, Unit>
    {
        private readonly IAuthService authService;

        public AdminDeleteUserCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<Unit> Handle(AdminDeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await authService.GetUserByUserInfoAsync(command.Info);
            var result = await authService.DeleteUserAsync(user);
            if (Utilities.HasErrors(result.Errors.ToList(), out var errorResponse)) throw new AuthorizationException(errorResponse);
            return Unit.Value;
        }
    }
}
