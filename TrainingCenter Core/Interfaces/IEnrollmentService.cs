using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.DTOs.Enrollment;

namespace TrainingCenter_Core.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentResponseDto>> GetAllEnrollmentsAsync();
        Task<EnrollmentResponseDto?> GetEnrollmentByIdAsync(int id);
        Task<IEnumerable<EnrollmentResponseDto>> GetEnrollmentsByStudentIdAsync(int studentId);
        Task<IEnumerable<EnrollmentResponseDto>> GetEnrollmentsByCourseIdAsync(int courseId);
        Task<EnrollmentResponseDto> CreateEnrollmentAsync(CreateEnrollmentDto createDto);
        Task<EnrollmentResponseDto?> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto updateDto);
        Task<bool> DeleteEnrollmentAsync(int id);
    }
}
