namespace LibraryApi.Domain.Dtos.Library.Book
{
    public class UpdateBookRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublicationDate { get; set; }
        public decimal Price { get; set; }
        public int PageAmount { get; set; }
        public int StockAmount { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public int PublisherId { get; set; }
        public int CoverTypeId { get; set; }
    }
}
