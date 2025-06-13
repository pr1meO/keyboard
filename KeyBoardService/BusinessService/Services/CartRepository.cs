using BusinessService.Context;
using BusinessService.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Services
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;

        public CartRepository(
            AppDbContext appDbContext
        )
        {
            _appDbContext = appDbContext;
        }

        public async Task CreateAsync(Cart cart)
        {
            await _appDbContext.Carts.AddAsync(cart);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Guid> GetIdByUserIdAsync(Guid userId)
        {
            return await _appDbContext.Carts
                .Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Cart?> FindByUserIdAsync(Guid userId)
        {
            return await _appDbContext.Carts
                .Where(c => c.UserId == userId)
                .Include(cp => cp.CartProducts)
                    .ThenInclude(p => p.Product)
                .AsTracking()
                .FirstOrDefaultAsync();
        }
    }
}
