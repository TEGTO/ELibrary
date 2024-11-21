using MediatR;
using UserApi.Services;

namespace UserApi.Command.Client.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserService userService;

        public DeleteUserCommandHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserAsync(request.ClaimsPrincipal, cancellationToken);

            if (user == null) { throw new InvalidOperationException("User to delete is not found!"); }

            await userService.DeleteUserAsync(user, cancellationToken);
            return Unit.Value;
        }
    }
}
