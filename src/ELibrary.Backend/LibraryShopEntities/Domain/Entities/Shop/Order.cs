using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Shop
{
    public enum OrderStatus
    {
        Canceled = -1, InProcessing, Processed, Delivering, Completed
    }
    public enum PaymentMethod
    {
        Cash
    }

    public class Order : ITrackable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Order amount must be at least 1.")]
        public int OrderAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }
        [Required]
        [MaxLength(512)]
        public string DeliveryAddress { get; set; } = default!;
        [Required]
        public DateTime DeliveryTime { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        public List<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();
        [Required]
        public string ClientId { get; set; } = default!;
        [ForeignKey("ClientId")]
        public Client Client { get; set; } = default!;

        public void Copy(Order other)
        {
            this.DeliveryAddress = other.DeliveryAddress;
            this.DeliveryTime = other.DeliveryTime;
            this.OrderStatus = other.OrderStatus;
        }
    }
}