using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.Domain.Dto.Book
{
    public class UpdateBookRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublicationDate { get; set; }
        public decimal Price { get; set; }
        public CoverType CoverType { get; set; }
        public string CoverImgUrl { get; set; }
        public int PageAmount { get; set; }
        public int StockAmount { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public int PublisherId { get; set; }
    }
}
