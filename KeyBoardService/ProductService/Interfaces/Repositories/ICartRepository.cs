using ProductService.API.Models;

namespace ProductService.API.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task CreateAsync(Cart cart);
        Task<Guid> GetIdByUserIdAsync(Guid userId);
        Task<Cart?> FindByUserIdAsync(Guid userId);
    }
}
