using FluentValidation;
using LibraryApi.Domain.Dtos.Library.Publisher;

namespace LibraryApi.Validators.Publisher
{
    public class CreatePublisherRequestValidator : AbstractValidator<CreatePublisherRequest>
    {
        public CreatePublisherRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}
