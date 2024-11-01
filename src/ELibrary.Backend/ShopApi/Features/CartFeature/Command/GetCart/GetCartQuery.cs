using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;

namespace ShopApi.Features.CartFeature.Command.GetCart
{
    public record GetCartQuery(string UserId) : IRequest<CartResponse>;
}
