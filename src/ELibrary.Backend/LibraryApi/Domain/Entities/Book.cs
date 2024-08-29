using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Domain.Entities
{
    public class Book : BaseEntity
    {
        [Required]
        [MaxLength(256)]
        public string Title { get; set; } = default!;
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; } = default!;
        public int GenreId { get; set; } = default!;
        public Author Author { get; set; } = default!;
        public Genre Genre { get; set; } = default!;

        public override void Copy(BaseEntity other)
        {
            if (other is Book otherBook)
            {
                this.Title = otherBook.Title;
                this.PublicationDate = otherBook.PublicationDate;
                this.AuthorId = otherBook.AuthorId;
                this.GenreId = otherBook.GenreId;
            }
        }
    }
}
