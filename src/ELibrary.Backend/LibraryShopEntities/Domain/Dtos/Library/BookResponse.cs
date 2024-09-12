using LibraryShopEntities.Domain.Dtos.Library.Publisher;

namespace LibraryShopEntities.Domain.Dtos.Library
{
    public class BookResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublicationDate { get; set; }
        public decimal Price { get; set; }
        public int PageAmount { get; set; }
        public int StockAmount { get; set; }
        public AuthorResponse Author { get; set; }
        public GenreResponse Genre { get; set; }
        public PublisherResponse Publisher { get; set; }
        public CoverTypeResponse CoverType { get; set; }
    }
}