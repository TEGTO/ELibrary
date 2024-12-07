namespace UserApi.Domain.Dtos.Requests
{
    public class AdminUserUpdateDataRequest
    {
        public string CurrentLogin { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
