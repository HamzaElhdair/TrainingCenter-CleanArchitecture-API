using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter_Core.DTOs.Instructor;
using TrainingCenter_Core.Interfaces;
using TrainingCenter_API.Authorization;

namespace TrainingCenter_API.Controllers
{

    [Authorize]
    [Route("api/Instructor")]
    [ApiController]
    public class InstructorController : BaseController
    {
        private readonly IInstructorService _instructorService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<InstructorController> _logger;

        public InstructorController(
            IInstructorService instructorService,
            IAuthorizationService authorizationService,
            ILogger<InstructorController> logger)
        {
            _instructorService = instructorService;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("All", Name = "GetAllInstructors")]
        public async Task<ActionResult<IEnumerable<InstructorResponseDto>>> GetAllInstructorsAsync()
        {
            var instructors = await _instructorService.GetAllInstructorsAsync();

            if (!instructors.Any())
            {
                return NotFound("No Instructors Found");
            }
            return Ok(instructors);
        }

        [Authorize]
        [HttpGet("{instructorId:int}", Name = "GetInstructorById")]
        public async Task<ActionResult<InstructorResponseDto>> GetInstructorByIdAsync(int instructorId)
        {
            if (instructorId < 1)
                return BadRequest("Invalid Id");

            var instructor = await _instructorService.GetInstructorByIdAsync(instructorId);

            if (instructor == null)
            {
                return NotFound($"Instructor with id :{instructorId} Not Found !");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, instructor, new SameUserOrAdminRequirement());

            if (!authorizationResult.Succeeded)
            {

                _logger.LogWarning("Unauthorized access attempt to instructor profile. UserId={UserId}, TargetInstructorId={TargetId}, IP={IP}",
                    CurrentUserId, instructorId, CurrentIp);

                return User.Identity?.IsAuthenticated == true ? Forbid() : Unauthorized();
            }

            return Ok(instructor);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("Create", Name = "CreateInstructor")]
        public async Task<ActionResult<InstructorResponseDto>> CreateInstructorAsync([FromBody] CreateInstructorDto createInstructorDto)
        {
            _logger.LogInformation("Admin action: Creating instructor. AdminId={AdminId}, Email={Email}, IP={IP}",
                CurrentUserId, createInstructorDto.Email, CurrentIp);

            var instructor = await _instructorService.CreateInstructorAsync(createInstructorDto);

            _logger.LogInformation("Admin action succeeded: Instructor created. AdminId={AdminId}, NewInstructorId={InstructorId}, IP={IP}",
                CurrentUserId, instructor.InstructorId, CurrentIp);

            return CreatedAtAction(nameof(GetInstructorByIdAsync), new { instructorId = instructor.InstructorId }, instructor);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("Update/{instructorId:int}", Name = "UpdateInstructor")]
        public async Task<ActionResult<InstructorResponseDto>> UpdateInstructorAsync(int instructorId, [FromBody] UpdateInstructorDto updateInstructorDto)
        {
            _logger.LogInformation("Admin action: Updating instructor. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, instructorId, CurrentIp);

            var instructor = await _instructorService.UpdateInstructorAsync(instructorId, updateInstructorDto);

            if (instructor == null)
            {
                _logger.LogWarning("Admin action failed (instructor not found for update). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, instructorId, CurrentIp);

                return NotFound($"Instructor with ID {instructorId} not found.");
            }

            _logger.LogInformation("Admin action succeeded: Instructor updated. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, instructorId, CurrentIp);

            return Ok(instructor);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("Delete/{instructorId:int}", Name = "DeleteInstructor")]
        public async Task<ActionResult> DeleteInstructorAsync(int instructorId)
        {
            _logger.LogInformation("Admin action started: Deleting instructor. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, instructorId, CurrentIp);

            var isDeleted = await _instructorService.DeleteInstructorAsync(instructorId);

            if (!isDeleted)
            {
                _logger.LogWarning("Admin action failed (instructor not found for deletion). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, instructorId, CurrentIp);

                return NotFound($"Instructor with ID {instructorId} not found.");
            }

            _logger.LogInformation("Admin action succeeded: Instructor deleted. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, instructorId, CurrentIp);

            return NoContent();
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("user/{UserId:int}", Name = "GetInstructorByUserId")]
        public async Task<ActionResult<InstructorResponseDto>> GetInstructorByUserId(int UserId)
        {
            var instructor = await _instructorService.GetInstructorByUserIdAsync(UserId);
            if (instructor == null)
            {
                return NotFound($"Instructor with ID {UserId} not found.");
            }
            return Ok(instructor);
        }
    }
}
