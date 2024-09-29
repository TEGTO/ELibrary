﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShopEntities.Domain.Entities.Library
{
    public enum CoverType
    {
        Any = 0, Hard, Soft
    }
    public class Book : BaseLibraryEntity
    {
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Required]
        public CoverType CoverType { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Page amount must be greater than 0")]
        public int PageAmount { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock amount must be greater than or equal to 0")]
        public int StockAmount { get; set; }
        [Required]
        [MaxLength(1024)]
        public string CoverImgUrl { get; set; } = default!;
        [Required]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; } = default!;
        [Required]
        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genre Genre { get; set; } = default!;
        [Required]
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
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
                this.CoverImgUrl = otherBook.CoverImgUrl;
                this.AuthorId = otherBook.AuthorId;
                this.GenreId = otherBook.GenreId;
                this.PublisherId = otherBook.PublisherId;
            }
        }
    }
}