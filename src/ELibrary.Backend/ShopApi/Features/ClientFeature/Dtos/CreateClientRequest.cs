﻿namespace ShopApi.Features.ClientFeature.Dtos
{
    public class CreateClientRequest
    {
        public string? Name { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
