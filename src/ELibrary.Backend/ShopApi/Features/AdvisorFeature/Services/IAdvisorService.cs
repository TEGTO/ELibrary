using ShopApi.Features.AdvisorFeature.Domain.Dtos;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public interface IAdvisorService
    {
        public Task<AdvisorResponse?> SendQueryAsync(AdvisorQueryRequest req, CancellationToken cancellationToken);
    }
}