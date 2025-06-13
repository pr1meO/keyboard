using CSharpFunctionalExtensions;
using ProductService.API.Contracts.Products;

namespace ProductService.API.Interfaces.Services
{
    public interface ICatalogService
    {
        Task<Result> CreateAsync(CreateProductRequest request);
        Task<Result<List<ProductDto>>> GetAllAsync();
        Task<Result<ProductDto>> GetByIdAsync(Guid id);
        Task<Result> UpdateAsync(Guid id, UpdateProductRequest request);
        Task<Result> DeleteAsync(Guid id);
    }
}
