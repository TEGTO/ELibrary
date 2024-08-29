﻿using AutoMapper;
using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Entities;

namespace LibraryApi
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
        public void Map_AuthorToGetAuthorResponse_MapsCorrectly()
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
            var result = mapper.Map<GetAuthorResponse>(author);
            // Assert
            Assert.That(result.Id, Is.EqualTo(author.Id));
            Assert.That(result.Name, Is.EqualTo(author.Name));
            Assert.That(result.LastName, Is.EqualTo(author.LastName));
            Assert.That(result.DateOfBirth, Is.EqualTo(author.DateOfBirth));
        }
        [Test]
        public void Map_CreateAuthorRequestToAuthor_MapsCorrectly()
        {
            // Arrange
            var createRequest = new CreateAuthorRequest
            {
                Name = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1)
            };
            // Act
            var result = mapper.Map<Author>(createRequest);
            // Assert
            Assert.That(result.Name, Is.EqualTo(createRequest.Name));
            Assert.That(result.LastName, Is.EqualTo(createRequest.LastName));
            Assert.That(result.DateOfBirth, Is.EqualTo(createRequest.DateOfBirth));
        }
        [Test]
        public void Map_AuthorToCreateAuthorResponse_MapsCorrectly()
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
            var result = mapper.Map<CreateAuthorResponse>(author);
            // Assert
            Assert.That(result.Id, Is.EqualTo(author.Id));
            Assert.That(result.Name, Is.EqualTo(author.Name));
            Assert.That(result.LastName, Is.EqualTo(author.LastName));
            Assert.That(result.DateOfBirth, Is.EqualTo(author.DateOfBirth));
        }
        [Test]
        public void Map_GenreToGetGenreResponse_MapsCorrectly()
        {
            // Arrange
            var genre = new Genre
            {
                Id = 1,
                Name = "Science Fiction"
            };
            // Act
            var result = mapper.Map<GetGenreResponse>(genre);
            // Assert
            Assert.That(result.Id, Is.EqualTo(genre.Id));
            Assert.That(result.Name, Is.EqualTo(genre.Name));
        }
        [Test]
        public void Map_CreateGenreRequestToGenre_MapsCorrectly()
        {
            // Arrange
            var createRequest = new CreateGenreRequest
            {
                Name = "Fantasy"
            };
            // Act
            var result = mapper.Map<Genre>(createRequest);
            // Assert
            Assert.That(result.Name, Is.EqualTo(createRequest.Name));
        }
        [Test]
        public void Map_GenreToCreateGenreResponse_MapsCorrectly()
        {
            // Arrange
            var genre = new Genre
            {
                Id = 1,
                Name = "Fantasy"
            };
            // Act
            var result = mapper.Map<CreateGenreResponse>(genre);
            // Assert
            Assert.That(result.Id, Is.EqualTo(genre.Id));
            Assert.That(result.Name, Is.EqualTo(genre.Name));
        }
        [Test]
        public void Map_BookToGetBookResponse_MapsCorrectly()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Dune",
                PublicationDate = new DateTime(1965, 8, 1),
                AuthorId = 1,
                GenreId = 1
            };
            // Act
            var result = mapper.Map<GetBookResponse>(book);
            // Assert
            Assert.That(result.Id, Is.EqualTo(book.Id));
            Assert.That(result.Title, Is.EqualTo(book.Title));
            Assert.That(result.PublicationDate, Is.EqualTo(book.PublicationDate));
            Assert.That(result.AuthorId, Is.EqualTo(book.AuthorId.ToString()));
            Assert.That(result.GenreId, Is.EqualTo(book.GenreId.ToString()));
        }
        [Test]
        public void Map_CreateBookRequestToBook_MapsCorrectly()
        {
            // Arrange
            var createRequest = new CreateBookRequest
            {
                Title = "Dune",
                PublicationDate = new DateTime(1965, 8, 1),
                AuthorId = "1",
                GenreId = "1"
            };
            // Act
            var result = mapper.Map<Book>(createRequest);
            // Assert
            Assert.That(result.Title, Is.EqualTo(createRequest.Title));
            Assert.That(result.PublicationDate, Is.EqualTo(createRequest.PublicationDate));
            Assert.That(result.AuthorId, Is.EqualTo(int.Parse(createRequest.AuthorId)));
            Assert.That(result.GenreId, Is.EqualTo(int.Parse(createRequest.GenreId)));
        }
        [Test]
        public void Map_BookToCreateBookResponse_MapsCorrectly()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Dune",
                PublicationDate = new DateTime(1965, 8, 1),
                AuthorId = 1,
                GenreId = 1
            };
            // Act
            var result = mapper.Map<CreateBookResponse>(book);
            // Assert
            Assert.That(result.Id, Is.EqualTo(book.Id));
            Assert.That(result.Title, Is.EqualTo(book.Title));
            Assert.That(result.PublicationDate, Is.EqualTo(book.PublicationDate));
            Assert.That(result.AuthorId, Is.EqualTo(book.AuthorId.ToString()));
            Assert.That(result.GenreId, Is.EqualTo(book.GenreId.ToString()));
        }
    }
}