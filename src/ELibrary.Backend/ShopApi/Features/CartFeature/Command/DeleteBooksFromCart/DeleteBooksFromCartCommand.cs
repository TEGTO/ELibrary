using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.CartFeature.Dtos;

namespace ShopApi.Features.CartFeature.Command.DeleteBooksFromCart
{
    public record DeleteBooksFromCartCommand(string UserId, DeleteCartBookFromCartRequest[] Requests) : IRequest<CartResponse>;
}
