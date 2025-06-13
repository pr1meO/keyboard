using BusinessService.Contracts.CartProducts;
using BusinessService.Models;
using CSharpFunctionalExtensions;

namespace BusinessService
{
    public interface ICartService
    {
        Task<Result<Cart>> CreateAsync(Guid userId);
        Task<Result<List<CartProductDto>>> GetProductsAsync(Guid userId);
        Task<Result<Guid>> GetIdAsync(Guid userId);
    }
}
