namespace UserApi.Domain.Dtos.Requests
{
    public class UserUpdateDataRequest
    {
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? Password { get; set; }
    }
}
