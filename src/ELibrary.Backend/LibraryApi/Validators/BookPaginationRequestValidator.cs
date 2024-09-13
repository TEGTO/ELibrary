using FluentValidation;
using LibraryApi.Domain.Dtos;

namespace LibraryApi.Validators
{
    public class BookPaginationRequestValidator : AbstractValidator<BookPaginationRequest>
    {
        public BookPaginationRequestValidator()
        {
            RuleFor(x => x.ContainsName).NotNull().MaximumLength(256);
            RuleFor(x => x.PageNumber).NotNull().GreaterThan(0);
            RuleFor(x => x.PageSize).NotNull().GreaterThan(0);
            RuleFor(x => x.PublicationFromUTC).LessThanOrEqualTo(x => x.PublicationToUTC);
            RuleFor(x => x.PublicationToUTC).GreaterThanOrEqualTo(x => x.PublicationFromUTC);
            RuleFor(x => x.MinPrice).LessThanOrEqualTo(x => x.MaxPrice);
            RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(x => x.MinPrice);
            RuleFor(x => x.OnlyInStock).NotNull();
            RuleFor(x => x.MinPageAmount).LessThanOrEqualTo(x => x.MaxPageAmount);
            RuleFor(x => x.MaxPageAmount).GreaterThanOrEqualTo(x => x.MinPageAmount);
            RuleFor(x => x.AuthorId).GreaterThan(0).When(x => x.AuthorId != null);
            RuleFor(x => x.GenreId).GreaterThan(0).When(x => x.GenreId != null);
            RuleFor(x => x.PublisherId).GreaterThan(0).When(x => x.PublisherId != null);
            RuleFor(x => x.CoverTypeId).GreaterThan(0).When(x => x.CoverTypeId != null);
        }
    }
}
