using System.ComponentModel.DataAnnotations;

namespace UserApi.Domain.Entities
{
    public class UserInfo
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [MaxLength(256)]
        public string? Adress { get; set; }
    }
}
