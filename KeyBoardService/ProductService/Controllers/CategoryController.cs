using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Configurations;
using ProductService.API.Contracts.Categories;
using ProductService.API.Error;
using ProductService.API.Interfaces.Services;

namespace ProductService.API.Controllers
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

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="request">The request containing the category data to be created.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpGet("/categories")]
        public async Task<IActionResult> GetAllAsync()
        {
            var categoriesResult = await _categoryService.GetAllAsync();

            if (categoriesResult.IsFailure)
                return NotFound(categoriesResult.Error);

            return Ok(categoriesResult.Value);
        }

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns></returns>
        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpGet("/categories/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var categoryResult = await _categoryService.GetByIdAsync(id);

            if (categoryResult.IsFailure)
                return NotFound(categoryResult.Error);

            return Ok(categoryResult.Value);
        }

        /// <summary>
        /// Updates a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="request">The request containing the updated category data.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns></returns>
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
