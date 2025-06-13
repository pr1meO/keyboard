using AuthService.API.Interfaces.Services.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.API.Implementations.Services.Tokens
{
    public class SigningService : ISigningService
    {
        public SigningCredentials GetSigningCredentials(string key) =>
            new SigningCredentials
            (
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256
            );
    }
}
