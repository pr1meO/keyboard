using BusinessService.Contracts;
using BusinessService.Error;
using BusinessService.Interfaces.Auths;
using BusinessService.Interfaces.Auths.Tokens;
using BusinessService.Interfaces.Hashs;
using BusinessService.Interfaces.Users;
using BusinessService.Models;
using CSharpFunctionalExtensions;

namespace BusinessService.Services.Auths
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserService userService,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            ILogger<AuthService> logger
        )
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result> RegisterAsync(RegisterUserRequest request)
        {
            var result = await _userService.ExistsByLoginAsync(request.Login);

            if (result.IsFailure)
                return Result.Failure(result.Error);

            return await _userService.CreateAsync(request);
        }

        public async Task<Result<JwtTokenResponse>> LoginAsync(LoginUserRequest request)
        {
            var user = await _userService.GetByLoginAsync(request.Login);

            if (user.IsFailure)
                return Result.Failure<JwtTokenResponse>(user.Error);

            var isVerify = _passwordHasher.Verify(request.Password, user.Value.PasswordHash);

            if (!isVerify)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(User)));
                return Result.Failure<JwtTokenResponse>(ErrorMessage.NotFound(nameof(User)));
            }

            return Result.Success(await _tokenService.GetAsync(user.Value));
        }
    }
}