using Microsoft.EntityFrameworkCore;
using ProductService.API.Contexts;
using ProductService.API.Interfaces.Repositories;
using ProductService.API.Models;

namespace ProductService.API.Implementations.Repositories
{
    public class CatalogsRepository : ICatalogsRepository
    {
        private readonly ProductDbContext _appDbContext;

        public CatalogsRepository(
            ProductDbContext appDbContext
        )
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Product product)
        {
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _appDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> FindByIdAsync(Guid id)
        {
            return await _appDbContext.Products
                .Where(p => p.Id == id)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            return await _appDbContext.Products
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<int> UpdateAsync(Guid id, Product updProduct)
        {
            var product = await _appDbContext.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return -1;

            product.Name = updProduct.Name;
            product.Description = updProduct.Description;
            product.Stock = updProduct.Stock;
            product.Price = updProduct.Price;
            product.CategoryId = updProduct.CategoryId;
            product.BrandId = updProduct.BrandId;

            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateStockAsync(Guid id, int quantity)
        {
            return await _appDbContext.Products
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(p => p.Stock, quantity));
        }
    }
}