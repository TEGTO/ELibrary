namespace UserApi.Domain.Models
{
    public class UserUpdateData
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? Password { get; set; }
    }
}
