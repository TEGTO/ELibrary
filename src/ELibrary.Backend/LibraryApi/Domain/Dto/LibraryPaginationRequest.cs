using Shared.Dtos;

namespace LibraryApi.Domain.Dtos
{
    public class LibraryPaginationRequest : PaginationRequest
    {
        public string ContainsName { get; set; } = string.Empty;
    }
}
