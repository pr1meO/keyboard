using AuthService.API.Configurations;
using AuthService.API.Contracts.Tokens;
using AuthService.API.Interfaces.Services.Claims;
using AuthService.API.Interfaces.Services.Tokens;
using AuthService.API.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.API.Implementations.Services.Tokens
{
    public class TokenFactory : ITokenFactory
    {
        private readonly TokenOptions _options;
        private readonly IClaimProvider _claimProvider;
        private readonly ISigningService _signingService;

        public TokenFactory(
            IOptions<TokenOptions> options,
            IClaimProvider claimProvider,
            ISigningService signingService
        )
        {
            _options = options.Value;
            _claimProvider = claimProvider;
            _signingService = signingService;
        }

        public JwtTokenResponse Create(User user, Guid tokenId) =>
            new JwtTokenResponse()
            {
                Access = CreateAccessToken(user.Id, user.Role?.Name!),
                Refresh = CreateRefreshToken(user.Id, tokenId)
            };

        private string CreateAccessToken(Guid userId, string role) =>
            new JwtSecurityTokenHandler()
                .WriteToken(
                    new JwtSecurityToken(
                        issuer: _options.Issuer,
                        audience: _options.Audience,
                        notBefore: DateTime.UtcNow,
                        expires: DateTime.UtcNow.AddHours(_options.AccessExpiresHours),
                        claims: _claimProvider.GetAccessClaims(userId, role),
                        signingCredentials: _signingService.GetSigningCredentials(_options.Key)));

        private string CreateRefreshToken(Guid userId, Guid tokenId) =>
            new JwtSecurityTokenHandler()
                .WriteToken(
                    new JwtSecurityToken(
                        issuer: _options.Issuer,
                        audience: _options.Audience,
                        notBefore: DateTime.UtcNow,
                        expires: DateTime.UtcNow.AddDays(_options.RefreshExpiresDays),
                        claims: _claimProvider.GetRefreshClaims(userId, tokenId),
                        signingCredentials: _signingService.GetSigningCredentials(_options.Key)));
    }
}
