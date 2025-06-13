using AuthService.API.Interfaces.Services.Claims;
using System.Security.Claims;

namespace AuthService.API.Implementations.Services.Claims
{
    public class ClaimProvider : IClaimProvider
    {
        public List<Claim> GetAccessClaims(Guid userId, string role) =>
            [
                CreateUserIdClaim(userId),
                CreateRoleClaim(role)
            ];

        public List<Claim> GetRefreshClaims(Guid userId, Guid tokenId) =>
            [
                CreateUserIdClaim(userId),
                CreateTokenIdClaim(tokenId)
            ];

        private Claim CreateRoleClaim(string role) => new Claim(ClaimTypes.Role, role);
        private Claim CreateUserIdClaim(Guid id) => new Claim(ClaimTypes.NameIdentifier, id.ToString());
        private Claim CreateTokenIdClaim(Guid id) => new Claim("TokenId", id.ToString());
    }
}