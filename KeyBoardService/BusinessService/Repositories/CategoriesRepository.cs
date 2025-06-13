using BusinessService.Context;
using BusinessService.Interfaces.Repositories;
using BusinessService.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoriesRepository(
            AppDbContext appDbContext
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
