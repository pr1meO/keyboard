using AuthService.API.Context;
using AuthService.API.Interfaces.Repositories;
using AuthService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Implementations.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AuthDbContext _appDbContext;

        public UsersRepository(
            AuthDbContext appDbContext
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
