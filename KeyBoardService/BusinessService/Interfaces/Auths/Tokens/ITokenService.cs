using BusinessService.Contracts;
using BusinessService.Models;

namespace BusinessService.Interfaces.Auths.Tokens
{
    public interface ITokenService
    {
        Task<JwtTokenResponse> GetAsync(User user);
    }
}
