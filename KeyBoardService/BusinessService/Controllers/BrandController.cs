using BusinessService.Configurations;
using BusinessService.Contracts.Brands;
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

        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpGet("/brands")]
        public async Task<IActionResult> GetAllAsync()
        {
            var brandsResult = await _brandService.GetAllAsync();

            if (brandsResult.IsFailure)
                return NotFound(brandsResult.Error);

            return Ok(brandsResult.Value);
        }

        [Authorize(Roles = $"{RoleTypes.Admin}, {RoleTypes.User}")]
        [HttpGet("/brands/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var brandResult = await _brandService.GetByIdAsync(id);

            if (brandResult.IsFailure)
                return NotFound(brandResult.Error);

            return Ok(brandResult.Value);
        }

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
