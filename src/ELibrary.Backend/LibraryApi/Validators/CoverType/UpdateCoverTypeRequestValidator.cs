using FluentValidation;
using LibraryApi.Domain.Dtos.Library.CoverType;

namespace LibraryApi.Validators.CoverType
{
    public class UpdateCoverTypeRequestValidator : AbstractValidator<UpdateCoverTypeRequest>
    {
        public UpdateCoverTypeRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}