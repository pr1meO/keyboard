using BusinessService.Contracts.CartProducts;
using BusinessService.Error;
using BusinessService.Interfaces.Cart;
using BusinessService.Interfaces.Repositories;
using BusinessService.Models;
using CSharpFunctionalExtensions;

namespace BusinessService.Services
{
    public class CartProductService : ICartProductService
    {
        private readonly ICartProductsRepository _cartProductsRepository;
        private readonly IStockService _stockService;
        private readonly ILogger<CartProductService> _logger;

        public CartProductService(
            ICartProductsRepository cartProductsRepository,
            IStockService stockService,
            ILogger<CartProductService> logger
        )
        {
            _cartProductsRepository = cartProductsRepository;
            _stockService = stockService;
            _logger = logger;
        }

        public async Task<Result> AddAsync(Guid cartId, CreateCartProductRequest request)
        {
            var result = await _cartProductsRepository.ExistsByProductIdAsync(cartId, request.ProductId);

            if (result)
            {
                _logger.LogWarning(ErrorMessage.Exists(nameof(CartProduct)));
                return Result.Failure(ErrorMessage.Exists(nameof(CartProduct)));
            }

            var checkResult = await _stockService.CheckStockAsync(request.ProductId, request.Quantity);

            if (checkResult.IsFailure)
                return Result.Failure(checkResult.Error);

            await _cartProductsRepository.AddAsync(new CartProduct
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                CartId = cartId
            });

            return Result.Success(await _stockService.UpdateStockAsync(request.ProductId, request.Quantity));
        }

        public async Task<Result> UpdateQuantityAsync(Guid id, Guid cartId, int quantity)
        {
            var cartProduct = await _cartProductsRepository.FindByIdAsync(id, cartId);

            if (cartProduct == null)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(CartProduct)));
                return Result.Failure<int>(ErrorMessage.NotFound(nameof(CartProduct)));
            }

            var difference = quantity - cartProduct.Quantity;

            var stockResult = await _stockService.CheckStockAsync(cartProduct.ProductId, difference);

            if (stockResult.IsFailure)
                return Result.Failure(stockResult.Error);

            var result = await _cartProductsRepository.UpdateQuantityAsync(id, cartId, quantity);

            if (result == -1)
            {
                _logger.LogWarning(ErrorMessage.FailedUpdate(nameof(CartProduct)));
                return Result.Failure(ErrorMessage.FailedUpdate(nameof(CartProduct)));
            }

            return Result.Success(await _stockService.UpdateStockAsync(cartProduct.ProductId, difference));
        }

        public async Task<Result> DeleteAsync(Guid id, Guid cartId)
        {
            var cartProduct = await _cartProductsRepository.FindByIdAsync(id, cartId);

            if (cartProduct == null)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(CartProduct)));
                return Result.Failure(ErrorMessage.NotFound(nameof(CartProduct)));
            }

            var result = await _cartProductsRepository.RemoveAsync(id, cartId);

            if (result == -1)
            {
                _logger.LogWarning(ErrorMessage.FailedDelete(nameof(CartProduct)));
                return Result.Failure(ErrorMessage.FailedDelete(nameof(CartProduct)));
            }

            return Result.Success(await _stockService.UpdateStockAsync(cartProduct.ProductId, -cartProduct.Quantity));
        }

        public async Task<Result> ClearAsync(Guid cartId)
        {
            var result = await _cartProductsRepository.ClearAsync(cartId);

            if (result == -1)
            {
                _logger.LogWarning(ErrorMessage.FailedDelete(nameof(CartProduct)));
                return Result.Failure(ErrorMessage.FailedDelete(nameof(CartProduct)));
            }

            return Result.Success();
        }
    }
}

public class StockService : IStockService
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<StockService> _logger;

    public StockService(
        IProductsRepository productsRepository,
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

public interface IStockService
{
    Task<Result<int>> GetById(Guid productId);
    Task<Result> CheckStockAsync(Guid productId, int quantity);
    Task<Result> UpdateStockAsync(Guid productId, int quantity);
}