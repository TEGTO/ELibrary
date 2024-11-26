using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Shop
{
    public class CartBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = default!;
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Book amount must be greater or equal than 0")]
        public int BookAmount { get; set; }
        [Required]
        public int BookId { get; set; } = default!;
        [Required]
        public string CartId { get; set; } = default!;
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = default!;

        public void Copy(CartBook other)
        {
            this.BookAmount = other.BookAmount;
        }
    }
}
