using AuthService.API.Models;
using CSharpFunctionalExtensions;

namespace AuthService.API.Interfaces.Services
{
    public interface IRoleService
    {
        Task<Result<int>> GetIdByNameAsync(string name);
        Task<Result<Role>> GetByNameAsync(string name);
    }
}
