using BusinessService.Models;
using CSharpFunctionalExtensions;

namespace BusinessService.Interfaces.Roles
{
    public interface IRoleService
    {
        Task<Result<int>> GetIdByNameAsync(string name);
        Task<Result<Role>> GetByNameAsync(string name);
    }
}
