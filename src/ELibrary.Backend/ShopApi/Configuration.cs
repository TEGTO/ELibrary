namespace ShopApi
{
    public static class Configuration
    {
        public static string SHOP_DATABASE_CONNECTION_STRING { get; } = "LibraryShopDb";
        public static string SHOP_MAX_ORDER_AMOUNT { get; } = "Shop:MaxOrderAmount";
        public static string USE_CORS { get; } = "UseCORS";
        public static string CHAT_CONFIGURATION_SECTION { get; } = "Chat";
        public static string CHAT_RESILIENCE_PIPELINE { get; } = "ChatResiliencePipeline";
        public static string DEFAULT_RESILIENCE_PIPELINE { get; } = "Default";
        public static string LIBRARY_API_URL { get; } = "LibraryApiUrl";
    }
}