﻿namespace AuthService.API.Contracts.Tokens
{
    public class JwtTokenResponse
    {
        public string Access { get; set; } = string.Empty;
        public string Refresh { get; set; } = string.Empty;
    }
}

