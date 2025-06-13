using BusinessService.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BusinessService.Controllers
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
