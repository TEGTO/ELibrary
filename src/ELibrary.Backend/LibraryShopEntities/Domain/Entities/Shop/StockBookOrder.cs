using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Shop
{
    public enum StockBookOrderType
    {
        StockReplenishment, ClientOrder, ClientOrderCancel
    }
    public class StockBookOrder : ITrackable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        [Required]
        public int TotalChangeAmount { get; set; }
        [Required]
        public StockBookOrderType Type { get; set; }
        [Required]
        public string ClientId { get; set; } = default!;
        [ForeignKey("ClientId")]
        public Client Client { get; set; } = default!;
        public List<StockBookChange> StockBookChanges { get; set; } = new List<StockBookChange>();
    }
}
