using BusinessService.Context;
using BusinessService.Interfaces.Repositories;
using BusinessService.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Repositories
{
    public class RefreshTokensRepository : IRefreshTokensRepository
    {
        private readonly AppDbContext _appDbContext;

        public RefreshTokensRepository(
            AppDbContext appDbContext
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
