using FluentValidation;
using LibraryApi.Domain.Dtos;
using Shared.Configurations;
using Shared.Validators;

namespace LibraryApi.Validators
{
    public class BookFilterRequestValidator : AbstractValidator<BookFilterRequest>
    {
        public BookFilterRequestValidator(PaginationConfiguration paginationConfiguration)
        {
            Include(new PaginationRequestValidator(paginationConfiguration));
            RuleFor(x => x.ContainsName).NotNull().MaximumLength(256);
            RuleFor(x => x.PublicationFrom).LessThanOrEqualTo(x => x.PublicationTo).When(x => x.PublicationFrom != null && x.PublicationTo != null);
            RuleFor(x => x.PublicationTo).GreaterThanOrEqualTo(x => x.PublicationFrom).When(x => x.PublicationFrom != null && x.PublicationTo != null);
            RuleFor(x => x.MinPrice).LessThanOrEqualTo(x => x.MaxPrice).When(x => x.MinPrice != null && x.MaxPrice != null);
            RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0).When(x => x.MinPrice != null);
            RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(x => x.MinPrice).When(x => x.MinPrice != null && x.MaxPrice != null);
            RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(0).When(x => x.MaxPrice != null);
            RuleFor(x => x.MinPageAmount).LessThanOrEqualTo(x => x.MaxPageAmount).When(x => x.MinPageAmount != null && x.MaxPageAmount != null);
            RuleFor(x => x.MinPageAmount).GreaterThanOrEqualTo(0).When(x => x.MinPageAmount != null);
            RuleFor(x => x.MaxPageAmount).GreaterThanOrEqualTo(x => x.MinPageAmount).When(x => x.MinPageAmount != null && x.MaxPageAmount != null);
            RuleFor(x => x.MaxPageAmount).GreaterThanOrEqualTo(0).When(x => x.MaxPageAmount != null);
            RuleFor(x => x.AuthorId).GreaterThan(0).When(x => x.AuthorId != null);
            RuleFor(x => x.GenreId).GreaterThan(0).When(x => x.GenreId != null);
            RuleFor(x => x.PublisherId).GreaterThan(0).When(x => x.PublisherId != null);
        }
    }
}
