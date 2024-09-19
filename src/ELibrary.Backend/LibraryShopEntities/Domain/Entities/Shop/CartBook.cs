using LibraryShopEntities.Domain.Entities.Library;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Shop
{
    public class CartBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int BookAmount { get; set; }
        [Required]
        public int BookId { get; set; } = default!;
        [Required]
        public string CartId { get; set; } = default!;
        public Book Book { get; set; } = default!;
        public Cart Cart { get; set; } = default!;

        public void Copy(CartBook other)
        {
            this.BookAmount = other.BookAmount;
            this.Book = other.Book;
        }
    }
}
