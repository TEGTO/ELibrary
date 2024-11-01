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
        public decimal BookPrice { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; } = default!;
    }
}
