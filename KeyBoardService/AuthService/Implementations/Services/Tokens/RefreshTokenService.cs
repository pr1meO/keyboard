using AuthService.API.Interfaces.Repositories;
using AuthService.API.Interfaces.Services.Tokens;
using AuthService.API.Models;

namespace AuthService.API.Implementations.Services.Tokens
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokensRepository _refreshRepository;

        public RefreshTokenService(
            IRefreshTokensRepository refreshRepository
        )
        {
            _refreshRepository = refreshRepository;
        }

        public async Task CreateAsync(Guid id, string token, Guid userId)
        {
            await _refreshRepository.AddAsync(new RefreshToken()
            {
                Id = id,
                Token = token,
                UserId = userId
            });
        }

        public async Task<RefreshToken?> GetByIdAsync(Guid id)
        {
            return await _refreshRepository.FindByIdAsync(id);
        }
    }
}
