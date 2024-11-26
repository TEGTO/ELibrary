using Pagination;

namespace UserApi.Domain.Dtos
{
    /// <summary>
    /// ContainsInfo - login, username, email, id, etc...
    /// </summary>
    public class AdminGetUserFilter : PaginationRequest
    {
        public string? ContainsInfo { get; set; }
    }
}
