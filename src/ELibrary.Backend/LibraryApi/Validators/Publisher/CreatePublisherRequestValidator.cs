using FluentValidation;
using LibraryShopEntities.Domain.Dtos.Library.Publisher;

namespace LibraryShopEntities.Validators.Publisher
{
    public class CreatePublisherRequestValidator : AbstractValidator<CreatePublisherRequest>
    {
        public CreatePublisherRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}
