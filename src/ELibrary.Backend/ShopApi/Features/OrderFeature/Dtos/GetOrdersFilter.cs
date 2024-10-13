using Shared.Domain.Dtos;

namespace ShopApi.Features.OrderFeature.Dtos
{
    public class GetOrdersFilter : PaginationRequest
    {
        public string? ClientId { get; set; }
    }
}
