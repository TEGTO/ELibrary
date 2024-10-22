using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;

namespace UserApi.Command.Admin.GetUserByInfo
{
    public class GetUserByInfoQueryHandler : IRequestHandler<GetUserByInfoQuery, AdminUserResponse>
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public GetUserByInfoQueryHandler(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<AdminUserResponse> Handle(GetUserByInfoQuery command, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserByUserInfoAsync(command.Info);

            if (user == null)
            {
                throw new KeyNotFoundException("User is not found!");
            }

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await userService.GetUserRolesAsync(user);
            return response;
        }
    }
}
