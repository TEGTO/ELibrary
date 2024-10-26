﻿using UserEntities.Domain.Entities;

namespace UserApi.Domain.Models
{
    public class CreateUserFromOAuth
    {
        public string Email { get; set; }
        public string LoginProviderSubject { get; set; }
        public AuthenticationMethod AuthMethod { get; set; }
    }
}