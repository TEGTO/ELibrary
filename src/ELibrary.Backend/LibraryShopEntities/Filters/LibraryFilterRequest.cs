using Pagination;

namespace LibraryShopEntities.Filters
{
    public class LibraryFilterRequest : PaginationRequest
    {
        public string ContainsName { get; set; } = string.Empty;
    }
}
