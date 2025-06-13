using BusinessService.Interfaces.Auths.Tokens;
using BusinessService.Interfaces.Repositories;
using BusinessService.Models;

namespace BusinessService.Services.Auths.Tokens
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
