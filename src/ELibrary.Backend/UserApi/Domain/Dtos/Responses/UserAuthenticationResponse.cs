using UserEntities.Domain.Entities;

namespace UserApi.Domain.Dtos.Responses
{
    public class UserAuthenticationResponse
    {
        public AuthToken AuthToken { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public LoginProvider LoginProvider { get; set; }
    }
}
