using CSharpFunctionalExtensions;
using ProductService.API.Contracts.CartProducts;
using ProductService.API.Models;

namespace ProductService.API.Interfaces.Services
{
    public interface ICartService
    {
        Task<Result<Cart>> CreateAsync(Guid userId);
        Task<Result<List<CartProductDto>>> GetProductsAsync(Guid userId);
        Task<Result<Guid>> GetIdAsync(Guid userId);
    }
}
