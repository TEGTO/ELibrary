namespace LibraryApi.Domain.Dto.Book
{
    public class CreateBookRequest
    {
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string AuthorId { get; set; }
        public string GenreId { get; set; }
    }
}