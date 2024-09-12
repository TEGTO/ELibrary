using Shared.Dtos;

namespace LibraryShopEntities.Domain.Dtos
{
    public class LibraryPaginationRequest : PaginationRequest
    {
        public string ContainsName { get; set; } = string.Empty;
    }
}
