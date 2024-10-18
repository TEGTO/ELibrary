using AutoMapper;
using MediatR;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;

namespace UserApi.Command.Admin.GetPaginatedUsers
{
    public class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, IEnumerable<AdminUserResponse>>
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public GetPaginatedUsersQueryHandler(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AdminUserResponse>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await authService.GetPaginatedUsersAsync(request.Filter, cancellationToken);

            var responses = new List<AdminUserResponse>();

            foreach (var user in users)
            {
                var response = mapper.Map<AdminUserResponse>(user);
                response.Roles = await authService.GetUserRolesAsync(user);
                responses.Add(response);
            }

            return responses;
        }
    }
}
