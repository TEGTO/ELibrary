﻿using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace ShopApi.Tests
{
    [TestFixture]
    internal class AutoMapperProfileTests
    {
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            mapper = config.CreateMapper();
        }

        [Test]
        public void Map_AuthorToAuthorResponse_MapsCorrectly()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                Name = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1)
            };
            // Act
            var result = mapper.Map<AuthorResponse>(author);
            // Assert
            Assert.That(result.Id, Is.EqualTo(author.Id));
            Assert.That(result.Name, Is.EqualTo(author.Name));
            Assert.That(result.LastName, Is.EqualTo(author.LastName));
            Assert.That(result.DateOfBirth, Is.EqualTo(author.DateOfBirth));
        }
        [Test]
        public void Map_GenreToGenreResponse_MapsCorrectly()
        {
            // Arrange
            var genre = new Genre
            {
                Id = 1,
                Name = "Science Fiction"
            };
            // Act
            var result = mapper.Map<GenreResponse>(genre);
            // Assert
            Assert.That(result.Id, Is.EqualTo(genre.Id));
            Assert.That(result.Name, Is.EqualTo(genre.Name));
        }
        [Test]
        public void Map_BookToBookResponse_MapsCorrectly()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Name = "Dune",
                PublicationDate = new DateTime(1965, 8, 1),
                AuthorId = 1,
                GenreId = 1,
                Author = new Author() { Id = 1 },
                Genre = new Genre() { Id = 1 },
            };
            // Act
            var result = mapper.Map<BookResponse>(book);
            // Assert
            Assert.That(result.Id, Is.EqualTo(book.Id));
            Assert.That(result.Name, Is.EqualTo(book.Name));
            Assert.That(result.PublicationDate, Is.EqualTo(book.PublicationDate));
            Assert.That(result.Author.Id, Is.EqualTo(book.Author.Id));
            Assert.That(result.Genre.Id, Is.EqualTo(book.Genre.Id));
        }
    }
}