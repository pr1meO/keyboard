using AuthService.API.Contracts.Users;
using AuthService.API.Error;
using AuthService.API.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IValidator<RegisterUserRequest> _registerValidator;
        private readonly IValidator<LoginUserRequest> _loginValidator;

        public AuthController(
            IIdentityService authService,
            IValidator<RegisterUserRequest> registerValidator,
            IValidator<LoginUserRequest> loginValidator
        )
        {
            _identityService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }


        /// <summary>
        /// Handles the user registration process.
        /// </summary>
        /// <param name="request">The registration data provided by the user.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]  // 200 - OK
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // 400 - BadRequest
        [ProducesResponseType(StatusCodes.Status409Conflict)]  // 409 - Conflict
        [HttpPost("/register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var result = await _identityService.RegisterAsync(request);

            if (result.IsFailure)
                return Conflict(result.Error);

            return Ok();
        }

        /// <summary>
        /// Handles the login process for a user.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]  // 200 - OK
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // 400 - BadRequest
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]  // 401 - Unauthorized
        [HttpPost("/login")]
        public async Task<IActionResult> LoginAsync(LoginUserRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(ErrorFormatter.Deserialize(validationResult.Errors));

            var result = await _identityService.LoginAsync(request);

            if (result.IsFailure)
                return Unauthorized(result.Error);

            return Ok(result.Value);
        }
    }
}
