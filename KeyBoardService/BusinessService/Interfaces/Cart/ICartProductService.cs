using BusinessService.Contracts.CartProducts;
using CSharpFunctionalExtensions;

namespace BusinessService.Interfaces.Cart
{
    public interface ICartProductService
    {
        Task<Result> AddAsync(Guid cartId, CreateCartProductRequest request);
        Task<Result> UpdateQuantityAsync(Guid id, Guid cartId, int quantity);
        Task<Result> DeleteAsync(Guid id, Guid cartId);
        Task<Result> ClearAsync(Guid cartId);
    }
}