using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Configurations;
using ProductService.API.Interfaces.Services;
using System.Security.Claims;

namespace ProductService.API.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Access)]
    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(
            ICartService cartService
        )
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Retrieves all products in the user's cart.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpGet("/cart")]
        public async Task<IActionResult> GetProductsAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var productsResult = await _cartService.GetProductsAsync(Guid.Parse(userId));

            if (productsResult.IsFailure)
                return NotFound(productsResult.Error);

            return Ok(productsResult.Value);
        }
    }
}
