using FluentValidation;
using LibraryShopEntities.Domain.Dtos.Library.CoverType;

namespace LibraryShopEntities.Validators.CoverType
{
    public class CreateCoverTypeRequestValidator : AbstractValidator<CreateCoverTypeRequest>
    {
        public CreateCoverTypeRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}