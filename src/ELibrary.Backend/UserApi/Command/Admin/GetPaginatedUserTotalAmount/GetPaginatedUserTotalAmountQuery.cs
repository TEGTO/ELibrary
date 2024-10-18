using MediatR;
using UserApi.Domain.Dtos;

namespace UserApi.Command.Admin.GetPaginatedUserTotalAmount
{
    public record GetPaginatedUserTotalAmountQuery(AdminGetUserFilter Filter) : IRequest<int>;
}
