namespace ShopApi.Features.AdvisorFeature
{
    public class ChatConfiguration
    {
        public string SearchServiceEndpoint { get; set; } = string.Empty;
        public string SearchIndexName { get; set; } = string.Empty;
        public string SearchApiKey { get; set; } = string.Empty;
        public string OpenAiApiKey { get; set; } = string.Empty;
        public string ChatModel { get; set; } = string.Empty;
        public string GroundedPrompt { get; set; } = string.Empty;
    }
}
