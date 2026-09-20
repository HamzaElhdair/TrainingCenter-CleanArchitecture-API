using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter_Core.DTOs.Enrollment;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_API.Controllers
{
    [Authorize]
    [Route("api/Enrollment")]
    [ApiController]
    public class EnrollmentController : BaseController
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(IEnrollmentService enrollmentService, ILogger<EnrollmentController> logger)
        {
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("All", Name = "GetAllEnrollments")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetAllEnrollmentsAsync()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();

            if (!enrollments.Any())
            {
                return NotFound("No Enrollments Found");
            }

            return Ok(enrollments);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{enrollmentId:int}", Name = "GetEnrollmentById")]
        public async Task<ActionResult<EnrollmentResponseDto>> GetEnrollmentByIdAsync(int enrollmentId)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(enrollmentId);

            if (enrollment == null)
            {
                return NotFound($"Enrollment with ID: {enrollmentId} Not Found !");
            }

            return Ok(enrollment);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("Student/{studentId:int}", Name = "GetEnrollmentsByStudentId")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetEnrollmentsByStudentIdAsync(int studentId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByStudentIdAsync(studentId);

            if (!enrollments.Any())
            {
                return NotFound($"No Enrollments Found for Student with ID: {studentId}");
            }

            return Ok(enrollments);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("Course/{courseId:int}", Name = "GetEnrollmentsByCourseId")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetEnrollmentsByCourseIdAsync(int courseId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByCourseIdAsync(courseId);

            if (!enrollments.Any())
            {
                return NotFound($"No Enrollments Found for Course with ID: {courseId}");
            }

            return Ok(enrollments);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("Create", Name = "CreateEnrollment")]
        public async Task<ActionResult<EnrollmentResponseDto>> CreateEnrollmentAsync([FromBody] CreateEnrollmentDto createEnrollmentDto)
        {
            try
            {

                _logger.LogInformation("Admin action: Creating enrollment. AdminId={AdminId}, StudentId={StudentId}, CourseId={CourseId}, IP={IP}",
                    CurrentUserId, createEnrollmentDto.StudentId, createEnrollmentDto.CourseId, CurrentIp);

                var enrollment = await _enrollmentService.CreateEnrollmentAsync(createEnrollmentDto);

                _logger.LogInformation("Admin action succeeded: Enrollment created. AdminId={AdminId}, EnrollmentId={EnrollmentId}, IP={IP}",
                    CurrentUserId, enrollment.EnrollmentId, CurrentIp);

                return CreatedAtAction(nameof(GetEnrollmentByIdAsync), new { enrollmentId = enrollment.EnrollmentId }, enrollment);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Admin action failed (ArgumentException). AdminId={AdminId}, Error={Error}, IP={IP}",
                    CurrentUserId, ex.Message, CurrentIp);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Admin action failed (InvalidOperationException). AdminId={AdminId}, Error={Error}, IP={IP}",
                    CurrentUserId, ex.Message, CurrentIp);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("Update/{enrollmentId:int}", Name = "UpdateEnrollment")]
        public async Task<ActionResult<EnrollmentResponseDto>> UpdateEnrollmentAsync(int enrollmentId, [FromBody] UpdateEnrollmentDto updateEnrollmentDto)
        {
            try
            {
                _logger.LogInformation("Admin action: Updating enrollment. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, enrollmentId, CurrentIp);

                var enrollment = await _enrollmentService.UpdateEnrollmentAsync(enrollmentId, updateEnrollmentDto);

                if (enrollment == null)
                {
                    _logger.LogWarning("Admin action failed (enrollment not found for update). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                        CurrentUserId, enrollmentId, CurrentIp);

                    return NotFound($"Enrollment with ID: {enrollmentId} Not Found.");
                }

                _logger.LogInformation("Admin action succeeded: Enrollment updated. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, enrollmentId, CurrentIp);

                return Ok(enrollment);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning("Admin action failed (ArgumentOutOfRangeException). AdminId={AdminId}, Error={Error}, IP={IP}",
                    CurrentUserId, ex.Message, CurrentIp);
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("Delete/{enrollmentId:int}", Name = "DeleteEnrollment")]
        public async Task<ActionResult> DeleteEnrollmentAsync(int enrollmentId)
        {
            _logger.LogInformation("Admin action started: Deleting enrollment. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, enrollmentId, CurrentIp);

            var isDeleted = await _enrollmentService.DeleteEnrollmentAsync(enrollmentId);

            if (!isDeleted)
            {
                _logger.LogWarning("Admin action failed (enrollment not found for deletion). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, enrollmentId, CurrentIp);

                return NotFound($"Enrollment with ID: {enrollmentId} Not Found.");
            }

            _logger.LogInformation("Admin action succeeded: Enrollment deleted. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, enrollmentId, CurrentIp);

            return NoContent();
        }
    }
}
