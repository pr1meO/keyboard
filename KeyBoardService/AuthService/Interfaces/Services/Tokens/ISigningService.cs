using Microsoft.IdentityModel.Tokens;

namespace AuthService.API.Interfaces.Services.Tokens
{
    public interface ISigningService
    {
        SigningCredentials GetSigningCredentials(string key);
    }
}
