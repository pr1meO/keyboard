using BusinessService.Contracts;
using BusinessService.Error;
using BusinessService.Interfaces.Auths;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessService.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterUserRequest> _registerValidator;
        private readonly IValidator<LoginUserRequest> _loginValidator;

        public AuthController(
            IAuthService authService,
            IValidator<RegisterUserRequest> registerValidator,
            IValidator<LoginUserRequest> loginValidator
        )
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var result = await _authService.RegisterAsync(request);

            if (result.IsFailure)
                return Conflict(result.Error);

            return Ok();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> LoginAsync(LoginUserRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var result = await _authService.LoginAsync(request);

            if (result.IsFailure)
                return Unauthorized(result.Error);

            return Ok(result.Value);
        }
    }
}
