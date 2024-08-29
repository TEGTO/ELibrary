using FluentValidation;
using LibraryApi.Domain.Dto.Genre;

namespace LibraryApi.Validators.Genre
{
    public class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequest>
    {
        public CreateGenreRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}