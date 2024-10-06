using FluentValidation;
using Shared.Validators;
using UserApi.Domain.Dtos.Requests;

namespace UserApi.Validators
{
    public class GetUserFilterRequestValidator : AbstractValidator<GetUserFilterRequest>
    {
        public GetUserFilterRequestValidator(PaginationConfiguration paginationConfiguration)
        {
            Include(new PaginationRequestValidator(paginationConfiguration));
            RuleFor(x => x.ContainsString).NotNull().MaximumLength(256);
        }
    }
}
