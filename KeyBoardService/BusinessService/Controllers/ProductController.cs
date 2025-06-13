using BusinessService.Configurations;
using BusinessService.Contracts.Brands;
using BusinessService.Contracts.Products;
using BusinessService.Error;
using BusinessService.Interfaces.Products;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Access)]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IValidator<CreateProductRequest> _createValidator;
        private readonly IValidator<UpdateProductRequest> _updateValidator;

        public ProductController(
            IProductService productService,
            IValidator<CreateProductRequest> createValidator,
            IValidator<UpdateProductRequest> updateValidator
        )
        {
            _productService = productService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [Authorize(Roles = RoleTypes.Admin)]
        [HttpPost("/products")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProductRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var result = await _productService.CreateAsync(request);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }

        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.User}")]
        [HttpGet("/products")]
        public async Task<IActionResult> GetAllAsync()
        {
            var productsResult = await _productService.GetAllAsync();

            if (productsResult.IsFailure)
                return NotFound(productsResult.Error);

            return Ok(productsResult.Value);
        }

        [Authorize(Roles = $"{RoleTypes.Admin},{RoleTypes.User}")]
        [HttpGet("/products/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var productResult = await _productService.GetByIdAsync(id);

            if (productResult.IsFailure)
                return NotFound(productResult.Error);

            return Ok(productResult.Value);
        }

        [Authorize(Roles = RoleTypes.Admin)]
        [HttpPut("/products/{id:guid}")]
        public async Task<IActionResult> UpdateAsync(
            Guid id,
            [FromBody] UpdateProductRequest request
        )
        {
            var validationResult = await _updateValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var result = await _productService.UpdateAsync(id, request);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }

        [Authorize(Roles = RoleTypes.Admin)]
        [HttpDelete("/products/{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _productService.DeleteAsync(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }
    }
}
