using UserEntities.Domain.Entities;

namespace UserApi.Domain.Models
{
    public class CreateUserFromOAuth
    {
        public required string Email { get; set; } = string.Empty;
        public required string LoginProviderSubject { get; set; } = string.Empty;
        public required AuthenticationMethod AuthMethod { get; set; }
    }
}
