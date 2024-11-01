using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetOrderById
{
    public record ManagerGetOrderByIdQuery(int OrderId) : IRequest<OrderResponse?>;
}
