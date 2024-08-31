using FluentValidation;
using LibraryApi.Domain.Dto.Book;

namespace LibraryApi.Validators.Book
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.AuthorId).GreaterThan(0);
            RuleFor(x => x.GenreId).GreaterThan(0);
        }
    }
}
