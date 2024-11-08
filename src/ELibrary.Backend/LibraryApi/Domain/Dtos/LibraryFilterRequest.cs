using Pagination;

namespace LibraryApi.Domain.Dtos
{
    public class LibraryFilterRequest : PaginationRequest
    {
        public string ContainsName { get; set; } = string.Empty;
    }
}
