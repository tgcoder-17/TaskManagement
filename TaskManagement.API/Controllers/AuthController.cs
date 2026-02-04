using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Common;
using TaskManagement.API.DTOs.Auth;
using TaskManagement.API.Services.Interfaces;

namespace TaskManagement.API.Controllers
{
    /// <summary>
    /// Handles user authentication and authorization
    /// </summary>
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user and returns a JWT access token
        /// </summary>
        /// <param name="dto">User registration details</param>
        /// <returns>JWT token with expiry information</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var authResponse = await _authService.RegisterAsync(dto);
            return Ok(ApiResponse<AuthResponseDto>.Success(authResponse));
        }

        /// <summary>
        /// Authenticates a user and returns a JWT access token
        /// </summary>
        /// <param name="dto">User login credentials</param>
        /// <returns>JWT token with expiry information</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var authResponse = await _authService.LoginAsync(dto);
            return Ok(ApiResponse<AuthResponseDto>.Success(authResponse));

        }
    }
}
