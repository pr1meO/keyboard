using AuthService.API.Contracts.Tokens;
using AuthService.API.Models;

namespace AuthService.API.Interfaces.Services.Tokens
{
    public interface ITokenFactory
    {
        JwtTokenResponse Create(User user, Guid tokenId);
    }
}
