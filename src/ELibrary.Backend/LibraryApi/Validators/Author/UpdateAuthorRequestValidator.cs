using FluentValidation;
using LibraryShopEntities.Domain.Dtos.Library.Author;

namespace LibraryShopEntities.Validators.Author
{
    public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}