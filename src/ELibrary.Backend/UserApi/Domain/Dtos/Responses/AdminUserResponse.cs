namespace UserApi.Domain.Dtos.Responses
{
    public class AdminUserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegistredAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public List<string> Roles { get; set; }
    }
}
