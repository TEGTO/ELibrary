using Shared.Dtos;

namespace UserApi.Domain.Dtos.Requests
{
    /// <summary>
    /// ContainsInfo - login, username, email, id, etc...
    /// </summary>
    public class GetUserFilterRequest : PaginationRequest
    {
        public string ContainsInfo { get; set; }
    }
}
