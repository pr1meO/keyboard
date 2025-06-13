using CSharpFunctionalExtensions;
using ProductService.API.Contracts.CartProducts;
using ProductService.API.Error;
using ProductService.API.Interfaces.Repositories;
using ProductService.API.Interfaces.Services;
using ProductService.API.Models;

namespace ProductService.API.Implementations.Services
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