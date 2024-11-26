using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryShopEntities.Domain.Dtos.Library
{
    public class BookResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime PublicationDate { get; set; }
        public decimal Price { get; set; }
        public CoverType CoverType { get; set; }
        public string? CoverImgUrl { get; set; }
        public int PageAmount { get; set; }
        public int StockAmount { get; set; }
        public string? Description { get; set; }
        public AuthorResponse? Author { get; set; }
        public GenreResponse? Genre { get; set; }
        public PublisherResponse? Publisher { get; set; }
    }
}