using System.Security.Claims;

namespace BusinessService.Interfaces.Auths.Claims
{
    public interface IClaimProvider
    {
        List<Claim> GetAccessClaims(Guid userId, string role);
        List<Claim> GetRefreshClaims(Guid userId, Guid tokenId);
    }
}
