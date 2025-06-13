using BusinessService.Configurations;
using BusinessService.Contracts.Categories;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<CreateCategoryRequest> _createValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateValidator;

        public CategoryController(
            ICategoryService categoryService,
            IValidator<CreateCategoryRequest> createValidator,
            IValidator<UpdateCategoryRequest> updateValidator
        )
        {
            _categoryService = categoryService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [Authorize(Roles = RoleTypes.Admin)]
        [HttpPost("/categories")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var result = await _categoryService.CreateAsync(request);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok();
        }

        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpGet("/categories")]
        public async Task<IActionResult> GetAllAsync()
        {
            var categoriesResult = await _categoryService.GetAllAsync();

            if (categoriesResult.IsFailure)
                return NotFound(categoriesResult.Error);

            return Ok(categoriesResult.Value);
        }

        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpGet("/categories/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var categoryResult = await _categoryService.GetByIdAsync(id);

            if (categoryResult.IsFailure)
                return NotFound(categoryResult.Error);

            return Ok(categoryResult.Value);
        }

        [Authorize(Roles = RoleTypes.Admin)]
        [HttpPut("/categories/{id:guid}")]
        public async Task<IActionResult> UpdateAsync(
            Guid id,
            [FromBody] UpdateCategoryRequest request
        )
        {
            var validationResult = await _updateValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var result = await _categoryService.UpdateAsync(id, request.Name);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }

        [Authorize(Roles = RoleTypes.Admin)]
        [HttpDelete("/categories/{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }
    }
}
