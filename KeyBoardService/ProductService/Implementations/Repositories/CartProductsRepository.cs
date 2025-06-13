using Microsoft.EntityFrameworkCore;
using ProductService.API.Contexts;
using ProductService.API.Interfaces.Repositories;
using ProductService.API.Models;

namespace ProductService.API.Implementations.Repositories
{
    public class CartProductsRepository : ICartProductsRepository
    {
        private readonly ProductDbContext _appDbcontext;

        public CartProductsRepository(
            ProductDbContext appDbContext
        )
        {
            _appDbcontext = appDbContext;
        }

        public async Task AddAsync(CartProduct cartProduct)
        {
            await _appDbcontext.CartProducts.AddAsync(cartProduct);
            await _appDbcontext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByProductIdAsync(Guid cartId, Guid productId)
        {
            return await _appDbcontext.CartProducts
                .Where(cp => cp.CartId == cartId && cp.ProductId == productId)
                .AnyAsync();
        }


        public async Task<CartProduct?> FindByIdAsync(Guid id, Guid cartId)
        {
            return await _appDbcontext.CartProducts
                .Where(cp => cp.Id == id && cp.CartId == cartId)
                .Include(cp => cp.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateQuantityAsync(Guid id, Guid cartId, int quantity)
        {
            return await _appDbcontext.CartProducts
                .Where(cp => cp.Id == id && cp.CartId == cartId)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(cp => cp.Quantity, quantity));
        }

        public async Task<int> RemoveAsync(Guid id, Guid cartId)
        {
            return await _appDbcontext.CartProducts
                .Where(cp => cp.Id == id && cp.CartId == cartId)
                .ExecuteDeleteAsync();
        }

        public async Task<int> ClearAsync(Guid cartId)
        {
            return await _appDbcontext.CartProducts
                .Where(cp => cp.CartId == cartId)
                .ExecuteDeleteAsync();
        }
    }
}
