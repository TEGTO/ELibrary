using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;

namespace LibraryShopEntities.Data
{
    public class LibraryShopDbContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<CoverType> CoverTypes { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        public LibraryShopDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(e => e.Books)
                .WithMany();
        }
    }
}