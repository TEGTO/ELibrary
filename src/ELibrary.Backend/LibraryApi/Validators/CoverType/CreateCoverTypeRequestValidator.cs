using FluentValidation;
using LibraryApi.Domain.Dtos.Library.CoverType;

namespace LibraryApi.Validators.CoverType
{
    public class CreateCoverTypeRequestValidator : AbstractValidator<CreateCoverTypeRequest>
    {
        public CreateCoverTypeRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}