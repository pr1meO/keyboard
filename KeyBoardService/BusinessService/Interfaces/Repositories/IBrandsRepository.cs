using BusinessService.Models;

namespace BusinessService.Interfaces.Repositories
{
    public interface IBrandsRepository
    {
        Task AddAsync(Brand brand);
        Task<List<Brand>> GetAllAsync();
        Task<Brand?> FindByIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, string name);
        Task<int> RemoveAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByIdAsync(Guid id);
    }
}
