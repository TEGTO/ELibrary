namespace LibraryApi
{
    public static class Configuration
    {
        public static string LIBRARY_DATABASE_CONNECTION_STRING { get; } = "LibraryShopDb";
        public static string EF_CREATE_DATABASE { get; } = "EFCreateDatabase";
        public static string USE_CORS { get; } = "UseCORS";
    }
}
