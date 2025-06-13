using BusinessService.Contracts.Products;
using CSharpFunctionalExtensions;

namespace BusinessService.Interfaces.Products
{
    public interface IProductService
    {
        Task<Result> CreateAsync(CreateProductRequest request);
        Task<Result<List<ProductDto>>> GetAllAsync();
        Task<Result<ProductDto>> GetByIdAsync(Guid id);
        Task<Result> UpdateAsync(Guid id, UpdateProductRequest request);
        Task<Result> DeleteAsync(Guid id);
    }
}
