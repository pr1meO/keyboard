using AuthService.API.Models;

namespace AuthService.API.Interfaces.Repositories
{
    public interface IRefreshTokensRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> FindByIdAsync(Guid id);
    }
}
