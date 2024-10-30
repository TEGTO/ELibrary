using FluentValidation;
using Shared.Configurations;
using Shared.Validators;
using UserApi.Domain.Dtos;

namespace UserApi.Validators
{
    public class AdminGetUserFilterValidator : AbstractValidator<AdminGetUserFilter>
    {
        public AdminGetUserFilterValidator(PaginationConfiguration paginationConfiguration)
        {
            Include(new PaginationRequestValidator(paginationConfiguration));
            RuleFor(x => x.ContainsInfo).NotNull().MaximumLength(256);
        }
    }
}
