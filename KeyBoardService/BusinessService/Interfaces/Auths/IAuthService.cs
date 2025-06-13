using BusinessService.Contracts;
using CSharpFunctionalExtensions;

namespace BusinessService.Interfaces.Auths
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterUserRequest request);
        Task<Result<JwtTokenResponse>> LoginAsync(LoginUserRequest request);
    }
}
