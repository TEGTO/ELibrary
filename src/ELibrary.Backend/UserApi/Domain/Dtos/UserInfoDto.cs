namespace UserApi.Domain.Dtos
{
    public class UserInfoDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}