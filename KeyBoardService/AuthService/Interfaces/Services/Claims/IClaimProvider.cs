using System.Security.Claims;

namespace AuthService.API.Interfaces.Services.Claims
{
    public interface IClaimProvider
    {
        List<Claim> GetAccessClaims(Guid userId, string role);
        List<Claim> GetRefreshClaims(Guid userId, Guid tokenId);
    }
}
