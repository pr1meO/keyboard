using CSharpFunctionalExtensions;
using ProductService.API.Contracts.CartProducts;

namespace ProductService.API.Interfaces.Services
{
    public interface ICartProductService
    {
        Task<Result> AddAsync(Guid cartId, CreateCartProductRequest request);
        Task<Result> UpdateQuantityAsync(Guid id, Guid cartId, int quantity);
        Task<Result> DeleteAsync(Guid id, Guid cartId);
        Task<Result> ClearAsync(Guid cartId);
    }
}