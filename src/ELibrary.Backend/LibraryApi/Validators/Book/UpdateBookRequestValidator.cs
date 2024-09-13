using FluentValidation;
using LibraryApi.Domain.Dtos.Library.Book;

namespace LibraryApi.Validators.Book
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.PublicationDate).NotNull().LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.Price).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageAmount).NotNull().GreaterThan(0);
            RuleFor(x => x.StockAmount).NotNull().GreaterThan(0);
            RuleFor(x => x.AuthorId).NotNull().GreaterThan(0);
            RuleFor(x => x.GenreId).NotNull().GreaterThan(0);
            RuleFor(x => x.PublisherId).NotNull().GreaterThan(0);
            RuleFor(x => x.CoverTypeId).NotNull().GreaterThan(0);
        }
    }
}
