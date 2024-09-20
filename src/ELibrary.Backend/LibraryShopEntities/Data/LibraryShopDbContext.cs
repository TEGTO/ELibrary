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
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartBook> CartBooks { get; set; }
        public virtual DbSet<OrderBook> OrderBooks { get; set; }

        public LibraryShopDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                  .HasMany(o => o.OrderBooks)
                  .WithOne(ob => ob.Order)
                  .HasForeignKey(ob => ob.OrderId)
                  .IsRequired();

            modelBuilder.Entity<Cart>()
              .HasMany(e => e.Books)
              .WithOne(ob => ob.Cart)
              .HasForeignKey(ob => ob.CartId)
              .IsRequired();
        }
    }
}