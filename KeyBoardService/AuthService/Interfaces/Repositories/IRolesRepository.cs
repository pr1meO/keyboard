using AuthService.API.Models;

namespace AuthService.API.Interfaces.Repositories
{
    public interface IRolesRepository
    {
        Task<int?> FindIdByNameAsync(string name);
        Task<Role?> FindByNameAsync(string name);
    }
}
