using BusinessService.Error;
using BusinessService.Interfaces.Repositories;
using BusinessService.Interfaces.Roles;
using BusinessService.Models;
using CSharpFunctionalExtensions;

namespace BusinessService.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            IRolesRepository rolesRepository,
            ILogger<RoleService> logger
        )
        {
            _rolesRepository = rolesRepository;
            _logger = logger;
        }

        public async Task<Result<int>> GetIdByNameAsync(string name)
        {
            var id = await _rolesRepository.FindIdByNameAsync(name);

            if (id == null)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(Role)));
                return Result.Failure<int>(ErrorMessage.NotFound(nameof(Role)));
            }

            return Result.Success(id.Value);
        }

        public async Task<Result<Role>> GetByNameAsync(string name)
        {
            var role = await _rolesRepository.FindByNameAsync(name);

            if (role == null)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(Role)));
                return Result.Failure<Role>(ErrorMessage.NotFound(nameof(Role)));
            }

            return Result.Success(role);
        }
    }
}
