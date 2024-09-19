using LibraryShopEntities.Domain.Entities.Library;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Shop
{
    public class OrderBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int BookAmount { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int OrderId { get; set; }
        public Book Book { get; set; } = default!;
        public Order Order { get; set; } = default!;

        public void Copy(OrderBook other)
        {
            this.BookAmount = other.BookAmount;
            this.BookId = other.BookId;
            this.Book = other.Book;
        }
    }
}
