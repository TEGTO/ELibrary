using FluentValidation;
using LibraryShopEntities.Domain.Dtos.Library.Genre;

namespace LibraryShopEntities.Validators.Genre
{
    public class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequest>
    {
        public CreateGenreRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}