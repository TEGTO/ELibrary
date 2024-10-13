namespace ShopApi
{
    public static class Configuration
    {
        public static string SHOP_DATABASE_CONNECTION_STRING { get; } = "LibraryShopDb";
        public static string SHOP_MAX_ORDER_AMOUNT { get; } = "Shop:MaxOrderAmount";
        public static string USE_CORS { get; } = "UseCORS";
    }
}