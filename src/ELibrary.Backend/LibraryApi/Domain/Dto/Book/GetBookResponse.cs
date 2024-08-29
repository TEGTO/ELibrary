namespace LibraryApi.Domain.Dto.Book
{
    public class GetBookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string AuthorId { get; set; }
        public string GenreId { get; set; }
    }
}