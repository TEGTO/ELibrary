using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserEntities.Domain.Entities;

namespace UserEntities.Data
{
    public class UserIdentityDbContext : IdentityDbContext<User>
    {
        public virtual DbSet<UserAuthenticationMethod> UserAuthenticationMethods { get; set; }

        public UserIdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("asp_net_users");
            builder.Entity<IdentityUserToken<string>>().ToTable("asp_net_user_tokens");
            builder.Entity<IdentityUserLogin<string>>().ToTable("asp_net_user_logins");
            builder.Entity<IdentityUserClaim<string>>().ToTable("asp_net_user_claims");
            builder.Entity<IdentityRole>().ToTable("asp_net_roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("asp_net_user_roles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("asp_net_role_claims");

            builder.Entity<UserAuthenticationMethod>()
            .HasKey(um => new { um.UserId, um.AuthenticationMethod });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<User>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.RegistredAtUtc = DateTime.UtcNow;
                    entry.Entity.UpdatedAtUtc = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAtUtc = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
