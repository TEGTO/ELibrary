using AutoMapper;
using MediatR;
using Shared.Exceptions;
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
            var updateData = mapper.Map<UserUpdateData>(command.Request);
            var user = await userService.GetUserAsync(command.UserPricipal, cancellationToken);

            var identityErrors = await userService.UpdateUserAsync(user, updateData, false, cancellationToken);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            return Unit.Value;
        }
    }
}
