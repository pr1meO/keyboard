using BusinessService.Contracts;
using BusinessService.Models;

namespace BusinessService.Interfaces.Auths.Tokens
{
    public interface ITokenFactory
    {
        JwtTokenResponse Create(User user, Guid tokenId);
    }
}
