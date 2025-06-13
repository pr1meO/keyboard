using BusinessService.Models;

namespace BusinessService
{
    public interface ICartRepository
    {
        Task CreateAsync(Cart cart);
        Task<Guid> GetIdByUserIdAsync(Guid userId);
        Task<Cart?> FindByUserIdAsync(Guid userId);
    }
}
