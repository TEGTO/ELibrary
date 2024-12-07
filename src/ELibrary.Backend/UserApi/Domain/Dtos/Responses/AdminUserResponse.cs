using UserEntities.Domain.Entities;

namespace UserApi.Domain.Dtos.Responses
{
    public class AdminUserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistredAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public List<string> Roles { get; set; } = [];
        public List<AuthenticationMethod> AuthenticationMethods { get; set; } = [];

    }
}
