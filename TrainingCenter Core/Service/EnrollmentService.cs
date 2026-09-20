using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.DTOs.Enrollment;
using TrainingCenter_Core.Entities;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_Core.Service
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository,
                                IStudentRepository studentRepository,
                                 ICourseRepository courseRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }
        private static EnrollmentResponseDto MapToResponseDto(Enrollment enrollment)
        {
            return new EnrollmentResponseDto(
                enrollment.EnrollmentId,
                enrollment.StudentId,
                enrollment.CourseId,
                enrollment.EnrollmentDate,
                enrollment.CompletionDate,
                enrollment.ProgressPercent,
                enrollment.FinalGrade,
                enrollment.Status
            );
        }

        public async Task<IEnumerable<EnrollmentResponseDto>> GetAllEnrollmentsAsync()
        {
            var enrollment = await _enrollmentRepository.GetAllAsync();
            return enrollment.Select(MapToResponseDto);
        }
        public async Task<EnrollmentResponseDto?> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);
            if (enrollment == null) return null;

            return MapToResponseDto(enrollment);
        }

        public async Task<IEnumerable<EnrollmentResponseDto>> GetEnrollmentsByStudentIdAsync(int studentId)
        {
            var enrollment2 = await _enrollmentRepository.GetByStudentIdAsync(studentId);
            return enrollment2.Select(MapToResponseDto);
        }
        public async Task<IEnumerable<EnrollmentResponseDto>> GetEnrollmentsByCourseIdAsync(int courseId)
        {
            var enrollment3 = await _enrollmentRepository.GetByCourseIdAsync(courseId);
            return enrollment3.Select(MapToResponseDto);
        }
        public async Task<EnrollmentResponseDto> CreateEnrollmentAsync(CreateEnrollmentDto createDto)
        {
            var student = _studentRepository.GetByIdAsync(createDto.StudentId);
            if (student == null)
                throw new ArgumentException($"Student with ID {createDto.StudentId} does not exist.");

            var course = _courseRepository.GetByIdAsync(createDto.CourseId);
            if (course == null) throw new ArgumentException($"Course with ID {createDto.CourseId} does not exist.");

            var existingEnrollments = await _enrollmentRepository.GetByStudentIdAsync(createDto.StudentId);
            if (existingEnrollments.Any(e => e.CourseId == createDto.CourseId))
            {
                throw new InvalidOperationException($"Student with ID {createDto.StudentId} is already enrolled in Course {createDto.CourseId}.");
            }

            if (createDto.ProgressPercent < 0 || createDto.ProgressPercent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(createDto.ProgressPercent), "Progress percent must be between 0 and 100.");
            }

            var enrollment = new Enrollment
            {
                StudentId = createDto.StudentId,
                CourseId = createDto.CourseId,
                CompletionDate = createDto.CompletionDate,
                ProgressPercent = createDto.ProgressPercent,
                Status = string.IsNullOrWhiteSpace(createDto.Status) ? "Active" : createDto.Status
            };

            await _enrollmentRepository.AddAsync(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            return MapToResponseDto(enrollment);
        }
        public async Task<EnrollmentResponseDto?> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto updateDto)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);
            if (enrollment == null) return null;


            if (updateDto.ProgressPercent < 0 || updateDto.ProgressPercent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(updateDto.ProgressPercent), "Progress percent must be between 0 and 100.");
            }


            if (updateDto.FinalGrade.HasValue && (updateDto.FinalGrade.Value < 0 || updateDto.FinalGrade.Value > 100))
            {
                throw new ArgumentOutOfRangeException(nameof(updateDto.FinalGrade), "Final grade must be between 0 and 100.");
            }

            enrollment.ProgressPercent = updateDto.ProgressPercent;
            enrollment.FinalGrade = updateDto.FinalGrade;
            enrollment.Status = updateDto.Status;
            enrollment.CompletionDate = updateDto.CompletionDate;


            if ((enrollment.ProgressPercent == 100 || enrollment.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                && !enrollment.CompletionDate.HasValue)
            {
                enrollment.CompletionDate = DateTime.UtcNow;
                enrollment.Status = "Completed";
            }

            _enrollmentRepository.Update(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            return MapToResponseDto(enrollment);
        }
        public async Task<bool> DeleteEnrollmentAsync(int id)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);
            if (enrollment == null) return false;

            _enrollmentRepository.Delete(enrollment);
            return await _enrollmentRepository.SaveChangesAsync();
        }

    }
}
