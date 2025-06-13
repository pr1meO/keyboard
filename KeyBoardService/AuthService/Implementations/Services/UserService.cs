using AuthService.API.Contracts.Users;
using AuthService.API.Enums;
using AuthService.API.Error;
using AuthService.API.Interfaces.Repositories;
using AuthService.API.Interfaces.Services;
using AuthService.API.Interfaces.Services.Hashs;
using AuthService.API.Models;
using CSharpFunctionalExtensions;

namespace AuthService.API.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRoleService _roleService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUsersRepository usersRepository,
            IRoleService roleService,
            IPasswordHasher passwordHasher,
            ILogger<UserService> logger
        )
        {
            _usersRepository = usersRepository;
            _roleService = roleService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Result<Guid>> CreateAsync(RegisterUserRequest request)
        {
            var roleId = await _roleService.GetIdByNameAsync(RoleType.User.ToString());

            if (roleId.IsFailure)
                return Result.Failure<Guid>(roleId.Error);

            var passwordHash = _passwordHasher.Generate(request.Password);

            var user = new User()
            {
                Lastname = request.Lastname,
                Firstname = request.Firstname,
                Login = request.Login,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = passwordHash,
                RoleId = roleId.Value
            };

            await _usersRepository.AddAsync(user);

            return Result.Success(user.Id);
        }

        public async Task<Result> ExistsByLoginAsync(string login)
        {
            var result = await _usersRepository.ExistsByLoginAsync(login);

            if (result)
            {
                _logger.LogWarning(ErrorMessage.Exists(nameof(User)));
                return Result.Failure(ErrorMessage.Exists(nameof(User)));
            }

            return Result.Success();
        }

        public async Task<Result<User>> GetByLoginAsync(string login)
        {
            var user = await _usersRepository.FindByLoginWithRoleAsync(login);

            if (user == null)
            {
                _logger.LogWarning(ErrorMessage.NotExists(nameof(User)));
                return Result.Failure<User>(ErrorMessage.NotExists(nameof(User)));
            }

            return Result.Success(user);
        }
    }
}
