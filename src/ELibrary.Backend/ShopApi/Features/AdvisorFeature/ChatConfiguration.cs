namespace ShopApi.Features.AdvisorFeature
{
    public class ChatConfiguration
    {
        public string OpenAiApiKey { get; set; } = string.Empty;
        public string DbConnectionString { get; set; } = string.Empty;
        public string ChatModel { get; set; } = string.Empty;
        public string GroundedPrompt { get; set; } = string.Empty;
        public int MaxAmountOfSimmilarDocuments { get; set; } = 5;
        public float ScoreThreshold { get; set; } = 0.5f;
        public string CollectionName { get; set; } = "documents";
        public int CollectionDimensions { get; set; } = 1536;
        public float GetDocumentsCacheTimeInMinutes { get; set; } = 5;
    }
}
