using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Configurations;
using ProductService.API.Contracts.Brands;
using ProductService.API.Error;
using ProductService.API.Interfaces.Services;

namespace ProductService.API.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.Access)]
    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IValidator<CreateBrandRequest> _createValidator;
        private readonly IValidator<UpdateBrandRequest> _updateValidator;

        public BrandController(
            IBrandService brandService,
            IValidator<CreateBrandRequest> createValidator,
            IValidator<UpdateBrandRequest> updateValidator
        )
        {
            _brandService = brandService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Creates a new brand.
        /// </summary>
        /// <param name="request">The request containing the brand data to be created.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleTypes.Admin)]
        [HttpPost("/brands")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBrandRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var result = await _brandService.CreateAsync(request);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok();
        }

        /// <summary>
        /// Retrieves all brands.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpGet("/brands")]
        public async Task<IActionResult> GetAllAsync()
        {
            var brandsResult = await _brandService.GetAllAsync();

            if (brandsResult.IsFailure)
                return NotFound(brandsResult.Error);

            return Ok(brandsResult.Value);
        }

        /// <summary>
        /// Retrieves a brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the brand to retrieve.</param>
        /// <returns></returns>
        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpGet("/brands/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var brandResult = await _brandService.GetByIdAsync(id);

            if (brandResult.IsFailure)
                return NotFound(brandResult.Error);

            return Ok(brandResult.Value);
        }

        /// <summary>
        /// Updates a brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the brand to update.</param>
        /// <param name="request">The request containing the updated brand data.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleTypes.Admin)]
        [HttpPut("/brands/{id:guid}")]
        public async Task<IActionResult> UpdateAsync(
            Guid id,
            [FromBody] UpdateBrandRequest request
        )
        {
            var validationResult = await _updateValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var result = await _brandService.UpdateAsync(id, request.Name);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }

        /// <summary>
        /// Deletes a brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the brand to delete.</param>
        /// <returns></returns>
        [Authorize(Roles = RoleTypes.Admin)]
        [HttpDelete("/brands/{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _brandService.DeleteAsync(id);

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok();
        }
    }
}
