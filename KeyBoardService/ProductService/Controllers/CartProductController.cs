using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Configurations;
using ProductService.API.Contracts.CartProducts;
using ProductService.API.Error;
using ProductService.API.Interfaces.Services;
using System.Security.Claims;

namespace ProductService.API.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Access)]
    [ApiController]
    [Route("[controller]")]
    public class CartProductController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICartProductService _cartProductService;
        private readonly IValidator<CreateCartProductRequest> _createValidator;
        private readonly IValidator<UpdateCartProductRequest> _updateValidator;

        public CartProductController(
            ICartService cartService,
            ICartProductService cartProductService,
            IValidator<CreateCartProductRequest> createValidator,
            IValidator<UpdateCartProductRequest> updateValidator
        )
        {
            _cartService = cartService;
            _cartProductService = cartProductService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Adds a product to the cart.
        /// </summary>
        /// <param name="request">The request containing the product data to be added to the cart.</param>
        /// <returns></returns>
        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpPost("/cart")]
        public async Task<IActionResult> AddAsync([FromBody] CreateCartProductRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var cartIdResult = await _cartService.GetIdAsync(Guid.Parse(userId));

            var result = await _cartProductService.AddAsync(cartIdResult.Value, request);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }

        /// <summary>
        /// Updates the quantity of a product in the cart.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="request">The request containing the updated quantity data.</param>
        /// <returns></returns>
        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpPut("/cart/{id:guid}")]
        public async Task<IActionResult> UpdateAsync(
            Guid id,
            [FromBody] UpdateCartProductRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var cartIdResult = await _cartService.GetIdAsync(Guid.Parse(userId));

            var result = await _cartProductService.UpdateQuantityAsync(id, cartIdResult.Value, request.Quantity);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }

        /// <summary>
        /// Deletes a product from the cart by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns></returns>
        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpDelete("/cart/{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var cartIdResult = await _cartService.GetIdAsync(Guid.Parse(userId));

            var result = await _cartProductService.DeleteAsync(id, cartIdResult.Value);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }

        /// <summary>
        /// Clears all products from the cart.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpDelete("/cart")]
        public async Task<IActionResult> ClearAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var cartIdResult = await _cartService.GetIdAsync(Guid.Parse(userId));

            var result = await _cartProductService.ClearAsync(cartIdResult.Value);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }
    }
}
