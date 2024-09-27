using FluentValidation;
using ShopApi.Features.ClientFeature.Dtos;

namespace ShopApi.Features.ClientFeature.Validators
{
    public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
    {
        public UpdateClientRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.MiddleName).NotNull().MaximumLength(256);
            RuleFor(x => x.LastName).NotNull().MaximumLength(256);
            RuleFor(x => x.DateOfBirth).LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.Address).NotNull().MaximumLength(256);
            RuleFor(x => x.Phone).NotNull().MaximumLength(256);
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().MaximumLength(256);
        }
    }
}
