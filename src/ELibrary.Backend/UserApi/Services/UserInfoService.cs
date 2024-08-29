using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using UserApi.Data;
using UserApi.Domain.Entities;

namespace UserApi.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IDatabaseRepository<UserIdentityDbContext> repository;

        public UserInfoService(IDatabaseRepository<UserIdentityDbContext> repository)
        {
            this.repository = repository;
        }

        #region IUserInfoService Members

        public async Task<UserInfo?> GetUserInfoAsync(string userId, CancellationToken cancellationToken)
        {
            UserInfo? userInfo = null;
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                userInfo = await dbContext.UserInfos.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            }
            return userInfo;
        }

        #endregion

        private async Task<UserIdentityDbContext> CreateDbContextAsync(CancellationToken cancellationToken)
        {
            return await repository.CreateDbContextAsync(cancellationToken);
        }
    }
}