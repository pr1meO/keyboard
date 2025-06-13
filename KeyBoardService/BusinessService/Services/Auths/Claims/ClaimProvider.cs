using BusinessService.Interfaces.Auths.Claims;
using System.Security.Claims;

namespace BusinessService.Services.Auths.Claims
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