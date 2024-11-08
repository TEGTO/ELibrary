using FluentValidation;
using Pagination;
using UserApi.Domain.Dtos;

namespace UserApi.Validators
{
    public class AdminGetUserFilterValidator : AbstractValidator<AdminGetUserFilter>
    {
        public AdminGetUserFilterValidator(PaginationOptions paginationConfiguration)
        {
            Include(new PaginationRequestValidator(paginationConfiguration));
            RuleFor(x => x.ContainsInfo).NotNull().MaximumLength(256);
        }
    }
}
