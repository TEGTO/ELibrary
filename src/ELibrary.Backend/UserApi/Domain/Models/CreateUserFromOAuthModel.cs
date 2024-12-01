using UserEntities.Domain.Entities;

namespace UserApi.Domain.Models
{
    public class CreateUserFromOAuthModel
    {
        public required string Email { get; set; }
        public required string LoginProviderSubject { get; set; }
        public required AuthenticationMethod AuthMethod { get; set; }
    }
}
