using AuthService.API.Contracts.Tokens;
using AuthService.API.Contracts.Users;
using CSharpFunctionalExtensions;

namespace AuthService.API.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<Result> RegisterAsync(RegisterUserRequest request);
        Task<Result<JwtTokenResponse>> LoginAsync(LoginUserRequest request);
    }
}
