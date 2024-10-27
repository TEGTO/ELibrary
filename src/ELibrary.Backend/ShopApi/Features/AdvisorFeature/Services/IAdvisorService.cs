namespace ShopApi.Features.AdvisorFeature.Services
{
    public interface IAdvisorService
    {
        public Task<string> SendQueryAsync(string query, CancellationToken cancellationToken);
    }
}