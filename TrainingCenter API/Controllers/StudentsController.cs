using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter_Core.DTOs.Student;
using TrainingCenter_Core.DTOs.StudentProfile;
using TrainingCenter_Core.Interfaces;
using TrainingCenter_API.Authorization;

namespace TrainingCenter_API.Controllers
{
    [Authorize]
    [Route("api/Students")]
    [ApiController]
    public class StudentsController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(
            IStudentService studentService,
            IAuthorizationService authorizationService,
            ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("All", Name = "GetAllStudents")]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();

            if (!students.Any())
            {
                return NotFound("No students found.");
            }

            return Ok(students);
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetStudentById")]
        public async Task<ActionResult<StudentResponseDto>> GetStudentById(int id)
        {
            if (id < 1)
                return BadRequest("Invalid student id.");

            var student = await _studentService.GetStudentByIdAsync(id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, student, new SameUserOrAdminRequirement());

            if (!authorizationResult.Succeeded)
            {

                _logger.LogWarning("Unauthorized access attempt to student profile. UserId={UserId}, TargetStudentId={TargetId}, IP={IP}",
                    CurrentUserId, id, CurrentIp);

                return User.Identity?.IsAuthenticated == true ? Forbid() : Unauthorized();
            }

            return Ok(student);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("Create", Name = "CreateStudent")]
        public async Task<ActionResult<StudentResponseDto>> CreateStudent([FromBody] CreateStudentDto createStudentDto)
        {

            _logger.LogInformation("Admin action: Creating student. AdminId={AdminId}, Email={Email}, IP={IP}",
                CurrentUserId, createStudentDto.Email, CurrentIp);

            var student = await _studentService.CreateStudentAsync(createStudentDto);

            _logger.LogInformation("Admin action succeeded: Student created. AdminId={AdminId}, NewStudentId={StudentId}, IP={IP}",
                CurrentUserId, student.StudentId, CurrentIp);

            return CreatedAtAction("GetStudentById", new { id = student.StudentId }, student);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("Update/{id:int}", Name = "UpdateStudent")]
        public async Task<ActionResult<StudentResponseDto>> UpdateStudent(int id, [FromBody] UpdateStudentDto updateStudentDto)
        {
            _logger.LogInformation("Admin action: Updating student. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, id, CurrentIp);

            var updatedStudent = await _studentService.UpdateStudentAsync(id, updateStudentDto);

            if (updatedStudent == null)
            {
                _logger.LogWarning("Admin action failed (student not found for update). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, id, CurrentIp);

                return NotFound($"Student with ID {id} not found.");
            }

            _logger.LogInformation("Admin action succeeded: Student updated. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, id, CurrentIp);

            return Ok(updatedStudent);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("Delete/{id:int}", Name = "DeleteStudent")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            _logger.LogInformation("Admin action started: Deleting student. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, id, CurrentIp);

            var isDeleted = await _studentService.DeleteStudentAsync(id);

            if (!isDeleted)
            {
                _logger.LogWarning("Admin action failed (student not found for deletion). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, id, CurrentIp);

                return NotFound($"Student with ID {id} not found.");
            }

            _logger.LogInformation("Admin action succeeded: Student deleted. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, id, CurrentIp);

            return NoContent();
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{studentId:int}/profile")]
        public async Task<ActionResult> UpdateProfile(int studentId, [FromBody] UpdateStudentProfileDto updateProfileDto)
        {
            var isUpdated = await _studentService.UpdateStudentProfileAsync(studentId, updateProfileDto);

            if (!isUpdated)
            {
                _logger.LogWarning("Admin action failed (profile update student not found). AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                    CurrentUserId, studentId, CurrentIp);
                return NotFound($"Student with ID {studentId} not found.");
            }

            _logger.LogInformation("Admin action succeeded: Profile updated. AdminId={AdminId}, TargetId={TargetId}, IP={IP}",
                CurrentUserId, studentId, CurrentIp);

            return NoContent();
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("user/{UserId:int}", Name = "GetStudentByUserId")]
        public async Task<ActionResult<StudentResponseDto>> GetStudentByUserId(int UserId)
        {
            var student = await _studentService.GetStudentByUserIdAsync(UserId);
            if (student == null)
            {
                return NotFound($"Student with ID {UserId} not found.");
            }
            return Ok(student);
        }
    }
}
