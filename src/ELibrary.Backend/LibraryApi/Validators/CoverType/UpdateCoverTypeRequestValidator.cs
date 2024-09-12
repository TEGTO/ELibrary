using FluentValidation;
using LibraryShopEntities.Domain.Dtos.Library.CoverType;

namespace LibraryShopEntities.Validators.CoverType
{
    public class UpdateCoverTypeRequestValidator : AbstractValidator<UpdateCoverTypeRequest>
    {
        public UpdateCoverTypeRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}