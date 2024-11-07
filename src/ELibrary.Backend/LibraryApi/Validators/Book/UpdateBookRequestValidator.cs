using FluentValidation;
using LibraryApi.Domain.Dto.Book;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.Validators.Book
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.PublicationDate).NotNull().LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.Price).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageAmount).NotNull().GreaterThan(0);
            RuleFor(x => x.CoverImgUrl).NotNull().NotEmpty().MaximumLength(1024);
            RuleFor(x => x.Description).MaximumLength(4096).When(x => x.Description != null);
            RuleFor(x => x.CoverType).NotNull().NotEqual(CoverType.Any);
            RuleFor(x => x.AuthorId).NotNull().GreaterThan(0);
            RuleFor(x => x.GenreId).NotNull().GreaterThan(0);
            RuleFor(x => x.PublisherId).NotNull().GreaterThan(0);
        }
    }
}
