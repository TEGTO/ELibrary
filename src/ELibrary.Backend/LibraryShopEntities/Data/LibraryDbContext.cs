using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.EntityFrameworkCore;

namespace LibraryShopEntities.Data
{
    public class LibraryDbContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<BookPopularity> BookPopularities { get; set; }

        public LibraryDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.BookPopularity)
                .WithOne(bp => bp.Book)
                .HasForeignKey<BookPopularity>(bp => bp.BookId);
        }
    }
}