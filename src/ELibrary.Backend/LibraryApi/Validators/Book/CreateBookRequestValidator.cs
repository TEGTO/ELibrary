using FluentValidation;
using LibraryShopEntities.Domain.Dtos.Library.Book;

namespace LibraryShopEntities.Validators.Book
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.PublicationDate).LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageAmount).GreaterThan(0);
            RuleFor(x => x.StockAmount).GreaterThan(0);
            RuleFor(x => x.AuthorId).GreaterThan(0);
            RuleFor(x => x.GenreId).GreaterThan(0);
            RuleFor(x => x.PublisherId).GreaterThan(0);
            RuleFor(x => x.CoverTypeId).GreaterThan(0);
        }
    }
}