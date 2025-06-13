using ProductService.API.Models;

namespace ProductService.API.Interfaces.Repositories
{
    public interface ICategoriesRepository
    {
        Task AddAsync(Category brand);
        Task<List<Category>> GetAllAsync();
        Task<Category?> FindByIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, string name);
        Task<int> RemoveAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByIdAsync(Guid id);
    }
}
