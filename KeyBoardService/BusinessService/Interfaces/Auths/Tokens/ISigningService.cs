using Microsoft.IdentityModel.Tokens;

namespace BusinessService.Interfaces.Auths.Tokens
{
    public interface ISigningService
    {
        SigningCredentials GetSigningCredentials(string key);
    }
}
