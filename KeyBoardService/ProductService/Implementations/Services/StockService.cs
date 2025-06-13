using CSharpFunctionalExtensions;
using ProductService.API.Error;
using ProductService.API.Interfaces.Repositories;
using ProductService.API.Interfaces.Services;
using ProductService.API.Models;

namespace ProductService.API.Implementations.Services
{
    public class StockService : IStockService
    {
        private readonly ICatalogsRepository _productsRepository;
        private readonly ILogger<StockService> _logger;

        public StockService(
            ICatalogsRepository productsRepository,
            ILogger<StockService> logger
        )
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        public async Task<Result<int>> GetById(Guid productId)
        {
            var product = await _productsRepository.FindByIdAsync(productId);

            if (product == null)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(Product)));
                return Result.Failure<int>(ErrorMessage.NotFound(nameof(Product)));
            }

            return Result.Success(product.Stock);
        }

        public async Task<Result> CheckStockAsync(Guid productId, int quantity)
        {
            var stockResult = await GetById(productId);

            if (stockResult.IsFailure)
                return Result.Failure(stockResult.Error);

            if (stockResult.Value < quantity)
            {
                _logger.LogWarning(ErrorMessage.NotEnough(nameof(CartProduct)));
                return Result.Failure(ErrorMessage.NotEnough(nameof(CartProduct)));
            }

            return Result.Success();
        }

        public async Task<Result> UpdateStockAsync(Guid productId, int quantity)
        {
            var stockResult = await GetById(productId);

            if (stockResult.IsFailure)
                return Result.Failure(stockResult.Error);

            var result = await _productsRepository.UpdateStockAsync(productId, stockResult.Value - quantity);

            if (result == -1)
            {
                _logger.LogWarning(ErrorMessage.FailedUpdate(nameof(Product)));
                return Result.Failure(ErrorMessage.FailedUpdate(nameof(Product)));
            }

            return Result.Success();
        }
    }
}
