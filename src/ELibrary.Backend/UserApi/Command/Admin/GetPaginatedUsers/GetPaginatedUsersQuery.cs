using MediatR;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Command.Admin.GetPaginatedUsers
{
    public record GetPaginatedUsersQuery(AdminGetUserFilter Filter) : IRequest<IEnumerable<AdminUserResponse>>;
}
