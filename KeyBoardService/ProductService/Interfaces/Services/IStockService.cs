using CSharpFunctionalExtensions;

namespace ProductService.API.Interfaces.Services
{
    public interface IStockService
    {
        Task<Result<int>> GetById(Guid productId);
        Task<Result> CheckStockAsync(Guid productId, int quantity);
        Task<Result> UpdateStockAsync(Guid productId, int quantity);
    }
}
