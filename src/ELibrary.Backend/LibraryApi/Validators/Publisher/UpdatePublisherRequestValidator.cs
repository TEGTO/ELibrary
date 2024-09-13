using FluentValidation;
using LibraryApi.Domain.Dtos.Library.Publisher;

namespace LibraryApi.Validators.Publisher
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
