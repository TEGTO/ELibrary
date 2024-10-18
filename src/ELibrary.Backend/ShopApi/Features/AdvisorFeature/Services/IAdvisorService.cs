namespace ShopApi.Features.AdvisorFeature.Services
{
    public interface IAdvisorService
    {
        public Task<string> AskQueryAsync(string query, CancellationToken cancellationToken);
    }
}