using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Shop
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = default!;
        [Required]
        [MaxLength(256)]
        public string UserId { get; set; } = default!;
        public List<CartBook> CartBooks { get; set; } = new List<CartBook>();
    }
}
