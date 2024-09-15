using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Library
{
    public enum CoverType
    {
        Any = 0, Hard, Soft
    }
    public class Book : BaseLibraryEntity
    {
        public DateTime PublicationDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public CoverType CoverType { get; set; }
        public int PageAmount { get; set; }
        public int StockAmount { get; set; }
        [Required]
        public int AuthorId { get; set; } = default!;
        [Required]
        public int GenreId { get; set; } = default!;
        [Required]
        public int PublisherId { get; set; } = default!;
        public Author Author { get; set; } = default!;
        public Genre Genre { get; set; } = default!;
        public Publisher Publisher { get; set; } = default!;

        public override void Copy(BaseLibraryEntity other)
        {
            if (other is Book otherBook)
            {
                this.Name = otherBook.Name;
                this.PublicationDate = otherBook.PublicationDate;
                this.Price = otherBook.Price;
                this.CoverType = otherBook.CoverType;
                this.PageAmount = otherBook.PageAmount;
                this.StockAmount = otherBook.StockAmount;
                this.AuthorId = otherBook.AuthorId;
                this.GenreId = otherBook.GenreId;
                this.PublisherId = otherBook.PublisherId;
            }
        }
    }
}
