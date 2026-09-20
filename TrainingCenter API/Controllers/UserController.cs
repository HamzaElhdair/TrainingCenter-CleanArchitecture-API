using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter_Core.DTOs.User;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_API.Controllers
{
    [Authorize]
    [Route("api/User")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("All", Name = "GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (!users.Any())
            {
                return NotFound("No Users Found !");
            }
            return Ok(users);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("by-email", Name = "GetUserByEmail")]
        public async Task<ActionResult<UserResponseDto>> GetUserByEmail([FromQuery] string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound($"User with Email {email} not found.");
            }
            return Ok(user);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{UserId:int}", Name = "GetUserById")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int UserId)
        {
            var user = await _userService.GetUserByIdAsync(UserId);

            if (user == null)
            {
                return NotFound($"User with ID {UserId} not found.");
            }
            return Ok(user);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("Create", Name = "CreateUser")]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            // 📝 Audit Log for creating a user
            _logger.LogInformation("SuperAdmin action: Creating user. AdminId={AdminId}, Email={Email}, IP={IP}",
                CurrentUserId, createUserDto.Email, CurrentIp);

            var user = await _userService.CreateUserAsync(createUserDto);

            _logger.LogInformation("SuperAdmin action succeeded: User created. AdminId={AdminId}, NewUserId={UserId}, IP={IP}",
                CurrentUserId, user.UserId, CurrentIp);

            return CreatedAtAction(nameof(GetUserById), new { UserId = user.UserId }, user);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("Update/{UserId:int}", Name = "UpdateUser")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(int UserId, [FromBody] UpdateUserDto updateUserDto)
        {
            _logger.LogInformation("SuperAdmin action: Updating user. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, UserId, CurrentIp);

            var user = await _userService.UpdateUserAsync(UserId, updateUserDto);

            if (user == null)
            {
                _logger.LogWarning("SuperAdmin action failed (user not found for update). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, UserId, CurrentIp);

                return NotFound($"User with ID {UserId} not found.");
            }

            _logger.LogInformation("SuperAdmin action succeeded: User updated. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, UserId, CurrentIp);

            return Ok(user);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("Delete/{UserId:int}", Name = "DeleteUser")]
        public async Task<ActionResult> DeleteUser(int UserId)
        {
            _logger.LogInformation("SuperAdmin action started: Deleting user. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, UserId, CurrentIp);

            var isDeleted = await _userService.DeleteUserAsync(UserId);

            if (!isDeleted)
            {
                _logger.LogWarning("SuperAdmin action failed (user not found for deletion). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, UserId, CurrentIp);

                return NotFound($"User with ID {UserId} not found.");
            }

            _logger.LogInformation("SuperAdmin action succeeded: User deleted. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, UserId, CurrentIp);

            return NoContent();
        }
    }
}
