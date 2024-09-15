using FluentValidation;
using LibraryApi.Domain.Dto.Publisher;

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
