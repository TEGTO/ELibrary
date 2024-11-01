using LibraryShopEntities.Domain.Entities;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;

namespace LibraryShopEntities.Data
{
    public class ShopDbContext : DbContext
    {
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartBook> CartBooks { get; set; }
        public virtual DbSet<OrderBook> OrderBooks { get; set; }
        public virtual DbSet<StockBookChange> StockBookChanges { get; set; }
        public virtual DbSet<StockBookOrder> StockBookOrders { get; set; }

        public ShopDbContext(DbContextOptions options) : base(options)
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

            modelBuilder.Entity<StockBookOrder>()
                .HasOne(sbo => sbo.Client)
                .WithMany()
                .HasForeignKey(sbo => sbo.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<ITrackable>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}