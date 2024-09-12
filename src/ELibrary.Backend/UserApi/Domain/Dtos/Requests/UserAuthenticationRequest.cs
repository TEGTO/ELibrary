namespace UserApi.Domain.Dtos.Requests
{
    public class UserAuthenticationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
