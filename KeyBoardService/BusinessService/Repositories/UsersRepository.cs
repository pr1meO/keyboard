using BusinessService.Context;
using BusinessService.Interfaces.Repositories;
using BusinessService.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessService.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _appDbContext;

        public UsersRepository(
            AppDbContext appDbContext
        )
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(User user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByLoginAsync(string login)
        {
            return await _appDbContext.Users
                .AnyAsync(u => u.Login == login);   
        }

        public async Task<User?> FindByLoginWithRoleAsync(string login)
        {
            return await _appDbContext.Users
                .AsNoTracking()
                .Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.Login == login);
        }
    }
}
