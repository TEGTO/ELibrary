namespace LibraryShopEntities.Domain.Dtos.Shop
{
    public class ClientResponse
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}