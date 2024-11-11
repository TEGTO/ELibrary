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

        public override string ToString()
        {
            return $"PageNumber: {PageNumber}, PageSize: {PageSize}, ContainsName: '{ContainsName}', " +
                   $"PublicationFrom: {PublicationFrom?.ToString("yyyy-MM-dd") ?? "null"}, " +
                   $"PublicationTo: {PublicationTo?.ToString("yyyy-MM-dd") ?? "null"}, " +
                   $"MinPrice: {MinPrice?.ToString("F2") ?? "null"}, MaxPrice: {MaxPrice?.ToString("F2") ?? "null"}, " +
                   $"CoverType: {CoverType?.ToString() ?? "null"}, OnlyInStock: {OnlyInStock?.ToString() ?? "null"}, " +
                   $"MinPageAmount: {MinPageAmount?.ToString() ?? "null"}, MaxPageAmount: {MaxPageAmount?.ToString() ?? "null"}, " +
                   $"AuthorId: {AuthorId?.ToString() ?? "null"}, GenreId: {GenreId?.ToString() ?? "null"}, " +
                   $"PublisherId: {PublisherId?.ToString() ?? "null"}, Sorting: {Sorting?.ToString() ?? "null"}";
        }
    }
}