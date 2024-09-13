using FluentValidation;
using LibraryApi.Domain.Dtos.Library.Author;

namespace LibraryApi.Validators.Author
{
    public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
    {
        public CreateAuthorRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.LastName).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.DateOfBirth).LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}