using BusinessService.Contracts.Categories;
using CSharpFunctionalExtensions;

namespace BusinessService.Interfaces.Products
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
