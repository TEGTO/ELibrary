using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using UserApi.Domain.Entities;

namespace AuthenticationApi.Domain.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public UserInfo UserInfo { get; set; } = default!;
    }
}
