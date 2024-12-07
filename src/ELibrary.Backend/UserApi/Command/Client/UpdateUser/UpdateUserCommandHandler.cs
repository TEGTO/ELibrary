using AutoMapper;
using ExceptionHandling;
using MediatR;
using UserApi.Domain.Models;
using UserApi.Services;

namespace UserApi.Command.Client.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UpdateUserCommandHandler(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var updateData = mapper.Map<UserUpdateModel>(command.Request);
            var user = await userService.GetUserAsync(command.UserPricipal, cancellationToken);

            if (user == null)
            {
                throw new InvalidOperationException("User to update is not found!");
            }

            var identityErrors = await userService.UpdateUserAsync(user, updateData, false, cancellationToken);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            return Unit.Value;
        }
    }
}
