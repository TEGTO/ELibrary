using FluentValidation;
using LibraryShopEntities.Domain.Dtos.Library.Publisher;

namespace LibraryShopEntities.Validators.Publisher
{
    public class UpdatePublisherRequestValidator : AbstractValidator<UpdatePublisherRequest>
    {
        public UpdatePublisherRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
        }
    }
}
