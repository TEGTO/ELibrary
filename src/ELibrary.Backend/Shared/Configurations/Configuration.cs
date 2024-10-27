namespace Shared.Configurations
{
    internal static class Configuration
    {
        public static string ALLOWED_CORS_ORIGINS { get; } = "AllowedCORSOrigins";
        public static string MAX_PAGINATION_PAGE_SIZE { get; } = "MaxPaginationPageSize";
        public static string REPOSITORY_RESILIENCE_PIPELINE { get; } = "RepositoryResiliencePipeline";
        public static string HTTP_CLIENT_RESILIENCE_PIPELINE { get; } = "HttpClioentResiliencePipeline";
        public static string DEFAULT_RESILIENCE_PIPELINE_SECTION { get; } = "ResiliencePipeline";
    }
}