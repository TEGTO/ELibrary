using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Shop
{
    [Index(nameof(UserId), IsUnique = true)]
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = default!;
        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = default!;
        [MaxLength(256)]
        public string MiddleName { get; set; } = string.Empty;
        [MaxLength(256)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [MaxLength(512)]
        public string Address { get; set; } = string.Empty;
        [Required]
        [MinLength(10), MaxLength(50), Phone]
        public string Phone { get; set; } = string.Empty;
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = default!;
        [Required]
        [MaxLength(256)]
        public string UserId { get; set; } = default!;
        public List<Order> Orders { get; set; } = new List<Order>();

        public void Copy(Client other)
        {
            this.Name = other.Name;
            this.MiddleName = other.MiddleName;
            this.LastName = other.LastName;
            this.DateOfBirth = other.DateOfBirth;
            this.Address = other.Address;
            this.Phone = other.Phone;
            this.Email = other.Email;
        }
    }
}
