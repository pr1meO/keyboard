using ProductService.API.Models;

namespace ProductService.API.Interfaces.Repositories
{
    public interface ICartProductsRepository
    {
        Task AddAsync(CartProduct cartProduct);
        Task<bool> ExistsByProductIdAsync(Guid cartId, Guid productId);
        Task<CartProduct?> FindByIdAsync(Guid id, Guid cartId);
        Task<int> RemoveAsync(Guid id, Guid cartId);
        Task<int> UpdateQuantityAsync(Guid id, Guid cartId, int quantity);
        Task<int> ClearAsync(Guid cartId);
    }
}