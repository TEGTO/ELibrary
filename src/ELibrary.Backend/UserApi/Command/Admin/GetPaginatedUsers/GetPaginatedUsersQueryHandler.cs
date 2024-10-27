using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;

namespace UserApi.Command.Admin.GetPaginatedUsers
{
    public class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, IEnumerable<AdminUserResponse>>
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public GetPaginatedUsersQueryHandler(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AdminUserResponse>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userService.GetPaginatedUsersAsync(request.Filter, cancellationToken);

            var responses = new List<AdminUserResponse>();

            foreach (var user in users)
            {
                var response = mapper.Map<AdminUserResponse>(user);
                response.Roles = await userService.GetUserRolesAsync(user, cancellationToken);
                responses.Add(response);
            }

            return responses;
        }
    }
}
