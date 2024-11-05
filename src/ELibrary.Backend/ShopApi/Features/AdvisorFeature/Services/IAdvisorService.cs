using ShopApi.Features.AdvisorFeature.Domain.Dtos;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public interface IAdvisorService
    {
        public Task<ChatAdvisorResponse?> SendQueryAsync(ChatAdvisorQueryRequest req, CancellationToken cancellationToken);
    }
}