﻿using FluentValidation;
using LibraryApi.Domain.Dtos;

namespace LibraryApi.Validators
{
    public class BookPaginationRequestValidator : AbstractValidator<BookFilterRequest>
    {
        public BookPaginationRequestValidator()
        {
            RuleFor(x => x.ContainsName).NotNull().MaximumLength(256);
            RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.PublicationFromUTC).LessThanOrEqualTo(x => x.PublicationToUTC).When(x => x.PublicationFromUTC != null && x.PublicationToUTC != null);
            RuleFor(x => x.PublicationToUTC).GreaterThanOrEqualTo(x => x.PublicationFromUTC).When(x => x.PublicationFromUTC != null && x.PublicationToUTC != null);
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