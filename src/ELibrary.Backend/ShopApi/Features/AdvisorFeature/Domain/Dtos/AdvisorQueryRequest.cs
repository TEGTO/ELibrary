using System.Text.Json.Serialization;

namespace ShopApi.Features.AdvisorFeature.Domain.Dtos
{
    public class AdvisorQueryRequest
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }
    }
}
