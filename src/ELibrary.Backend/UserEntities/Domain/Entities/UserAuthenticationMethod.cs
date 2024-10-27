using Microsoft.EntityFrameworkCore;

namespace UserEntities.Domain.Entities
{
    public enum AuthenticationMethod
    {
        BaseAuthentication = 0, GoogleOAuth
    }
    [Index(nameof(UserId), nameof(AuthenticationMethod), IsUnique = true)]
    public class UserAuthenticationMethod
    {
        public string UserId { get; set; }
        public User User { get; set; } = default!;
        public AuthenticationMethod AuthenticationMethod { get; set; }
    }
}
