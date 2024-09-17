using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.Domain.Dtos
{
    public class BookFilterRequest : LibraryFilterRequest
    {
        public DateTime? PublicationFromUTC { get; set; }
        public DateTime? PublicationToUTC { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public CoverType? CoverType { get; set; }
        public bool? OnlyInStock { get; set; }
        public int? MinPageAmount { get; set; }
        public int? MaxPageAmount { get; set; }
        public int? AuthorId { get; set; }
        public int? GenreId { get; set; }
        public int? PublisherId { get; set; }

        public BookFilterRequest()
        {
            ApplyDefaults();
        }
        public void ApplyDefaults()
        {
            PublicationFromUTC = PublicationFromUTC ?? DateTime.MinValue;
            PublicationToUTC = PublicationToUTC ?? DateTime.MaxValue;
            MinPrice = MinPrice ?? decimal.MinValue;
            MaxPrice = MaxPrice ?? decimal.MaxValue;
            CoverType = CoverType ?? LibraryShopEntities.Domain.Entities.Library.CoverType.Any;
            OnlyInStock = OnlyInStock ?? true;
            MinPageAmount = MinPageAmount ?? int.MinValue;
            MaxPageAmount = MaxPageAmount ?? int.MaxValue;
        }
    }
}