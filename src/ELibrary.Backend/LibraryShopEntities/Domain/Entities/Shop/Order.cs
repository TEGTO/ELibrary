using LibraryShopEntities.Domain.Entities.Library;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Shop
{
    public enum OrderStatus
    {
        InProcessing, Packed, Delivered
    }

    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        [NotMapped]
        public int OrderAmount { get => Books.Count; }
        [NotMapped]
        public decimal TotalPrice { get => Books.Sum(x => x.Price); }
        [Required]
        [MaxLength(512)]
        public string DeliveryAddress { get; set; } = default!;
        public DateTime DeliveryTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
        [Required]
        public string ClientId { get; set; } = default!;
        public Client Client { get; set; } = default!;

        public void Copy(Order other)
        {
            this.DeliveryAddress = other.DeliveryAddress;
            this.DeliveryTime = other.DeliveryTime;
            this.OrderStatus = other.OrderStatus;
        }
    }
}
