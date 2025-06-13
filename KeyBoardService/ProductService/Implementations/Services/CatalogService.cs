using CSharpFunctionalExtensions;
using ProductService.API.Contracts.Products;
using ProductService.API.Error;
using ProductService.API.Interfaces.Repositories;
using ProductService.API.Interfaces.Services;
using ProductService.API.Models;

namespace ProductService.API.Implementations.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogsRepository _productsRepository;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(
            ICatalogsRepository productsRepository,
            ICategoryService categoryService,
            IBrandService brandService,
            ILogger<CatalogService> logger
        )
        {
            _productsRepository = productsRepository;
            _categoryService = categoryService;
            _brandService = brandService;
            _logger = logger;
        }

        public async Task<Result> CreateAsync(CreateProductRequest request)
        {
            var validResult = await ValidateCategoryAndBrandAsync(request.CategoryId, request.BrandId);

            if (validResult.IsFailure)
                return Result.Failure(validResult.Error);

            await _productsRepository.AddAsync(new Product()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = request.CategoryId,
                BrandId = request.BrandId,
            });

            return Result.Success();
        }

        public async Task<Result<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productsRepository.GetAllAsync();

            if (products.Count == 0)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(Product)));
                return Result.Failure<List<ProductDto>>(ErrorMessage.NotFound(nameof(Product)));
            }

            var productDtos = products
                .Select(p => new ProductDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryName = p.Category!.Name,
                    BrandName = p.Brand!.Name
                })
                .ToList();

            return Result.Success(productDtos);
        }

        public async Task<Result<ProductDto>> GetByIdAsync(Guid id)
        {
            var product = await _productsRepository.FindByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(Product)));
                return Result.Failure<ProductDto>(ErrorMessage.NotFound(nameof(Product)));
            }

            var productDto = new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryName = product.Category!.Name,
                BrandName = product.Brand!.Name
            };

            return Result.Success(productDto);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var result = await _productsRepository.RemoveAsync(id);

            if (result == 0)
            {
                _logger.LogWarning(ErrorMessage.FailedDelete(nameof(Product)));
                return Result.Failure(ErrorMessage.FailedDelete(nameof(Product)));
            }

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateProductRequest request)
        {
            var validResult = await ValidateCategoryAndBrandAsync(request.CategoryId, request.BrandId);

            if (validResult.IsFailure)
                return Result.Failure(validResult.Error);

            var result = await _productsRepository.UpdateAsync(id, new Product()
            {
                Name = request.Name,
                Description = request.Description,
                Stock = request.Stock,
                Price = request.Price,
                CategoryId = request.CategoryId,
                BrandId = request.BrandId
            });

            if (result == -1)
            {
                _logger.LogWarning(ErrorMessage.FailedUpdate(nameof(Product)));
                return Result.Failure(ErrorMessage.FailedUpdate(nameof(Product)));
            }

            return Result.Success();
        }

       private async Task<Result> ValidateCategoryAndBrandAsync(Guid categoryId, Guid brandId)
       {
            var category = await _categoryService.ExistsByIdAsync(categoryId);

            if (category.IsFailure)
                return Result.Failure(category.Error);

            var brand = await _brandService.ExistsByIdAsync(brandId);

            if (brand.IsFailure)
                return Result.Failure(brand.Error);

            return Result.Success();
       }
    }
}