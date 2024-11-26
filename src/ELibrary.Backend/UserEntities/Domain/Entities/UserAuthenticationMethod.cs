using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UserEntities.Domain.Entities
{
    public enum AuthenticationMethod
    {
        BaseAuthentication = 0, GoogleOAuth
    }
    [Index(nameof(UserId), nameof(AuthenticationMethod), IsUnique = true)]
    public class UserAuthenticationMethod
    {
        [Required]
        public string UserId { get; set; } = default!;
        public User User { get; set; } = default!;
        public AuthenticationMethod AuthenticationMethod { get; set; }
    }
}
