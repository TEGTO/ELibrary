﻿namespace UserApi.Domain.Dtos
{
    public class AuthToken
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryDate { get; set; }
    }
}
