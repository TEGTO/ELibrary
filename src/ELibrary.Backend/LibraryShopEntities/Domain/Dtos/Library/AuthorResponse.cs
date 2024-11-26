namespace LibraryShopEntities.Domain.Dtos.Library
{
    public class AuthorResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
