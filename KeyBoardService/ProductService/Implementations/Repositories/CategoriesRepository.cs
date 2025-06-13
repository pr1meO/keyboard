using Microsoft.EntityFrameworkCore;
using ProductService.API.Contexts;
using ProductService.API.Interfaces.Repositories;
using ProductService.API.Models;

namespace ProductService.API.Implementations.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ProductDbContext _appDbContext;

        public CategoriesRepository(
            ProductDbContext appDbContext
        )
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(Category category)
        {
            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _appDbContext.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category?> FindByIdAsync(Guid id)
        {
            return await _appDbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> UpdateAsync(Guid id, string name)
        {
            return await _appDbContext.Categories
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(c => c.Name, name));
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            return await _appDbContext.Categories
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _appDbContext.Categories
                .AnyAsync(c => c.Name == name);
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _appDbContext.Categories
                .AnyAsync(c => c.Id == id);
        }
    }
}
