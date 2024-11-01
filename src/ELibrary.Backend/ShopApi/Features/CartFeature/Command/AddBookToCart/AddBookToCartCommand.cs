using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.CartFeature.Dtos;

namespace ShopApi.Features.CartFeature.Command.AddBookToCart
{
    public record AddBookToCartCommand(string UserId, AddBookToCartRequest Request) : IRequest<BookListingResponse>;
}
