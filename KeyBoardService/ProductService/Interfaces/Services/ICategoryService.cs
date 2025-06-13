using CSharpFunctionalExtensions;
using ProductService.API.Contracts.Categories;

namespace ProductService.API.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<Result> CreateAsync(CreateCategoryRequest request);
        Task<Result<List<CategoryDto>>> GetAllAsync();
        Task<Result<CategoryDto>> GetByIdAsync(Guid id);
        Task<Result> UpdateAsync(Guid id, string name);
        Task<Result> DeleteAsync(Guid id);
        Task<Result> ExistsByIdAsync(Guid id);
    }
}
