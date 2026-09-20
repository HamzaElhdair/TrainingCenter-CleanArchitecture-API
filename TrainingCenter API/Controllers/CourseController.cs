using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter_Core.DTOs.Course;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_API.Controllers
{
    [Authorize]
    [Route("api/Course")]
    [ApiController]
    public class CourseController : BaseController
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("All", Name = "GetAllCourses")]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetAllCourses()
        {
            var course = await _courseService.GetAllCoursesAsync();
            if (!course.Any())
            {
                return NotFound("No Courses Found !");
            }
            return Ok(course);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("{id:int}", Name = "GetCourseById")]
        public async Task<ActionResult<CourseResponseDto>> GetCourseById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound($"Course with id {id} not found !");
            }
            return Ok(course);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("Create", Name = "CreateCourse")]
        public async Task<ActionResult<CourseResponseDto>> CreateCourse([FromBody] CreateCourseDto createCourse)
        {
            _logger.LogInformation("Admin action: Creating course. AdminId={AdminId}, Title={Title}, IP={IP}",
                CurrentUserId, createCourse.Title, CurrentIp);

            var course = await _courseService.CreateCourseAsync(createCourse);

            _logger.LogInformation("Admin action succeeded: Course created. AdminId={AdminId}, NewCourseId={CourseId}, IP={IP}",
                CurrentUserId, course.CourseId, CurrentIp);

            return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseId }, course);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("Update/{id:int}", Name = "UpdateCourse")]
        public async Task<ActionResult<CourseResponseDto>> UpdateCourse(int id, [FromBody] UpdateCourseDto updateCourse)
        {
            _logger.LogInformation("Admin action: Updating course. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, id, CurrentIp);

            var course = await _courseService.UpdateCourseAsync(id, updateCourse);

            if (course == null)
            {
                _logger.LogWarning("Admin action failed (course not found for update). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, id, CurrentIp);

                return NotFound($"Course with ID {id} not found.");
            }

            _logger.LogInformation("Admin action succeeded: Course updated. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, id, CurrentIp);

            return Ok(course);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("Delete/{id:int}", Name = "DeleteCourse")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            _logger.LogInformation("Admin action started: Deleting course. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, id, CurrentIp);

            var isDeleted = await _courseService.DeleteCourseAsync(id);

            if (!isDeleted)
            {
                _logger.LogWarning("Admin action failed (course not found for deletion). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, id, CurrentIp);

                return NotFound($"Course with ID {id} not found.");
            }

            _logger.LogInformation("Admin action succeeded: Course deleted. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, id, CurrentIp);

            return NoContent();
        }
    }

}
