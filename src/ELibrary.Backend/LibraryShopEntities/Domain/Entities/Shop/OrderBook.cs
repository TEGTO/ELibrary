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
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Book amount must be greater than 0")]
        public int BookAmount { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; } = default!;
        [ForeignKey("OrderId")]
        public Order Order { get; set; } = default!;

        public void Copy(OrderBook other)
        {
            this.BookAmount = other.BookAmount;
            this.BookId = other.BookId;
            this.Book = other.Book;
        }
    }
}
