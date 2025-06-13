using AuthService.API.Contracts.Users;
using AuthService.API.Models;
using CSharpFunctionalExtensions;

namespace AuthService.API.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<Guid>> CreateAsync(RegisterUserRequest request);
        Task<Result> ExistsByLoginAsync(string login);
        Task<Result<User>> GetByLoginAsync(string login);
    }
}
