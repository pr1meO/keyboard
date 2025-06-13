using BusinessService.Models;

namespace BusinessService.Interfaces.Repositories
{
    public interface IRefreshTokensRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> FindByIdAsync(Guid id);
    }
}
