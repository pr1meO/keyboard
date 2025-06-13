using BusinessService.Models;

namespace BusinessService.Interfaces.Auths.Tokens
{
    public interface IRefreshTokenService
    {
        Task CreateAsync(Guid id, string token, Guid userId);
        Task<RefreshToken?> GetByIdAsync(Guid id);
    }
}
