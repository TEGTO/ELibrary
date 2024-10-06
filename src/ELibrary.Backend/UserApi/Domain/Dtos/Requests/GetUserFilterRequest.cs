using Shared.Dtos;

namespace UserApi.Domain.Dtos.Requests
{
    public class GetUserFilterRequest : PaginationRequest
    {
        public string ContainsString { get; set; }
    }
}
