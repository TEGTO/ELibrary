﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UserEntities.Domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        [Required]
        public List<UserAuthenticationMethod> AuthenticationMethods { get; set; } = new List<UserAuthenticationMethod>();
        [Required]
        public DateTime RegistredAtUtc { get; set; }
        [Required]
        public DateTime UpdatedAtUtc { get; set; }
    }
}