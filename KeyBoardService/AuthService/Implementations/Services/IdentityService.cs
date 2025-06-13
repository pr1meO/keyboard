using AuthService.API.Contracts.Tokens;
using AuthService.API.Contracts.Users;
using AuthService.API.Error;
using AuthService.API.Interfaces.Rabbit;
using AuthService.API.Interfaces.Services;
using AuthService.API.Interfaces.Services.Hashs;
using AuthService.API.Interfaces.Services.Tokens;
using AuthService.API.Models;
using Background.Contracts;
using CSharpFunctionalExtensions;


namespace AuthService.API.Implementations.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IRabbitSendService _rabbitService;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(
            IUserService userService,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IRabbitSendService rabbitService,
            ILogger<IdentityService> logger
        )
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _rabbitService = rabbitService;
            _logger = logger;
        }

        public async Task<Result> RegisterAsync(RegisterUserRequest request)
        {
            var result = await _userService.ExistsByLoginAsync(request.Login);

            if (result.IsFailure)
                return Result.Failure(result.Error);

            var userIdResult = await _userService.CreateAsync(request);

            if (userIdResult.IsFailure)
                return Result.Failure(userIdResult.Error);

            await _rabbitService.SendAsync<IRegisteredUser>(new RegisteredUser()
            {
                Id = userIdResult.Value
            });

            return Result.Success();
        }

        public async Task<Result<JwtTokenResponse>> LoginAsync(LoginUserRequest request)
        {
            var user = await _userService.GetByLoginAsync(request.Login);

            if (user.IsFailure)
                return Result.Failure<JwtTokenResponse>(user.Error);

            var result = _passwordHasher.Verify(request.Password, user.Value.PasswordHash);

            if (!result)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(User)));
                return Result.Failure<JwtTokenResponse>(ErrorMessage.NotFound(nameof(User)));
            }

            return Result.Success(await _tokenService.GetAsync(user.Value));
        }
    }
}