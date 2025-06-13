using BusinessService.Contracts.Categories;
using BusinessService.Error;
using BusinessService.Interfaces.Products;
using BusinessService.Interfaces.Repositories;
using BusinessService.Models;
using CSharpFunctionalExtensions;

namespace BusinessService.Services.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoriesRepository categoriesRepository,
            ILogger<CategoryService> logger
        )
        {
            _categoriesRepository = categoriesRepository;
            _logger = logger;
        }

        public async Task<Result> ExistsByIdAsync(Guid id)
        {
            var result = await _categoriesRepository.ExistsByIdAsync(id);

            if (!result)
            {
                _logger.LogWarning(ErrorMessage.NotExists(nameof(Category)));
                return Result.Failure(ErrorMessage.NotExists(nameof(Category)));
            }

            return Result.Success(result);
        }

        public async Task<Result> CreateAsync(CreateCategoryRequest request)
        {
            var result = await _categoriesRepository.ExistsByNameAsync(request.Name);

            if (result)
            {
                _logger.LogWarning(ErrorMessage.Exists(nameof(Category)));
                return Result.Failure(ErrorMessage.Exists(nameof(Category)));
            }

            await _categoriesRepository.AddAsync(new Category()
            {
                Name = request.Name
            });

            return Result.Success();
        }

        public async Task<Result<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoriesRepository.GetAllAsync();

            if (categories.Count == 0)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(Category)));
                return Result.Failure<List<CategoryDto>>(ErrorMessage.NotFound(nameof(Category)));
            }

            var categoryDtos = categories
                .Select(b => new CategoryDto
                {
                    Id = b.Id,
                    Name = b.Name,
                })
                .ToList();

            return Result.Success(categoryDtos);
        }

        public async Task<Result<CategoryDto>> GetByIdAsync(Guid id)
        {
            var category = await _categoriesRepository.FindByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(Category)));
                return Result.Failure<CategoryDto>(ErrorMessage.NotFound(nameof(Category)));
            }

            var categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
            };

            return Result.Success(categoryDto);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var result = await _categoriesRepository.RemoveAsync(id);

            if (result == 0)
            {
                _logger.LogWarning(ErrorMessage.FailedDelete(nameof(Category)));
                return Result.Failure(ErrorMessage.FailedDelete(nameof(Category)));
            }

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(Guid id, string name)
        {
            var result = await _categoriesRepository.UpdateAsync(id, name);

            if (result == 0)
            {
                _logger.LogWarning(ErrorMessage.FailedUpdate(nameof(Category)));
                return Result.Failure(ErrorMessage.FailedUpdate(nameof(Category)));
            }

            return Result.Success();
        }
    }
}
