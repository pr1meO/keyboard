using BusinessService.Context;
using BusinessService.Interfaces.Repositories;
using BusinessService.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly AppDbContext _appDbContext;

        public RolesRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int?> FindIdByNameAsync(string name)
        {
            return await _appDbContext.Roles
                .Where(r => r.Name == name)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Role?> FindByNameAsync(string name)
        {
            return await _appDbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
