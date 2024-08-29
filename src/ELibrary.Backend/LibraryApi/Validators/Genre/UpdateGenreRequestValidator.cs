using FluentValidation;
using LibraryApi.Domain.Dto.Genre;

namespace LibraryApi.Validators.Genre
{
    public class UpdateGenreRequestValidator : AbstractValidator<UpdateGenreRequest>
    {
        public UpdateGenreRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}