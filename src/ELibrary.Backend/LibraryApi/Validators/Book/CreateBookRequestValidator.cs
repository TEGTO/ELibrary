﻿using FluentValidation;
using LibraryApi.Domain.Dto.Book;

namespace LibraryApi.Validators.Book
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.AuthorId).GreaterThan(0);
            RuleFor(x => x.GenreId).GreaterThan(0);
        }
    }
}