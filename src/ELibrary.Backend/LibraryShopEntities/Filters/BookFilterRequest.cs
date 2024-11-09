using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryShopEntities.Filters
{
    public enum BookSorting
    {
        MostPopular, LeastPopular
    }

    public class BookFilterRequest : LibraryFilterRequest
    {
        public DateTime? PublicationFrom { get; set; }
        public DateTime? PublicationTo { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public CoverType? CoverType { get; set; }
        public bool? OnlyInStock { get; set; }
        public int? MinPageAmount { get; set; }
        public int? MaxPageAmount { get; set; }
        public int? AuthorId { get; set; }
        public int? GenreId { get; set; }
        public int? PublisherId { get; set; }
        public BookSorting? Sorting { get; set; }
    }
}