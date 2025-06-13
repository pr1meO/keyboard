using BusinessService.Models;

namespace BusinessService.Interfaces.Repositories
{
    public interface IRolesRepository
    {
        Task<int?> FindIdByNameAsync(string name);
        Task<Role?> FindByNameAsync(string name);
    }
}
