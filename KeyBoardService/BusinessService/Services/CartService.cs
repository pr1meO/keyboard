using BusinessService.Contracts.CartProducts;
using BusinessService.Error;
using BusinessService.Models;
using CSharpFunctionalExtensions;

namespace BusinessService.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartService> _logger;

        public CartService(
            ICartRepository cartRepository,
            ILogger<CartService> logger
        )
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<Result<Cart>> CreateAsync(Guid userId)
        {
            var cart = new Cart()
            {
                UserId = userId
            };
            await _cartRepository.CreateAsync(cart);

            _logger.LogInformation("Cart created.");

            return Result.Success(cart);
        }

        public async Task<Result<List<CartProductDto>>> GetProductsAsync(Guid userId)
        {
            var cart = await _cartRepository.FindByUserIdAsync(userId);

            if (cart == null)
            {
                var cartResult = await CreateAsync(userId);
                cart = cartResult.Value;
            }

            if (cart.CartProducts.Count == 0)
            {
                _logger.LogWarning(ErrorMessage.NotFound(nameof(CartProduct)));
                return Result.Failure<List<CartProductDto>>(ErrorMessage.NotFound(nameof(CartProduct)));
            }

            var cartProductDtos = cart.CartProducts
                .Select(cp => new CartProductDto
                {
                    Id = cp.Id,
                    Name = cp.Product!.Name,
                    Price = cp.Product.Price * cp.Quantity,
                    Quantity = cp.Quantity
                })
                .ToList();

            return Result.Success(cartProductDtos);
        }

        public async Task<Result<Guid>> GetIdAsync(Guid userId)
        {
            var cartId = await _cartRepository.GetIdByUserIdAsync(userId);

            if (cartId == Guid.Empty)
            {
                var cartResult = await CreateAsync(userId);
                cartId = cartResult.Value.Id;
            }

            return Result.Success(cartId);
        }
    }
}