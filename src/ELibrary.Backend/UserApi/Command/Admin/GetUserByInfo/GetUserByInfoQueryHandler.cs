using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;

namespace UserApi.Command.Admin.GetUserByInfo
{
    public class GetUserByInfoQueryHandler : IRequestHandler<GetUserByInfoQuery, AdminUserResponse>
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public GetUserByInfoQueryHandler(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        public async Task<AdminUserResponse> Handle(GetUserByInfoQuery command, CancellationToken cancellationToken)
        {
            var user = await authService.GetUserByUserInfoAsync(command.Info);

            if (user == null)
            {
                throw new KeyNotFoundException("User is not found!");
            }

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await authService.GetUserRolesAsync(user);
            return response;
        }
    }
}
