using BusinessService.Models;

namespace BusinessService.Interfaces.Repositories
{
    public interface IProductsRepository
    {
        Task AddAsync(Product product);
        Task<List<Product>> GetAllAsync();
        Task<Product?> FindByIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, Product product);
        Task<int> RemoveAsync(Guid id);
        Task<int> UpdateStockAsync(Guid id, int quantity);
    }
}
