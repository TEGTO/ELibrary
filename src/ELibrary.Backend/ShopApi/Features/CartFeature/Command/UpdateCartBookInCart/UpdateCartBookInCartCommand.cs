using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.CartFeature.Dtos;

namespace ShopApi.Features.CartFeature.Command.UpdateCartBookInCart
{
    public record UpdateCartBookInCartCommand(string UserId, UpdateCartBookRequest Request) : IRequest<BookListingResponse>;
}
