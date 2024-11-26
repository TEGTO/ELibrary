using System.Text.Json.Serialization;

namespace ShopApi.Features.AdvisorFeature.Domain.Dtos
{
    public class ChatAdvisorQueryRequest
    {
        [JsonPropertyName("query")]
        public string Query { get; set; } = string.Empty;
    }
}
