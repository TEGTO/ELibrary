using FluentValidation;
using LibraryApi.Domain.Dto.Book;

namespace LibraryApi.Validators.Book
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.AuthorId).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.GenreId).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}
