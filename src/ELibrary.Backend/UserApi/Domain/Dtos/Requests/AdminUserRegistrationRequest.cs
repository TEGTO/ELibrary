namespace UserApi.Domain.Dtos.Requests
{
    public class AdminUserRegistrationRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
