using AuthService.API.Models;

namespace AuthService.API.Interfaces.Services.Tokens
{
    public interface IRefreshTokenService
    {
        Task CreateAsync(Guid id, string token, Guid userId);
        Task<RefreshToken?> GetByIdAsync(Guid id);
    }
}
