using MediatR;
using UserApi.Services;

namespace UserApi.Command.Client.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IAuthService authService;

        public DeleteUserCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await authService.GetUserAsync(request.ClaimsPrincipal);
            await authService.DeleteUserAsync(user);
            return Unit.Value;
        }
    }
}
