using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserApi.Domain.Entities;

namespace UserApi.Data
{
    public class UserIdentityDbContext : IdentityDbContext<User>
    {
        public DbSet<UserInfo> UserInfos { get; set; }

        public UserIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
