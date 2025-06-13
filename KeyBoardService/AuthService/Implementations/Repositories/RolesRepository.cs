using AuthService.API.Context;
using AuthService.API.Interfaces.Repositories;
using AuthService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Implementations.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly AuthDbContext _appDbContext;

        public RolesRepository(AuthDbContext appDbContext)
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
