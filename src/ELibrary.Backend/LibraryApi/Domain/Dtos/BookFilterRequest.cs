using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.Domain.Dtos
{
    public class BookFilterRequest : LibraryFilterRequest
    {
        public DateTime PublicationFromUTC { get; set; } = DateTime.MinValue;
        public DateTime PublicationToUTC { get; set; } = DateTime.MaxValue;
        public decimal MinPrice { get; set; } = decimal.MinValue;
        public decimal MaxPrice { get; set; } = decimal.MaxValue;
        public CoverType CoverType { get; set; } = CoverType.Any;
        public bool OnlyInStock { get; set; } = true;
        public int MinPageAmount { get; set; } = int.MinValue;
        public int MaxPageAmount { get; set; } = int.MaxValue;
        public int? AuthorId { get; set; } = null;
        public int? GenreId { get; set; } = null;
        public int? PublisherId { get; set; } = null;
    }
}