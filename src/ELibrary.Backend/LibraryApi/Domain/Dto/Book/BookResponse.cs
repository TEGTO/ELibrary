using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dto.Genre;

namespace LibraryApi.Domain.Dto.Book
{
    public class BookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public AuthorResponse Author { get; set; }
        public GenreResponse Genre { get; set; }
    }
}