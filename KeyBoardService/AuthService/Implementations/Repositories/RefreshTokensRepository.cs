using AuthService.API.Context;
using AuthService.API.Interfaces.Repositories;
using AuthService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Implementations.Repositories
{
    public class RefreshTokensRepository : IRefreshTokensRepository
    {
        private readonly AuthDbContext _appDbContext;

        public RefreshTokensRepository(
            AuthDbContext appDbContext
        )
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _appDbContext.RefreshTokens.AddAsync(token);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken?> FindByIdAsync(Guid id)
        {
            return await _appDbContext.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }

}
