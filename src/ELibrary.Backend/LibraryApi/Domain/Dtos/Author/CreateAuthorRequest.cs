namespace LibraryApi.Domain.Dto.Author
{
    public class CreateAuthorRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}