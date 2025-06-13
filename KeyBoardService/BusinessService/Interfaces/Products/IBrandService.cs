using BusinessService.Contracts.Brands;
using CSharpFunctionalExtensions;

namespace BusinessService.Interfaces.Products
{
    public interface IBrandService
    {
        Task<Result> CreateAsync(CreateBrandRequest request);
        Task<Result<List<BrandDto>>> GetAllAsync();
        Task<Result<BrandDto>> GetByIdAsync(Guid id);
        Task<Result> UpdateAsync(Guid id, string name);
        Task<Result> DeleteAsync(Guid id);
        Task<Result> ExistsByIdAsync(Guid id);
    }
}
