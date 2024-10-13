using LibraryShopEntities.Domain.Entities.Library;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Shop
{
    public class StockBookChange
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; } = default!;
        [ForeignKey("BookId")]
        public Book Book { get; set; } = default!;
        [Required]
        public int ChangeAmount { get; set; }
    }
}