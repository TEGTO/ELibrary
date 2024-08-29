using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Domain.Entities
{
    public class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = default!;
        [MaxLength(256)]
        public string Name { get; set; } = default!;
        [MaxLength(256)]
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        [MaxLength(256)]
        public string Address { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}