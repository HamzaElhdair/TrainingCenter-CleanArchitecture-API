using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using TrainingCenter_Core.DTOs.Auth;
using TrainingCenter_Core.DTOs.User;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        [EnableRateLimiting("AuthLimiter")]
        [HttpPost("Login", Name = "LoginUser")]

        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginUserDto loginUserDto)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var AuthResult = await _userService.LoginAsync(loginUserDto);
            if (AuthResult == null)
            {

                _logger.LogWarning("Failed login attempt. Email={Email}, IP={IP}", loginUserDto.Email, ip);
                return Unauthorized("Invalid credentials");

            }


            return Ok(AuthResult);
        }


        [EnableRateLimiting("AuthLimiter")]
        [HttpPost("refresh")]

        public async Task<ActionResult<TokenResponse>> RefreshToken([FromBody] RefreshRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var tokenResponse = await _userService.RefreshTokenAsync(request);
            if (tokenResponse == null)
            {

                _logger.LogWarning("Invalid refresh attempt. IP={IP}", ip);
                return Unauthorized(new { message = "Invalid credentials" });

            }


            return Ok(tokenResponse);
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var success = await _userService.LogoutAsync(userId);
            if (!success)
            {
                return BadRequest(new { message = "Logout failed." });
            }

            return Ok(new { message = "Logged out successfully." });
        }


    }
}
