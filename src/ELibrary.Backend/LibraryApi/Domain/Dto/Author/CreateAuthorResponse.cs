namespace LibraryApi.Domain.Dto.Author
{
    public class CreateAuthorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}