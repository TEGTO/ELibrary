using FluentValidation;
using LibraryShopEntities.Domain.Dtos.Library.Author;

namespace LibraryShopEntities.Validators.Author
{
    public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
    {
        public CreateAuthorRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}