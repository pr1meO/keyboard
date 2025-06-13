using BusinessService.Models;

namespace BusinessService.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task AddAsync(User user);
        Task<bool> ExistsByLoginAsync(string login);
        Task<User?> FindByLoginWithRoleAsync(string login);
    }
}
