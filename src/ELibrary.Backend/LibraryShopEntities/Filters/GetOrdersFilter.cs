using Pagination;

namespace LibraryShopEntities.Filters
{
    public class GetOrdersFilter : PaginationRequest
    {
        public string? ClientId { get; set; }
    }
}
