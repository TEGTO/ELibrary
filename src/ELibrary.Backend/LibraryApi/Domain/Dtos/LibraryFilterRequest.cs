using Shared.Domain.Dtos;

namespace LibraryApi.Domain.Dtos
{
    public class LibraryFilterRequest : PaginationRequest
    {
        public string ContainsName { get; set; } = string.Empty;
    }
}
