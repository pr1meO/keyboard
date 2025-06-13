using BusinessService.Contracts;
using BusinessService.Models;
using CSharpFunctionalExtensions;

namespace BusinessService.Interfaces.Users
{
    public interface IUserService
    {
        Task<Result> CreateAsync(RegisterUserRequest request);
        Task<Result> ExistsByLoginAsync(string login);
        Task<Result<User>> GetByLoginAsync(string login);
    }
}
