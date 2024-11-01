using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using Shared.Domain.Dtos;

namespace ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderPaginated
{
    public record GetStockOrderPaginatedQuery(PaginationRequest PaginationRequest) : IRequest<IEnumerable<StockBookOrderResponse>>;
}
