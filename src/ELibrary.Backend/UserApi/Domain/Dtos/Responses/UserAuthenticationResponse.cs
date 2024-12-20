﻿namespace UserApi.Domain.Dtos.Responses
{
    public class UserAuthenticationResponse
    {
        public AuthToken? AuthToken { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = [];
    }
}
