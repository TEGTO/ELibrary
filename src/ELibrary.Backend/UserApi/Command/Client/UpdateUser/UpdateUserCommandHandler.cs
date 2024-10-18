using AutoMapper;
using MediatR;
using Shared.Exceptions;
using UserApi.Domain.Models;
using UserApi.Services;

namespace UserApi.Command.Client.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public UpdateUserCommandHandler(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var updateData = mapper.Map<UserUpdateData>(command.Request);
            var user = await authService.GetUserAsync(command.UserPricipal);

            var identityErrors = await authService.UpdateUserAsync(user, updateData, false);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            return Unit.Value;
        }
    }
}
