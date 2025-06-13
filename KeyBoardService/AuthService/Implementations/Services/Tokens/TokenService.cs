using AuthService.API.Contracts.Tokens;
using AuthService.API.Interfaces.Services.Tokens;
using AuthService.API.Models;

namespace AuthService.API.Implementations.Services.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly ITokenFactory _tokenFactory;
        private readonly IRefreshTokenService _refreshTokenService;

        public TokenService(
            ITokenFactory tokenFactory,
            IRefreshTokenService refreshTokenService
        )
        {
            _tokenFactory = tokenFactory;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<JwtTokenResponse> GetAsync(User user)
        {
            var refreshId = Guid.NewGuid();
            var tokens = _tokenFactory.Create(user, refreshId);

            await _refreshTokenService.CreateAsync(refreshId, tokens.Refresh, user.Id);

            return tokens;
        }
    }
}
