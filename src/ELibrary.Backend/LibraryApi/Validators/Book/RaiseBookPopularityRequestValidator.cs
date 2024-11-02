using FluentValidation;
using LibraryShopEntities.Domain.Dtos.SharedRequests;

namespace LibraryApi.Validators.Book
{
    public class RaiseBookPopularityRequestValidator : AbstractValidator<RaiseBookPopularityRequest>
    {
        public RaiseBookPopularityRequestValidator()
        {
            RuleFor(x => x.Ids).NotNull();
        }
    }
}
