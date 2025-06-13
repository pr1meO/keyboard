using Microsoft.EntityFrameworkCore;
using ProductService.API.Contexts;
using ProductService.API.Interfaces.Repositories;
using ProductService.API.Models;

namespace ProductService.API.Implementations.Repositories
{
    public class BrandsRepository : IBrandsRepository
    {
        private readonly ProductDbContext _appDbContext;

        public BrandsRepository(
            ProductDbContext appDbContext
        )
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Brand brand)
        {
            await _appDbContext.Brands.AddAsync(brand);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _appDbContext.Brands
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Brand?> FindByIdAsync(Guid id)
        {
            return await _appDbContext.Brands
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int> UpdateAsync(Guid id, string name)
        {
            return await _appDbContext.Brands
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(b => b.Name, name));
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            return await _appDbContext.Brands
                .Where(b => b.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _appDbContext.Brands
                .AnyAsync(b => b.Name == name);
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _appDbContext.Brands
                .AnyAsync(b => b.Id == id);
        }
    }
}
