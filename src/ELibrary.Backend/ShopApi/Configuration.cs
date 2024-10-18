namespace ShopApi
{
    public static class Configuration
    {
        public static string SHOP_DATABASE_CONNECTION_STRING { get; } = "LibraryShopDb";
        public static string SHOP_MAX_ORDER_AMOUNT { get; } = "Shop:MaxOrderAmount";
        public static string USE_CORS { get; } = "UseCORS";
        public static string CHAT_SERACH_SERVICE_ENDPOINT { get; } = "Chat:SearchServiceEndpoint";
        public static string CHAT_SEARCH_INDEX_NAME { get; } = "Chat:SearchIndexName";
        public static string CHAT_SEARCH_API_KEY { get; } = "Chat:SearchApiKey";
        public static string CHAT_OPEN_AI_ENDPOINT { get; } = "Chat:OpenAiEndpoint";
        public static string CHAT_OPENAI_API_KEY { get; } = "Chat:OpenAiApiKey";
        public static string CHAT_OPENAI_CHAT_MODEL { get; } = "Chat:ChatModel";
        public static string CHAT_OPENAI_GROUNDED_PROMPT { get; } = "Chat:GroundedPrompt";
    }
}