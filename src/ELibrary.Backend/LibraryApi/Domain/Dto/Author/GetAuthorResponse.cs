namespace LibraryApi.Domain.Dto.Author
{
    public class GetAuthorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}