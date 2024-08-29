﻿namespace LibraryApi.Domain.Dto.Author
{
    public class UpdateAuthorRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}