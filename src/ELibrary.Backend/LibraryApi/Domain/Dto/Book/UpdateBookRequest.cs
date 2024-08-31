namespace LibraryApi.Domain.Dto.Book
{
    public class UpdateBookRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
    }
}
