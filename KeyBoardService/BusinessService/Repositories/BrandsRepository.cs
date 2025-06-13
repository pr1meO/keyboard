using BusinessService.Context;
using BusinessService.Interfaces.Repositories;
using BusinessService.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Repositories
{
    public class BrandsRepository : IBrandsRepository
    {
        private readonly AppDbContext _appDbContext;

        public BrandsRepository(
            AppDbContext appDbContext
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
