using UserApi.Domain.Entities;

namespace UserApi.Services
{
    public interface IUserInfoService
    {
        public Task<UserInfo?> GetUserInfoAsync(string userId, CancellationToken cancellationToken);
    }
}