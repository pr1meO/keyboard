using BusinessService.Contracts.Brands;
using BusinessService.Error;
using BusinessService.Interfaces.Products;
using BusinessService.Interfaces.Repositories;
using BusinessService.Models;
using CSharpFunctionalExtensions;

namespace BusinessService.Services.Products
{
    public class BrandService : IBrandService
    {
        private readonly IBrandsRepository _brandsRepository;
        private readonly ILogger<BrandService> _logger;

        public BrandService(
            IBrandsRepository brandRepository,
            ILogger<BrandService> logger
        )
        {
            _brandsRepository = brandRepository;
            _logger = logger;
        }

        public async Task<Result> ExistsByIdAsync(Guid id)
        {
            var result = await _brandsRepository.ExistsByIdAsync(id);

            if (!result)
            {
                _logger.LogWarning(ErrorMessage.NotExists(nameof(Brand)));
                return Result.Failure(ErrorMessage.NotExists(nameof(Brand)));
            }

            return Result.Success();
        }

        public async Task<Result> CreateAsync(CreateBrandRequest request)
        {
            var result = await _brandsRepository.ExistsByNameAsync(request.Name);

            if (result)
            {
                _logger.LogWarning(ErrorMessage.Exists(nameof(Brand)));
                return Result.Failure(ErrorMessage.Exists(nameof(Brand)));
            }

            await _brandsRepository.AddAsync(new Brand()
            {
                Name = request.Name
            });

            return Result.Success();
        }

        public async Task<Result<List<BrandDto>>> GetAllAsync()
        {
            var brands = await _brandsRepository.GetAllAsync();

            if (brands.Count == 0)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(Brand)));
                return Result.Failure<List<BrandDto>>(ErrorMessage.NotFound(nameof(Brand)));
            }

            var brandDtos = brands
                .Select(b => new BrandDto
                {
                    Id = b.Id,
                    Name = b.Name,
                })
                .ToList();

            return Result.Success(brandDtos);
        }

        public async Task<Result<BrandDto>> GetByIdAsync(Guid id)
        {
            var brand = await _brandsRepository.FindByIdAsync(id);

            if (brand == null)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(Brand)));
                return Result.Failure<BrandDto>(ErrorMessage.NotFound(nameof(Brand)));
            }

            var brandDto = new BrandDto()
            {
                Id = brand.Id,
                Name = brand.Name,
            };

            return Result.Success(brandDto);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var result = await _brandsRepository.RemoveAsync(id);

            if (result == 0)
            {
                _logger.LogWarning(ErrorMessage.FailedDelete(nameof(Brand)));
                return Result.Failure(ErrorMessage.FailedDelete(nameof(Brand)));
            }

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(Guid id, string name)
        {
            var result = await _brandsRepository.UpdateAsync(id, name);

            if (result == 0)
            {
                _logger.LogWarning(ErrorMessage.FailedUpdate(nameof(Brand)));
                return Result.Failure(ErrorMessage.FailedUpdate(nameof(Brand)));
            }

            return Result.Success();
        }
    }
}