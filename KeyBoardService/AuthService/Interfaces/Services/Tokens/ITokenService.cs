using AuthService.API.Contracts.Tokens;
using AuthService.API.Models;

namespace AuthService.API.Interfaces.Services.Tokens
{
    public interface ITokenService
    {
        Task<JwtTokenResponse> GetAsync(User user);
    }
}
