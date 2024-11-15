using FluentValidation;
using ShopApi.Features.ClientFeature.Dtos;
using System.Text.RegularExpressions;

namespace ShopApi.Features.ClientFeature.Validators
{
    public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
    {
        public CreateClientRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.MiddleName).NotNull().MaximumLength(256);
            RuleFor(x => x.LastName).NotNull().MaximumLength(256);
            RuleFor(x => x.DateOfBirth).LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.Address).NotNull().MaximumLength(256);
            RuleFor(x => x.Phone).NotNull().MaximumLength(256);
            RuleFor(p => p.Phone).NotNull().NotEmpty().MinimumLength(10).MaximumLength(50)
              .Matches(new Regex(@"^\d*$")).WithMessage("Phone number is not valid!");
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().MaximumLength(256);
        }
    }
}
