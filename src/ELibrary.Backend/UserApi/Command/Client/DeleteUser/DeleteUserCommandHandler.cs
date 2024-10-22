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
            var user = await userService.GetUserAsync(request.ClaimsPrincipal);
            await userService.DeleteUserAsync(user);
            return Unit.Value;
        }
    }
}
