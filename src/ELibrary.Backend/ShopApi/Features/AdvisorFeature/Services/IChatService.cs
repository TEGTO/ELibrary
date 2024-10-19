
namespace ShopApi.Features.AdvisorFeature.Services
{
    public interface IChatService
    {
        public Task<string> GetChatCompletionAsync(string prompt, string query, CancellationToken cancellationToken);
    }
}