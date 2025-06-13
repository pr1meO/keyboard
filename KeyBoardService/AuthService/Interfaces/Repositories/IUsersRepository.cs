using AuthService.API.Models;

namespace AuthService.API.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task AddAsync(User user);
        Task<bool> ExistsByLoginAsync(string login);
        Task<User?> FindByLoginWithRoleAsync(string login);
    }
}
