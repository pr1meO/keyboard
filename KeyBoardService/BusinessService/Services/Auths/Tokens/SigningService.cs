using BusinessService.Interfaces.Auths.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BusinessService.Services.Auths.Tokens
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
